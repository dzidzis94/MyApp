using System.Collections.Generic;

namespace MyApp.Web.Models
{
    public class SubSection
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Data { get; set; } // For client to fill in
        public int? ParentId { get; set; }
        public List<SubSection> SubSections { get; set; } = new List<SubSection>();
    }
}
