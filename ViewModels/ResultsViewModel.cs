using System.Collections.Generic;

namespace INF4001N_1814748_NVSAAY001_2024.ViewModels
{
    public class ResultsViewModel
    {
        public string ElectionTitle { get; set; }
        public List<string> CandidateNames { get; set; }
        public List<int> VoteCounts { get; set; }
        public List<string> CandidatePhotos { get; set; }
    }
}
