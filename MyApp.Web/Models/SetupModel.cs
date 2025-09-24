using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models
{
    public class SetupModel
    {
        [Required(ErrorMessage = "Vārds ir obligāts")]
        [Display(Name = "Vārds")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Uzvārds ir obligāts")]
        [Display(Name = "Uzvārds")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-pasts ir obligāts")]
        [EmailAddress(ErrorMessage = "Nepareizs e-pasta formāts")]
        [Display(Name = "E-pasts")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Parole ir obligāta")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Parolei jābūt vismaz 6 rakstzīmēm")]
        [Display(Name = "Parole")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Paroles apstiprināšana ir obligāta")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Paroles nesakrīt")]
        [Display(Name = "Apstiprināt paroli")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}