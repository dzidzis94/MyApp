using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class Client : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Other client-specific properties can be added here
    }
}