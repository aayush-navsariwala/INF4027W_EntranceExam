using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class Election
    {
        public int ElectionId { get; set; } 
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";

        // Navigation property for related candidates
        public ICollection<Candidate> Candidates { get; set; }
    }
}
