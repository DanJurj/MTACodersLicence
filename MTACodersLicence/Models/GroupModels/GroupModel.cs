using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTACodersLicence.Models.GroupModels
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<GroupMemberModel> Members { get; set; }
        public ICollection<GroupContestModel> Contests { get; set; }
        public ICollection<JoinGroupRequestModel> JoinRequests { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }
    }
}
