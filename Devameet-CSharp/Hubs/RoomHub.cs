using Devameet_CSharp.Dtos;
using Devameet_CSharp.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Devameet_CSharp.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomRepository _roomRepository;
        //toda vez que um cliente se conecta, ele recebe um id
        private string ClientId => Context.ConnectionId;

        public RoomHub(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        //entrada do cliente no hub e com a geraçao do clientId
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Client: {ClientId} connected!");
            await base.OnConnectedAsync();
        }
        //
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Disconnecting client...");
            var userSocket = await _roomRepository.GetUserPosition(ClientId);

            if (userSocket == null)
                return;

            var link = userSocket.Meet.Link;
            await _roomRepository.DeleteUserPosition(ClientId);

            await Clients.Others.SendAsync($"remove-user", new { SocketId = ClientId });

            await base.OnDisconnectedAsync(exception);
        }
        //entrar na sala de video chamada e atualizar a lista de usuarios na sala de video chamada
        //e enviar para os outros usuarios da sala de video chamada que um novo usuario entrou na sala
        public async Task Join(JoinDto dto)
        {
            var link = dto.Link;
            var userId = Int32.Parse(dto.UserId);


            Console.WriteLine("Joining room...");
            var userSocket = await _roomRepository.GetUserPosition(ClientId);
            if (userSocket != null)
            {
                Console.WriteLine("User already in room");
            }
            else
            {
                Console.WriteLine("User ID: " + userId + "ClientId: " + ClientId + " Link: " + link);
                await Groups.AddToGroupAsync(ClientId, link);

                var updatePositionDto = new UpdatePositionDto();
                updatePositionDto.X = 2;
                updatePositionDto.Y = 2;
                updatePositionDto.Orientation = "down";

                await _roomRepository.UpdateUserPosition(userId, link, ClientId, updatePositionDto);
                var users = await _roomRepository.ListUsersPosition(link);

                Console.WriteLine("Sending to client.... Users: " + users.Count + "");
                await Clients.Group(link).SendAsync($"update-user-list", new { Users = users });
                await Clients.OthersInGroup(link).SendAsync($"add-user", new { User = ClientId });
                Console.WriteLine("Sent to client!");
            }
        }
        //movimentaçao da pessoa na sala de video chamada
        //e atualiza a posiçao da pessoa na sala de video chamada para os outros usuarios
        public async Task Move(MoveDto movedto)
        {
            var userId = Int32.Parse(movedto.UserId);
            var link = movedto.Link;

            var updatePositionDto = new UpdatePositionDto();
            updatePositionDto.X = movedto.X;
            updatePositionDto.Y = movedto.Y;
            updatePositionDto.Orientation = movedto.Orientation;

            await _roomRepository.UpdateUserPosition(userId, link, ClientId, updatePositionDto);
            var users = await _roomRepository.ListUsersPosition(link);
            Console.WriteLine("Enviando a nova posiçao para os outros usuarios da sala de video chamada...");
            await Clients.Group(link).SendAsync($"update-user-list", new { Users = users });
        }

        public async Task TogglMuteUser(ToggleMuteDto dto)
        {
            var link = dto.Link;
            await _roomRepository.UpdateUserMute(dto);
            var users = await _roomRepository.ListUsersPosition(link);
            await Clients.Group(link).SendAsync($"update-user-list", new { Users = users });
        }

        public async Task CallUser(CallUserDto dto)
        {
            await Clients.Client(dto.To).SendAsync("call-made", new { Offer = dto.Offer, Socket = ClientId });
        }

        public async Task MakeAnswer(MakeAnswerDto dto)
        {
            await Clients.Client(dto.To).SendAsync("answer-made", new { Answer = dto.Answer, Socket = ClientId });
        }
    }
}
