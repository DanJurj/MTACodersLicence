using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models.GroupModels
{
    public class GroupMemberModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public GroupModel Group { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
