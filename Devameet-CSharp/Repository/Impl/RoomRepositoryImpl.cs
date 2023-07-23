using Devameet_CSharp.Dtos;
using Devameet_CSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace Devameet_CSharp.Repository.Impl
{
    public class RoomRepositoryImpl : IRoomRepository
    {
        private readonly DevameetContext _context;

        public RoomRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }

        public async Task<Meet> GetRoom(string link)
        {
            // TODO: Include Meet Objects
            return await _context.Meets.Where(m => m.Link == link).FirstAsync();
        }

        //listar todos os usuarios na sala de video chamada
        public async Task<ICollection<PositionDto>> ListUsersPosition(string link)
        {
            //buscar o id da meet
            var meet = await _context.Meets.Where(m => m.Link == link).FirstOrDefaultAsync();
            //buscar todos os usuarios na sala de video chamada
            var rooms = await _context.Rooms.Where(r => r.MeetId == meet.Id).ToListAsync();
            return rooms.Select(r => new PositionDto
            {
                X = r.X,
                Y = r.Y,
                Orientation = r.Orientation,
                Id = r.Id,
                Name = r.UserName,
                Avatar = r.Avatar,
                Muted = r.Muted,
                Meet = r.Meet.Link,
                User = r.UserId.ToString(),
                ClientId = r.ClientId
            }).ToList();
        }

        public Task<Room?> GetUserPosition(string clientId)
        {
            return _context.Rooms.Where(r => r.ClientId == clientId)
                .Include(room => room.Meet)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteUserPosition(string clientId)
        {
            //encontrar usuario na sala de video chamada
            var room = await _context.Rooms.Where(r => r.ClientId == clientId).ToListAsync();
            //deletar usuario da sala de video chamada
            _context.Rooms.RemoveRange(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserPosition(int userId, string link, string clientId, UpdatePositionDto dto)
        {
            //recuperar o id da meet
            var meet = await _context.Meets.Where(m => m.Link == link).FirstOrDefaultAsync();
            //buscar os dados do user
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            //verificar quantas pessaos estao na sala
            var usersInRoom = await _context.Rooms.Where(r => r.MeetId == meet.Id).ToListAsync();
           //colocar limtaçao de pessoas na sala
            if (usersInRoom.Count > 20)
                throw new Exception("A sala está cheia");
            //verificar se a pessoa ja esta na sala
            if (usersInRoom.Any(r => r.ClientId == clientId || r.UserId == userId))
            {
                var position = await _context.Rooms.Where(r => r.ClientId == clientId || r.UserId == userId).FirstOrDefaultAsync();
                position.X = dto.X;
                position.Y = dto.Y;
                position.Orientation = dto.Orientation;
            }
            else
            {
                //se ele na existir na sala, criar um novo
                var room = new Room();
                room.X = dto.X;
                room.Y = dto.Y;
                room.Orientation = dto.Orientation;
                room.ClientId = clientId;
                room.UserId = user.Id;
                room.MeetId = meet.Id;
                room.UserName = user.Name;
                room.Avatar = user.Avatar;

                await _context.Rooms.AddAsync(room);
            }

            await _context.SaveChangesAsync();
        }

        //mutar e desmutar
        public async Task UpdateUserMute(MuteDto mutedto)
        {
            //pegar o id da meet
            var meet = await _context.Meets.Where(m => m.Link == mutedto.Link).FirstAsync();
            //pegar o id do user
            var user = await _context.Users.Where(u => u.Id == Int32.Parse(mutedto.UserId)).FirstAsync();
            //pegar o registro do usuario na sala
            var room = await _context.Rooms.Where(r => r.MeetId == meet.Id && r.UserId == user.Id).FirstAsync();
            //mudar o status do mute
            room.Muted = mutedto.Muted;
            await _context.SaveChangesAsync();


        }

        public Meet GetRoomById(int meetid)
        {
            //pegar primeiro o meetid da sala de reunião
                      
            Meet meet = _context.Meets.Where(m => m.Id == meetid).FirstOrDefault();
            //depois coloca todos os objetos do meet
            meet.MeetObjects = _context.MeetObjects.Where(o => o.MeetId == meetid).ToList();

            return meet;
        }
    }
}

