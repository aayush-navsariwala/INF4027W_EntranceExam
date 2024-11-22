using INF4001N_1814748_NVSAAY001_2024.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.ViewModels
{
    public class CastVoteViewModel
    {
        public List<Candidate> NationalElectionCandidates { get; set; } = new List<Candidate>();
        public List<Candidate> ProvincialElectionCandidates { get; set; } = new List<Candidate>();

        [Required(ErrorMessage = "Please select a candidate for the National Election.")]
        public int? SelectedNationalCandidateId { get; set; }

        [Required(ErrorMessage = "Please select a candidate for the Provincial Election.")]
        public int? SelectedProvincialCandidateId { get; set; }
    }
}