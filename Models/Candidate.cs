using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateID { get; set; }

        [Required]
        [Display(Name = "Candidate Name")]
        public string Name { get; set; }

        public string Party { get; set; }

        [Required]
        [ForeignKey("Election")]
        public int ElectionID { get; set; }

        public virtual Election Election { get; set; }

        public int VoteCount { get; set; } = 0; 
    }
}
