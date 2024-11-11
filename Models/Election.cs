using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class Election
    {
        [Key]
        public int ElectionID { get; set; }

        [Required]
        [Display(Name = "Election Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Active"; 

        public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
    }
}
