using Devameet_CSharp.Dtos;
using Devameet_CSharp.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Devameet_CSharp.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IRoomRepository _roomRepository;
        //toda vez que um cliente se conecta, ele recebe um id
        //Criaçao do ClientId do socket (identificaçao unica dentro so SignalR)
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
            //buscar o usuario na sala de video chamada
            var userSocket = await _roomRepository.GetUserPosition(ClientId);
            
            if (userSocket == null)
                return;
            
            var link = userSocket.Meet.Link;
            //deletar posiçao do user no banco de dados
            await _roomRepository.DeleteUserPosition(ClientId);
            //deletar usuario da sala de video chamada
            await Clients.Others.SendAsync($"remove-user", new { SocketId = ClientId });

            await base.OnDisconnectedAsync(exception);
        }
        //entrar na sala de video chamada e atualizar a lista de usuarios na sala de video chamada
        //e enviar para os outros usuarios da sala de video chamada que um novo usuario entrou na sala
        public async Task Join(JoinDto joindto)
        {
            var link = joindto.Link;
            var userid = Int32.Parse(joindto.UserId);


            Console.WriteLine("Joining room...");
            var userSocket = await _roomRepository.GetUserPosition(ClientId);
            if (userSocket != null)
            {
                Console.WriteLine("Usuário já está na sala");
            }
            else
            {
                Console.WriteLine("O Usuario: " + userid.ToString() + " entrou na sala com o ClientId: " + ClientId + " Link: " + link);
                //adicionando um usuaro novo que chegou na sala de video chamada
                await Groups.AddToGroupAsync(ClientId, link);
                //entrada do usuario na posiçao inicial da sala de video chamada
                //o usuario entrou na posiçao inicial 2,2 e olhando para baixo
                var updatePositionDto = new UpdatePositionDto();
                updatePositionDto.X = 2;
                updatePositionDto.Y = 2;
                updatePositionDto.Orientation = "down";
                //atualizar no banco de dados a posiçao do usuario na sala de video chamada
                await _roomRepository.UpdateUserPosition(userid, link, ClientId, updatePositionDto);
                //lista de usuarios na sala de video chamada
                var users = await _roomRepository.ListUsersPosition(link);

                //devolver a lista de usuarios na sala de video chamada para o usuario que entrou na sala de video chamada
                Console.WriteLine("Enviando para o cliente.... Usuários: " + users.Count + "");
                //atualizar lista de usuarios na sala de video chamada para os outros usuarios da sala de video chamada
                await Clients.Group(link).SendAsync($"update-user-list", new { Users = users });
                //enviar para os outros usuarios da sala de video chamada que um novo usuario entrou na sala de video chamada
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

            //atualizar no banco de dados a posiçao do usuario na sala de video chamada
            await _roomRepository.UpdateUserPosition(userId, link, ClientId, updatePositionDto);
            var users = await _roomRepository.ListUsersPosition(link);
            Console.WriteLine("Enviando a nova posiçao para os outros usuarios da sala de video chamada...");
            await Clients.Group(link).SendAsync($"update-user-list", new { Users = users });
        }
        //deixa mute o usuario na sala de video chamada
        public async Task UpdadeMuteUser(MuteDto mutedto)
        {
            var link = mutedto.Link;
            //atualizar no banco de dados o mute do usuario na sala de video chamada
            await _roomRepository.UpdateUserMute(mutedto);
            var users = await _roomRepository.ListUsersPosition(link);
            //avisar para os outros usuarios da sala de video chamada que o usuario foi mutado
            await Clients.Group(link).SendAsync($"update-user-list", new { Users = users });
        }

        //conectar com o outro usuario da sala de video chamada
        public async Task CallUser(CallUserDto callUserdto)
        {
            await Clients.Client(callUserdto.To).SendAsync("call-made", new { Offer = callUserdto.Offer, Socket = ClientId });
        }
        //resposta do outro usuario da sala de video chamada
        public async Task MakeAnswer(MakeAnswerDto makeAnswerdto)
        {
            await Clients.Client(makeAnswerdto.To).SendAsync("answer-made", new { Answer = makeAnswerdto.Answer, Socket = ClientId });
        }
    }
}
