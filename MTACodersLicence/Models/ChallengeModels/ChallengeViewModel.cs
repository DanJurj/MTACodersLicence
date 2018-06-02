namespace MTACodersLicence.Models.ChallengeModels
{
    public class ChallengeViewModel
    {
        public ChallengeModel Challenge { get; set; }
        public CodingSessionModel CodingSession { get; set; }
        public bool HasPreviousSave { get; set; }
        public int RemainingTime { get; set; } //minutes
        public bool HasRemainingTime { get; set; }
        public string Stdout { get; set; }
        public string Stderr { get; set; }
        public string Error { get; set; }
        public bool HasError { get; set; }
    }
}
