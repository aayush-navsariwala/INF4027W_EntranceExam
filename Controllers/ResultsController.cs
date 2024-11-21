using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var nationalElection = await _context.Elections
                .Include(e => e.Candidates)
                .Where(e => e.Title == "2024 National Elections")
                .Select(e => new ResultsViewModel
                {
                    ElectionTitle = e.Title,
                    CandidateNames = e.Candidates.Select(c => c.Name).ToList(),
                    VoteCounts = e.Candidates.Select(c => c.VoteCount).ToList(),
                    CandidatePhotos = e.Candidates.Select(c => c.PhotoUrl).ToList()
                })
                .FirstOrDefaultAsync();

            var provincialElection = await _context.Elections
                .Include(e => e.Candidates)
                .Where(e => e.Title == "2024 Provincial Elections")
                .Select(e => new ResultsViewModel
                {
                    ElectionTitle = e.Title,
                    CandidateNames = e.Candidates.Select(c => c.Name).ToList(),
                    VoteCounts = e.Candidates.Select(c => c.VoteCount).ToList(),
                    CandidatePhotos = e.Candidates.Select(c => c.PhotoUrl).ToList()
                })
                .FirstOrDefaultAsync();

            var results = new List<ResultsViewModel> { nationalElection, provincialElection };

            return View(results);
        }
    }
}
