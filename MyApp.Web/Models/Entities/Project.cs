using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CreatedById { get; set; }
        public Admin CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ProjectSubSection> SubSections { get; set; } = new List<ProjectSubSection>();
    }
}