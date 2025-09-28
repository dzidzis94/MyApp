using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }
}