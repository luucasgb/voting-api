using Microsoft.AspNetCore.Mvc;

namespace voting_api.Controllers
{
    public class VotesController
    {

        [HttpGet]
        [Route("api/votes")]

        public IActionResult GetVotes()
        {
            // Logic to get votes
            return Ok(new { message = "Votes retrieved successfully" });
        }

        [HttpPost]
        [Route("api/votes")]
        public IActionResult CreateVote([FromBody] Vote vote)
        {
            // Logic to create a vote
            return CreatedAtAction(nameof(GetVotes), new { id = vote.Id }, vote);
        }
    }
}
