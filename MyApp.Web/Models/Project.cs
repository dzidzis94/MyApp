using System.Collections.Generic;

namespace MyApp.Web.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<SubSection> SubSections { get; set; } = new List<SubSection>();
        public List<int> AssignedUserIds { get; set; } = new List<int>();
    }
}
