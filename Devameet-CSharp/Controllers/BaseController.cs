using Devameet_CSharp.Models;
using Devameet_CSharp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Devameet_CSharp.Controllers
{

    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IUserRepository _userRepository;

        public BaseController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        protected User GetToken()
        {
            //pegar informaçao da claims do sId para uma varivel , para que eu tenha qual id do usuario
            var iduser = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(iduser))
            {
                return null;
            }
            else
            {
                return _userRepository.GetUserById(int.Parse(iduser));
            }

        }
    }
    
    
}
