using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class Vote
    {
        [Key]
        public int VoteID { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Candidate")]
        public int CandidateID { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Required]
        [ForeignKey("Election")]
        public int ElectionID { get; set; }

        public virtual Election Election { get; set; }

        [Required]
        public DateTime VoteTimestamp { get; set; }
    }
}
