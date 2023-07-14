using Devameet_CSharp.Dtos;
using Devameet_CSharp.Models;
using Devameet_CSharp.Repository;
using Devameet_CSharp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Devameet_CSharp.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        //controle de log
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;

        public AuthController(ILogger<AuthController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/[controller]/register")]
        public IActionResult SaveUser([FromBody] UserRegisterDto userdto)
        {
            try
            {

                if (userdto != null)
                {
                    var errors = new List<string>();

                    if (string.IsNullOrEmpty(userdto.Name) || string.IsNullOrWhiteSpace(userdto.Name))
                    {
                        errors.Add("Nome inválido");
                    }
                    if (string.IsNullOrEmpty(userdto.Email) || string.IsNullOrWhiteSpace(userdto.Email) || !userdto.Email.Contains("@"))
                    {
                        errors.Add("E-mail inválido");
                    }
                    if (string.IsNullOrEmpty(userdto.Password) || string.IsNullOrWhiteSpace(userdto.Password))
                    {
                        errors.Add("Senha inválido");
                    }

                    if (errors.Count > 0)
                    {
                        return BadRequest(new ErrorResponseDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Description= "Ocorreu um erro ao salvar o usuário",
                            Errors = errors
                        });
                    }
                    //inserçao de dados
                    User user = new User()
                    {
                        //nao deixar o email em letras email
                        Email = userdto.Email.ToLower(),
                        //criptografar senha
                        Password = MD5Utils.GenerateHashMD5(userdto.Password),
                        Name = userdto.Name,
                        Avatar = userdto.Avatar
                    };
                    //salvar no banco
                    
                    if (!_userRepository.VerifyEmail(user.Email))
                    {
                        _userRepository.Save(user);
                    }
                    else
                    {
                        return BadRequest(new ErrorResponseDto()
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Description = "Usuário já está cadastrado!"
                        });
                    }

                }

                return Ok("Usuário foi salvo com sucesso");
            }
            catch (Exception e)
            {
                _logger.LogError("Ocorreu um erro ao salvar o usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Description = "Ocorreu o seguinte erro: " + e.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
    
}
