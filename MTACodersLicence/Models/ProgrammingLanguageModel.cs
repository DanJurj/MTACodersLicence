using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTACodersLicence.Models
{
    public class ProgrammingLanguageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
        public bool Available { get; set; }
        public string CodeTemplate { get; set; }
    }
}
