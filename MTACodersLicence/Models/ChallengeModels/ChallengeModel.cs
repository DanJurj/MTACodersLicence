using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using MTACodersLicence.Models.ChallengeModels;

namespace MTACodersLicence.Models
{
    public class ChallengeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }
        public string Desciption { get; set; }
        public string Tasks { get; set; }
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }  
        public string Hint { get; set; }
        public ICollection<BatteryModel> Batteries { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
