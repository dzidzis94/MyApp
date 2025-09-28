using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class Admin : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Other admin-specific properties can be added here
    }
}