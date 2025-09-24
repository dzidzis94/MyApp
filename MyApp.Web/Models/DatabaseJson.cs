using System.Collections.Generic;

namespace MyApp.Web.Models
{
    public class DatabaseJson
    {
        public List<Admin> Admins { get; set; }
        public List<Client> Clients { get; set; }
        public List<Project> Projects { get; set; }
    }
}
