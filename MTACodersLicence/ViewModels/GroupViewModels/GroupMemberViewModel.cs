using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models.GroupModels
{
    public class GroupMemberViewModel
    {
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<GroupMemberModel> Members { get; set; }
    }
}
