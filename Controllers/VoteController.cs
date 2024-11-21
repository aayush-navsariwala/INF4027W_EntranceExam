using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.Models;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public VoteController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CastVote(int electionId)
        {
            var viewModel = new CastVoteViewModel
            {
                ElectionId = electionId,
                Candidates = await _context.Candidates
                               .Where(c => c.ElectionId == electionId) 
                               .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CastVote(CastVoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Candidates = await _context.Candidates
                                        .Where(c => c.ElectionId == model.ElectionId) 
                                        .ToListAsync();
                return View(model);
            }

            var userId = _userManager.GetUserId(User); // Get the logged-in user's ID

            // Check if the user has already voted in this election
            var existingVote = await _context.Votes
                                  .FirstOrDefaultAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == model.ElectionId); 

            if (existingVote != null)
            {
                ModelState.AddModelError(string.Empty, "You have already voted in this election.");
                model.Candidates = await _context.Candidates
                                        .Where(c => c.ElectionId == model.ElectionId) 
                                        .ToListAsync();
                return View(model);
            }

            // Cast the vote
            var vote = new Vote
            {
                UserId = Guid.Parse(userId), 
                ElectionId = model.ElectionId, 
                CandidateId = model.SelectedCandidateId, 
                VoteTimestamp = DateTime.Now
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return RedirectToAction("ConfirmVote");
        }

        public IActionResult ConfirmVote()
        {
            return View();
        }
    }
}
