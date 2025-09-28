using System.Collections.Generic;

namespace MyApp.Web.Models
{
    public class ProjectEditViewModel
    {
        public Project Project { get; set; }
        public List<User> AllClients { get; set; }
        public List<int> SelectedClientIds { get; set; }
    }
}