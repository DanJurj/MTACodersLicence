using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models.GroupModels
{
    public class GroupChallengeModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public GroupModel Group { get; set; }
        public int ChallengeId { get; set; }
        public ChallengeModel Challenge { get; set; }
        public DateTime AssignDate { get; set; }
    }
}
