using INF4001N_1814748_NVSAAY001_2024.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.ViewModels
{
    public class CastVoteViewModel
    {
        [Required]
        public int ElectionId { get; set; }

        [Required(ErrorMessage = "Please select a candidate.")]
        [Display(Name = "Select Candidate")]
        public int SelectedCandidateId { get; set; }

        public List<Candidate> Candidates { get; set; }
    }
}
