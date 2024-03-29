﻿using System.ComponentModel.DataAnnotations.Schema;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models.SolutionModels
{
    public class SolutionViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool Verified { get; set; }
        public int Score { get; set; }

        public int ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }
    }
}
