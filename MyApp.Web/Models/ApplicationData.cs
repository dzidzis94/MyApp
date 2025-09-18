using System.Collections.Generic;

namespace MyApp.Web.Models
{
    public class ApplicationData
    {
        public List<Admin> Admins { get; set; } = new List<Admin>();
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
