using System;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models.GroupModels
{
    public class GroupContestModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public GroupModel Group { get; set; }
        public int ContestId { get; set; }
        public ContestModel Contest { get; set; }
        public DateTime AssignDate { get; set; }
    }
}
