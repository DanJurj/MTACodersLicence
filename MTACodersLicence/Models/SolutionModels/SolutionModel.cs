using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models
{
    public class SolutionModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Verified { get; set; }
        public int Score { get; set; }
        public string Language { get; set; }
        [Display(Name = "Received Time")]
        public DateTime ReceiveDateTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime Duration { get; set; }

        public int ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }
    }
}
