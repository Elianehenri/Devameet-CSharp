using Devameet_CSharp.Dtos;
using Devameet_CSharp.Models;
using Devameet_CSharp.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

namespace Devameet_CSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetController : BaseController
    {
        public readonly ILogger<MeetController> _logger;
        private readonly IMeetRepository _meetRepository;
        private readonly IMeetObjectRepository _meetObjectRepository;

        public MeetController(ILogger<MeetController> logger, IUserRepository userRepository, IMeetRepository meetRepository, IMeetObjectRepository meetObjectRepository) : base(userRepository)
        {
            _logger = logger;
            _meetRepository = meetRepository;
            _meetObjectRepository = meetObjectRepository;
        }

        [HttpGet]
     
        public IActionResult GetMeet()
        {
            try
            {
                //lista da sala de reunioes do user
                User user = GetToken();

                List<Meet> meets = _meetRepository.GetMeetByUser(user.Id);

                return Ok(meets);

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter  a sala de reunião");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
            
        }
        [HttpGet]
        [Route("objects")]
        public IActionResult GetMeetObjects(int meetid)
        {
            try
            {
                //lista da sala de reunioes do user
                User user = GetToken();

                //List<MeetObjects> meets = _meetObjectRepository.GetMeetObjectsByMeet(meetid);

                

                return Ok(_meetObjectRepository.GetMeetObjectsById(meetid));

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter  os objetos da sala de reunião");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu um erro ao obter  os objetos da sala de reunião: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }

        }

        [HttpPost]
    
        public IActionResult CreateMeet([FromBody] MeetRequestDto meetRequestDto)
        {
            try
            {
                if (String.IsNullOrEmpty(meetRequestDto.Name) || String.IsNullOrWhiteSpace(meetRequestDto.Name))
                {
                    _logger.LogError("O nome da sala de reunião precisa ser preenchido");
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto()
                    {
                        Description = "O nome da sala de reunião precisa ser preenchido",
                        Status = StatusCodes.Status400BadRequest
                    });
                }
                else
                {
                    Meet meet = new Meet();
                    meet.Name = meetRequestDto.Name;
                    meet.Color = meetRequestDto.Color;
                    meet.UserId = GetToken().Id;
                    meet.Link = Guid.NewGuid().ToString();

                    _meetRepository.CreateMeet(meet);

                    return Ok("Sala de reunião salva com sucesso");
                }


            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao inlcuir a sala de reunião");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPut]
      
        public IActionResult UpdateMeet([FromBody] MeetUpdateRequestDto meetUpdateRequestDto, int meetId)

        {
            try
            {
                Meet meet = _meetRepository.GetMeetById(meetId);
                meet.Name = meetUpdateRequestDto.Name;
                meet.Color = meetUpdateRequestDto.Color.ToString();
                _meetRepository.UpdateMeet(meet);
                List<MeetObjects> meetObjects = new List<MeetObjects>();
                //percorrer a lista de objetos da sala de reuniao
                foreach (MeetObjectDto objectsDto in meetUpdateRequestDto.Objects)
                {
                    MeetObjects meetObj = new MeetObjects();
                    meetObj.Name = objectsDto.Name;
                    meetObj.X = objectsDto.X;
                    meetObj.Y = objectsDto.Y;
                    meetObj.Orientation = objectsDto.Orientation;
                    meetObj.MeetId = meet.Id;
                    meetObj.ZIndex = objectsDto.ZIndex;
                    meetObj.Walkable = objectsDto.Walkable == null ? true : false ;
                    meetObjects.Add(meetObj);
                }
                _meetObjectRepository.CreateObjectsMeet(meetObjects, meetId);
                return Ok("Sala de reunião atualizada com sucesso");

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao inlcuir a sala de reunião");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpDelete]
        
        public IActionResult DeleteMeet(int meetid)
        {
            try
            {
                _meetRepository.DeleteMeet(meetid);
                return Ok("Sala de reunião excluída com sucesso");

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao deletar a sala de reunião");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
}
