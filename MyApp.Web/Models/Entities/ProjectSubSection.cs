using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class ProjectSubSection : BaseEntity
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<ProjectSubSectionSubmission> Submissions { get; set; } = new List<ProjectSubSectionSubmission>();
    }
}