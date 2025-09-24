using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models
{
    public class UserCreateModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vārds ir obligāts")]
        [Display(Name = "Vārds")]
        [StringLength(50, ErrorMessage = "Vārdam jābūt līdz 50 rakstzīmēm")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Uzvārds ir obligāts")]
        [Display(Name = "Uzvārds")]
        [StringLength(50, ErrorMessage = "Uzvārdam jābūt līdz 50 rakstzīmēm")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "E-pasts ir obligāts")]
        [EmailAddress(ErrorMessage = "Nepareizs e-pasta formāts")]
        [Display(Name = "E-pasts")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Nepareizs telefona numura formāts")]
        [Display(Name = "Telefona numurs")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Lietotāja tips ir obligāts")]
        [Display(Name = "Lietotāja tips")]
        public string UserType { get; set; } = "Client";

        [Display(Name = "Parole")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Parolei jābūt vismaz 6 rakstzīmēm")]
        public string? Password { get; set; }

        [Display(Name = "Apstiprināt paroli")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Paroles nesakrīt")]
        public string? ConfirmPassword { get; set; }
    }
}