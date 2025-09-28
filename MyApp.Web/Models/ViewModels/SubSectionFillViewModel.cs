using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models.ViewModels
{
    public class SubSectionFillViewModel
    {
        public int SubSectionId { get; set; }
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}