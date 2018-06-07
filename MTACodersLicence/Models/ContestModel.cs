using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models
{
    public class ContestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Time { get; set; }  //minutes
        public bool Active { get; set; }

        public ICollection<ChallengeModel> Challenges { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser Owner { get; set; }
    }
}
