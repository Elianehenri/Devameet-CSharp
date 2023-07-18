using Devameet_CSharp.Dtos;
using Devameet_CSharp.Models;
using Devameet_CSharp.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Eventing.Reader;

namespace Devameet_CSharp.Controllers
{
    [ApiController]
    
    public class UserController : BaseController
    {
        public readonly ILogger<UserController> _logger;
     

        public UserController(ILogger<UserController> logger, IUserRepository userRepository) : base(userRepository)
        {
            _logger = logger;
            
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult GetUser()
        {
            try
            {

                User user = GetToken();

                return Ok(new UserResponseDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar
                });

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter o usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet]
        [Route("api/[controller]/GetUserById")]

        public IActionResult GetUserById(int idUser)
        {
            try
            {

                User user = _userRepository.GetUserById(idUser);

                return Ok(new UserResponseDto
                {
                    Name = user.Name,
                    Email = user.Email,
                    Avatar = user.Avatar
                });

            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao obter o usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPut]
        [Route("api/[controller]")]
        public IActionResult UpdateUser([FromBody] UserRequestDto userdto)
        {
            try
            {
                User user = GetToken();

                if (user != null)
                {

                    if (!String.IsNullOrEmpty(user.Name) && !String.IsNullOrWhiteSpace(user.Name) &&
                            !String.IsNullOrEmpty(user.Avatar) && !String.IsNullOrWhiteSpace(user.Avatar))
                    {
                        user.Avatar = userdto.Avatar;
                        user.Name = userdto.Name;

                        _userRepository.UpdateUser(user);


                        return Ok("Usuário foi salvo com sucesso");
                    }

                    else
                    {
                        _logger.LogError("Os dados do usuario devem estar preenchidos corretamente");
                        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto()
                        {
                            Description = "Os dados do usuario devem estar preenchidos corretamente",
                            Status = StatusCodes.Status400BadRequest
                        });
                    }
                }
                else
                {
                    _logger.LogError("Este usuario nao é valido");
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponseDto()
                    {
                        Description = "Este usuario nao é valido",
                        Status = StatusCodes.Status400BadRequest
                    });
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro na atualizaçao dos dados do usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }

        }

    }



}


