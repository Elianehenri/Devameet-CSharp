using Devameet_CSharp.Models;

namespace Devameet_CSharp.Repository.Impl
{
    public class UserRepositoryImpl : IUserRepository
    {
        private readonly DevameetContext _context;

        public UserRepositoryImpl(DevameetContext context)
        {
            _context = context;
        }

        public User GetUserByLoginPassword(string login, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
        }

        public void Save(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
        }

        public bool VerifyEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }
    }
}
