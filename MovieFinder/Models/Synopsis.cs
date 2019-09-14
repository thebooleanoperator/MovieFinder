namespace MovieFinder.Models
{
    public partial class Synopsis
    {
        public Synopsis()
        {

        }

        public int SynopsisId { get; set; }
        public string SynopsisSummary { get; set; }
        public int MovieId { get; set; }
    }
}
