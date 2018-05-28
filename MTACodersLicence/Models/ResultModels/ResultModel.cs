using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models.ResultModels
{
    public class ResultModel
    {
        public int Id { get; set; }

        public int SolutionId { get; set; }
        [ForeignKey(nameof(SolutionId))]
        public virtual SolutionModel Solution { get; set; }

        public int BatteryId { get; set; }
        [ForeignKey(nameof(BatteryId))]
        public virtual BatteryModel Battery { get; set; }

        public ICollection<TestResultModel> TestResults { get; set; }
    }
}
