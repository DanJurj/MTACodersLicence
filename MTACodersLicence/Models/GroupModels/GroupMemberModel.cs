using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Name")]
        public ApplicationUser User { get; set; }
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }
    }
}
