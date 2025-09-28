using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models
{
    public class AdminViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Vārds")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Uzvārds")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "E-pasts")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefona numurs")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Pilnais vārds")]
        public string FullName => $"{Name} {LastName}";
    }
}
