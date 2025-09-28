using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class RecentActivity : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public string Action { get; set; } = string.Empty;

        public int? RelatedId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}