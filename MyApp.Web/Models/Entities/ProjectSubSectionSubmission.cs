using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class ProjectSubSectionSubmission : BaseEntity
    {
        public int SubSectionId { get; set; }
        public ProjectSubSection ProjectSubSection { get; set; } = null!;

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}