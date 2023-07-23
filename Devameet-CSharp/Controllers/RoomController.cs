using Devameet_CSharp.Dtos;
using Devameet_CSharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Devameet_CSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : BaseController
    {
        public readonly ILogger<MeetController> _logger;
        private readonly IRoomRepository _roomRepository;

        public RoomController(ILogger<MeetController> logger, IUserRepository userRepository, IRoomRepository roomRepository) : base(userRepository)
        {
            _logger = logger;
            _roomRepository = roomRepository;
        }
        [HttpGet]
        public ActionResult GetRoom(int meetid)
        {
            try
            {
              
                return Ok(_roomRepository.GetRoomById(meetid));
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter  os dados da sala de reunião");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu um erro ao obter  os dados da sala de reunião: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
    