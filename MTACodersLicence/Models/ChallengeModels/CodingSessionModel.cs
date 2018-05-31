using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models.ChallengeModels
{
    public class CodingSessionModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }

        public int ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }

        public string Code { get; set; }
        public bool HasPreviousSave { get; set; }
    }
}
