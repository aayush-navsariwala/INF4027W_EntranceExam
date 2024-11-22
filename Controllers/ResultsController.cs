using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INF4001N_1814748_NVSAAY001_2024.Models;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using static System.Collections.Specialized.BitVector32;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class ResultsController : Controller
    {
        //Reference to the application's database context
        private readonly ApplicationDbContext _context;

        //Constructor to inject the database context
        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Retrieves and processes the election results for both national and provincial elections
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Total population assumed for percentage calculations
            const int totalPopulation = 100;

            //Fetch and process results for the national election
            var nationalElection = await _context.Elections
                .Include(e => e.Candidates)
                //Filter for the national election
                .Where(e => e.Title == "2024 National Elections")
                .Select(e => new ResultsViewModel
                {
                    //Title of the election
                    ElectionTitle = e.Title,
                    //List of candidate names
                    CandidateNames = e.Candidates.Select(c => c.Name).ToList(),
                    //List of vote counts for each candidate
                    VoteCounts = e.Candidates.Select(c => c.VoteCount).ToList(),
                    //URLs of candidate photos
                    CandidatePhotos = e.Candidates.Select(c => c.PhotoUrl).ToList(),
                    //Total number of votes cast
                    TotalVotes = e.Candidates.Sum(c => c.VoteCount),
                    //Percentage of the population that voted
                    PopulationPercentageVoted = e.Candidates.Sum(c => c.VoteCount) / (double)totalPopulation * 100,
                    //Percentage of votes per candidate
                    CandidateVotePercentages = e.Candidates.Select(c => c.VoteCount / (double)e.Candidates.Sum(x => x.VoteCount) * 100).ToList() 
                })
                //Get the first matching result asynchronously
                .FirstOrDefaultAsync();

            //Fetch and process results for the provincial election
            var provincialElection = await _context.Elections
                //Include associated candidates
                .Include(e => e.Candidates)
                //Filter for the provincial election
                .Where(e => e.Title == "2024 Provincial Elections")
            .Select(e => new ResultsViewModel
                {
                    //Title of the election
                    ElectionTitle = e.Title,
                    //List of candidate names
                    CandidateNames = e.Candidates.Select(c => c.Name).ToList(),
                    //List of vote counts for each candidate
                    VoteCounts = e.Candidates.Select(c => c.VoteCount).ToList(),
                    //URLs of candidate photos
                    CandidatePhotos = e.Candidates.Select(c => c.PhotoUrl).ToList(),
                    //Total number of votes cast
                    TotalVotes = e.Candidates.Sum(c => c.VoteCount),
                    //Percentage of the population that voted
                    PopulationPercentageVoted = e.Candidates.Sum(c => c.VoteCount) / (double)totalPopulation * 100,
                    //Percentage of votes per candidate
                    CandidateVotePercentages = e.Candidates.Select(c => c.VoteCount / (double)e.Candidates.Sum(x => x.VoteCount) * 100).ToList()
                })
                //Get the first matching result asynchronously
                .FirstOrDefaultAsync();

            //Combine national and provincial results into a list for display
            var results = new List<ResultsViewModel> { nationalElection, provincialElection };

            //Return the results view with the processed data
            return View(results);
        }
    }
}