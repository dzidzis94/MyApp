using MyApp.Web.Models;
using System.Linq;
using BCrypt.Net;

namespace MyApp.Web.Services
{
    public class AuthService
    {
        private readonly DataService _dataService;

        public AuthService(DataService dataService)
        {
            _dataService = dataService;
        }

        public User Authenticate(string email, string password)
        {
            var user = _dataService.Data.Users.SingleOrDefault(u => u.Email == email);
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