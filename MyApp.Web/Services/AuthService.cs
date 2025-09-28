using MyApp.Web.Data;
using MyApp.Web.Models.Entities;
using System.Linq;

namespace MyApp.Web.Services
{
    public class AuthService
    {
        private readonly DarbuContext _context;

        public AuthService(DarbuContext context)
        {
            _context = context;
        }

        public User? Authenticate(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return user;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}