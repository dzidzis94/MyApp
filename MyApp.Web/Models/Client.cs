using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Web.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vārds ir obligāts")]
        [StringLength(50, ErrorMessage = "Vārdam jābūt līdz 50 rakstzīmēm")]
        [Display(Name = "Vārds")]
        public string? FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Uzvārds ir obligāts")]
        [StringLength(50, ErrorMessage = "Uzvārdam jābūt līdz 50 rakstzīmēm")]
        [Display(Name = "Uzvārds")]
        public string? LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-pasts ir obligāts")]
        [EmailAddress(ErrorMessage = "Nepareizs e-pasta formāts")]
        [Display(Name = "E-pasts")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Nepareizs telefona numura formāts")]
        [Display(Name = "Telefona numurs")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Uzņēmuma nosaukums")]
        public string? CompanyName { get; set; }

        [Display(Name = "Adrese")]
        public string? Address { get; set; }

        [Display(Name = "Pilsēta")]
        public string? City { get; set; }

        [Display(Name = "Valsts")]
        public string? Country { get; set; } = "Latvija";

        [Display(Name = "Pasta indekss")]
        public string? PostalCode { get; set; }

        [Display(Name = "Aktīvs")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Pievienošanas datums")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserType => "Client"; // Jauna īpašība
      
    }
}