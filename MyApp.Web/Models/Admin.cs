using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Web.Models
{
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vārds ir obligāts")]
        [StringLength(50, ErrorMessage = "Vārdam jābūt līdz 50 rakstzīmēm")]
        public string? Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Uzvārds ir obligāts")]
        [StringLength(50, ErrorMessage = "Uzvārdam jābūt līdz 50 rakstzīmēm")]
        public string? LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-pasts ir obligāts")]
        [EmailAddress(ErrorMessage = "Nepareizs e-pasta formāts")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Nepareizs telefona numura formāts")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Parole ir obligāta")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Parolei jābūt vismaz 6 rakstzīmēm")]
        public string? Password { get; set; }

        public string UserType => "Admin";
        public bool IsActive { get; set; } = true;

        // ✅ PIEVIENO ŠO TUKŠO KONSTRUKTORU
        public Admin()
        {
            // Tukšs konstruktors Entity Framework darbībai
        }

        // Jūsu esošais konstruktors ar parametriem
       // public Admin(int id, string? name, string? lastName, string? email, string? phoneNumber, string? password, bool isActive)
       // {
          //  Id = id;
         //   Name = name;
         //   LastName = lastName;
        //    Email = email;
         //   PhoneNumber = phoneNumber;
        //    Password = password;
         //   IsActive = isActive;
       // }
    }
}