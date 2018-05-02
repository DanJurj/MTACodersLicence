using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace MTACodersLicence.Models
{
    public class ChallengeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Desciption { get; set; }
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }  
        public string Hint { get; set; }
        public ICollection<TestModel> Tests { get; set; }

        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
