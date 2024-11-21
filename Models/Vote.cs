using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class Vote
    {
        public int VoteId { get; set; } 
        public Guid UserId { get; set; } 
        public int ElectionId { get; set; } 
        public int CandidateId { get; set; } 
        public DateTime VoteTimestamp { get; set; }
    }
}
