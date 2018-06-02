using System.Collections.Generic;
using MTACodersLicence.Models;
using MTACodersLicence.Models.GroupModels;

namespace MTACodersLicence.ViewModels.GroupViewModels
{
    public class GroupMemberViewModel
    {
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<GroupMemberModel> Members { get; set; }
    }
}
