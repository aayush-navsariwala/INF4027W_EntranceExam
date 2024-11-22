using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class ProvincialResult
    {
        [Key]
        public int ProvincialResultID { get; set; }

        [Required]
        [ForeignKey("Candidate")]
        public int CandidateID { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Required]
        public string Province { get; set; } 

        public int TotalVotes { get; set; } 
    }
}