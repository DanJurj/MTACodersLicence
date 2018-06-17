using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.TestModels;

namespace MTACodersLicence.Models.BatteryModels
{
    public class BatteryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TestModel> Tests { get; set; }

        public int? ChallengeId { get; set; }
        [ForeignKey(nameof(ChallengeId))]
        public virtual ChallengeModel Challenge { get; set; }
    }
}
