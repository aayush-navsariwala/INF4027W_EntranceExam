using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; } 
        public string Name { get; set; }
        public string Party { get; set; }
        public string PhotoUrl { get; set; } 
        public string Manifesto { get; set; } 
        public int ElectionId { get; set; } 
        public int VoteCount { get; set; } = 0;
        public Election Election { get; set; }
    }
}
