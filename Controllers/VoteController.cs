using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.Models;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Matching;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class VoteController : Controller
    {
        //Constructor to inject the database context
        private readonly ApplicationDbContext _context;

        public VoteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CastVote()
        {
            //Retrieve the current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (userId == null)
            {
                //Return unauthorised if the user is not logged in
                return Unauthorized(); 
            }

            //Check if the user has already voted in both elections
            var hasVotedNational = await _context.Votes.AnyAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == 1);
            var hasVotedProvincial = await _context.Votes.AnyAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == 2);

            //Prepare the view model with candidates who haven't been voted for yet
            var viewModel = new CastVoteViewModel
            {
                //No candidates shown if already voted
                NationalElectionCandidates = hasVotedNational
                    ? new List<Candidate>() 
                    : await _context.Candidates.Where(c => c.ElectionId == 1).ToListAsync(),

                //No candidates shown if already voted
                ProvincialElectionCandidates = hasVotedProvincial
                    ? new List<Candidate>() 
                    : await _context.Candidates.Where(c => c.ElectionId == 2).ToListAsync()
            };

            //Redirect to the home page if the user has already voted in both elections
            if (hasVotedNational && hasVotedProvincial)
            {
                TempData["AlreadyVotedMessage"] = "You have already voted in both elections.";
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CastVote(CastVoteViewModel model)
        {
            //Validate the form model
            if (!ModelState.IsValid)
            {
                //Reload candidates if validation fails
                model.NationalElectionCandidates = await _context.Candidates
                    .Where(c => c.ElectionId == 1)
                    .ToListAsync();
                model.ProvincialElectionCandidates = await _context.Candidates
                    .Where(c => c.ElectionId == 2)
                    .ToListAsync();
                return View(model);
            }

            //Retrieve the current user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                //Handle unauthorized access
                TempData["ErrorMessage"] = "Unable to retrieve your user information. Please log in again.";
                return RedirectToAction("Login", "Account");
            }

            try
            {
                //Save National Election vote if applicable
                if (model.SelectedNationalCandidateId.HasValue)
                {
                    //Check if the user has already voted in the National Election
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

                        //Increment the vote count for the candidate
                        var candidate = await _context.Candidates.FindAsync(model.SelectedNationalCandidateId.Value);
                        if (candidate != null)
                        {
                            candidate.VoteCount++;
                        }
                    }
                }

                //Save Provincial Election vote if applicable
                if (model.SelectedProvincialCandidateId.HasValue)
                {
                    //Check if the user has already voted in the Provincial Election
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

                        //Increment the vote count for the candidate
                        var candidate = await _context.Candidates.FindAsync(model.SelectedProvincialCandidateId.Value);
                        if (candidate != null)
                        {
                            candidate.VoteCount++;
                        }
                    }
                }

                //Save changes to the database
                await _context.SaveChangesAsync();

                //Check if the user has voted in both elections
                var hasVotedNational = await _context.Votes.AnyAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == 1);
                var hasVotedProvincial = await _context.Votes.AnyAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == 2);

                if (hasVotedNational && hasVotedProvincial)
                {
                    //Redirect with success message
                    TempData["VoteSuccessMessage"] = "Your votes have been successfully saved. Thank you for participating!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //If one election is not voted, reload the page to continue voting
                    TempData["PartialVoteMessage"] = "You have successfully voted in one election. Please complete voting in the other election.";
                }
            }
            catch (Exception ex)
            {
                //Handle errors during vote saving
                Console.WriteLine($"Error saving vote: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while saving your vote. Please try again.";
            }

            //Reload candidates and return to the CastVote view
            model.NationalElectionCandidates = await _context.Candidates
                .Where(c => c.ElectionId == 1)
                .ToListAsync();
            model.ProvincialElectionCandidates = await _context.Candidates
                .Where(c => c.ElectionId == 2)
                .ToListAsync();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CastVoteAjax([FromBody] VoteRequestModel request)
        {
            if (request == null || request.CandidateId <= 0 || request.ElectionId <= 0)
            {
                return Json(new { success = false, message = "Invalid request data." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Json(new { success = false, message = "User is not authorized. Please log in again." });
            }

            try
            {
                var existingVote = await _context.Votes
                    .FirstOrDefaultAsync(v => v.UserId == Guid.Parse(userId) && v.ElectionId == request.ElectionId);

                if (existingVote != null)
                {
                    return Json(new { success = false, message = "You have already voted in this election." });
                }

                var vote = new Vote
                {
                    UserId = Guid.Parse(userId),
                    ElectionId = request.ElectionId,
                    CandidateId = request.CandidateId,
                    VoteTimestamp = DateTime.Now
                };

                _context.Votes.Add(vote);

                var candidate = await _context.Candidates.FindAsync(request.CandidateId);
                if (candidate != null)
                {
                    candidate.VoteCount++;
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Your vote has been successfully recorded." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during vote submission: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while processing your vote." });
            }
        }

        public IActionResult ConfirmVote()
        {
            return View();
        }
    }
}