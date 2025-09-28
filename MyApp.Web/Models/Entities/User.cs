using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty; // "Admin" or "Client"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}