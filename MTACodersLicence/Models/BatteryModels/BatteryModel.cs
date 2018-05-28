using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models.ChallengeModels
{
    public class BatteryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TestModel> Tests { get; set; }

        [Display(Name = "Public")]
        public bool IsPublic { get; set; }

        public int? ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }
    }
}
