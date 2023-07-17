using Devameet_CSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace Devameet_CSharp.Repository
{
    public interface IUserRepository
    {
        bool VerifyEmail(string email);
        void Save(User user);
        User GetUserByLoginPassword(string login, string password);
    }
}
