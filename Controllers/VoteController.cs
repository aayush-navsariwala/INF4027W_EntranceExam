using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.Models;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class VoteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CastVote()
        {
            var viewModel = new CastVoteViewModel
            {
                NationalElectionCandidates = await _context.Candidates
        .Where(c => c.ElectionId == 1) // National Election
        .ToListAsync(),
                ProvincialElectionCandidates = await _context.Candidates
        .Where(c => c.ElectionId == 2) // Provincial Election
        .ToListAsync()
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CastVote(CastVoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.NationalElectionCandidates = await _context.Candidates
                    .Where(c => c.ElectionId == 1)
                    .ToListAsync();
                model.ProvincialElectionCandidates = await _context.Candidates
                    .Where(c => c.ElectionId == 2)
                    .ToListAsync();
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieve current user ID

            if (userId == null)
            {
                return Unauthorized(); // Ensure the user is logged in
            }

            // Validate and save National Election vote
            if (model.SelectedNationalCandidateId.HasValue)
            {
                var existingNationalVote = await _context.Votes
                    .FirstOrDefaultAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == 1);

                if (existingNationalVote == null)
                {
                    var nationalVote = new Vote
                    {
                        UserId = Guid.Parse(userId),
                        ElectionId = 1,
                        CandidateId = model.SelectedNationalCandidateId.Value,
                        VoteTimestamp = DateTime.Now
                    };
                    _context.Votes.Add(nationalVote);
                    var candidate = await _context.Candidates.FindAsync(model.SelectedNationalCandidateId.Value);
                    candidate.VoteCount++;
                }
            }

            // Validate and save Provincial Election vote
            if (model.SelectedProvincialCandidateId.HasValue)
            {
                var existingProvincialVote = await _context.Votes
                    .FirstOrDefaultAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == 2);

                if (existingProvincialVote == null)
                {
                    var provincialVote = new Vote
                    {
                        UserId = Guid.Parse(userId),
                        ElectionId = 2,
                        CandidateId = model.SelectedProvincialCandidateId.Value,
                        VoteTimestamp = DateTime.Now
                    };
                    _context.Votes.Add(provincialVote);
                    var candidate = await _context.Candidates.FindAsync(model.SelectedProvincialCandidateId.Value);
                    candidate.VoteCount++;
                }
            }

            await _context.SaveChangesAsync(); // Save changes to the database

            return RedirectToAction("ConfirmVote");
        }

        public IActionResult ConfirmVote()
        {
            return View();
        }
    }
}
