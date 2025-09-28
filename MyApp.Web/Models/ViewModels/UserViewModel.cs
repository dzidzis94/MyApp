using System;

namespace MyApp.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string UserType { get; set; } = "Client";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int ProjectCount { get; set; } = 0;

        public string FullName => $"{FirstName} {LastName}";
        public string Initials
        {
            get
            {
                var first = string.IsNullOrEmpty(FirstName) ? "" : FirstName[0].ToString();
                var last = string.IsNullOrEmpty(LastName) ? "" : LastName[0].ToString();
                return first + last;
            }
        }
    }
}