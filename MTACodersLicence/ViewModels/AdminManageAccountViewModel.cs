using System.Collections.Generic;
using MTACodersLicence.Models;

namespace MTACodersLicence.ViewModels
{
    public class AdminManageAccountViewModel
    {
        public ApplicationUser Account { get; set; }
        public IList<string> Roles { get; set; }
    }
}
