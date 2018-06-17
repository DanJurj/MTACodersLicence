using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models
{
    public class ProfessorKey
    {
        public int Id { get; set; }
        public string Key { get; set; }
        [Display(Name = "Number of Accounts available")]
        public int NumberOfAccountsAvailable { get; set; }
    }
}
