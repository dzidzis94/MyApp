using System.Collections.Generic;

namespace MyApp.Web.Models
{
    public class ApplicationData
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
