using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "E-pasts ir obligāts")]
        [EmailAddress(ErrorMessage = "Nepareizs e-pasta formāts")]
        [Display(Name = "E-pasts")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parole ir obligāta")]
        [DataType(DataType.Password)]
        [Display(Name = "Parole")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Atcerēties mani")]
        public bool RememberMe { get; set; }
    }
}