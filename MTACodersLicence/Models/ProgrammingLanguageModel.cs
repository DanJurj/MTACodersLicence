using System.ComponentModel.DataAnnotations;

namespace MTACodersLicence.Models
{
    public class ProgrammingLanguageModel
    {
        public int Id { get; set; }
        [Display(Name = "Language Code")]
        public int LanguageCode { get; set; }
        public string Name { get; set; }
        [Display(Name = "Editor Mode")]
        public string EditorMode { get; set; }
        public bool Available { get; set; }
        [Display(Name = "Code Template")]
        public string CodeTemplate { get; set; }
    }
}
