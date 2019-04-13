using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InzBackInfrastructure.Commands.Matches;
using InzBackInfrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InzBackApi.Controllers
{
    [Route("[controller]")]

    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;
        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{id}")]
        [Route("GetMatch/{id}")]
        public async Task<IActionResult> GetMatch(int id)  
        {
            var match = await _matchService.GetAsyncMatch(id);
            return Json(match);
        }

        [HttpGet("{userId}")]
        [Route("GetNearestMatch/{userId}")]
        public async Task<IActionResult> GetNearestMatch(int userId)   
        {
            var matches = await _matchService.GetNearestMatch(userId);
            return Json(matches);
        }

        [HttpGet("{userId}")]
        [Route("GetAllMatchesPast/{userId}")]
        public async Task<IActionResult> GetAllMatchesPast(int userId)   
        {
            var matches = await _matchService.GetAsyncMatchesPast(userId);
            return Json(matches);
        }

        [HttpGet("{userId}")]
        [Route("GetAllMatchcesFuture/{userId}")]
        public async Task<IActionResult> GetAllMatchcesFuture(int userId)   
        {
            var matches = await _matchService.GetAsyncMatchesFuture(userId);
            return Json(matches);
        }

        [HttpGet("{userId}")]
        [Route("GetTimeToMatch/{userId}")]
        public async Task<IActionResult> GetTimeToMatch(int userId)   
        {
            var time = await _matchService.GetAsyncTimeToMatchValue(userId);
            return Json(time);
        }

        [HttpPost("{userId}")]
        [Route("PostMatch/{userId}")]
        public async Task<IActionResult> PostMatch([FromBody]CreateMatch match, int userId)   
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            int matchId = await _matchService.CreateAsyncMatch(match, userId);
            return Created($"/Match/GetMatch/{matchId}", matchId);
        }

        [HttpPut("{matchId}")]
        [Route("PutMatch/{matchId}")]
        public async Task<IActionResult> PutMatch(int matchId, [FromBody]UpdateMatch match)    
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _matchService.UpdateAsyncMatch(matchId, match);
            return NoContent();
        }
        
        [HttpDelete("{id}")] 
        [Route("DeleteMatch/{id}")]
        public async Task<IActionResult> DeleteMatch(int id)    
        {
            await _matchService.DeleteAsyncMatch(id);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Route("AddPlayersToMatch/{id}")]  
        public async Task<IActionResult> PostPlayersToMatch(int id, [FromBody]AddPlayersIdToMatch players) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            
            await _matchService.AddPlayersToMatch(id, players.playerListId);

            
            return Created($"/Match/GetMatch/{id}", null);
        }

        [HttpGet("{id}")]
        [Route("GetPlayersInMatch/{id}")]
        public async Task<IActionResult> GetAllPlayersInMatch(int id)   
        {
            var players = await _matchService.GetPlayersInMatch(id);
            return Json(players);
        }

        [HttpGet("{userId}/{id}")]
        [Route("{userId}/GetPlayersOutMatch/{id}")]
        public async Task<IActionResult> GetPlayersOutMatch(int id, int userId)     
        {
            var players = await _matchService.GetPlayersOutMatch(id, userId);
            return Json(players);
        }

        [HttpDelete("{id}")]
        [Route("DeletePlayersInMatch/{id}")]
        public async Task<IActionResult> DeletePlayersInMatch(int id,[FromBody]DeletePlayersIdFromMatch players)    
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _matchService.DeletePlayersInMatch(id, players.playerListId);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Route("UpdateStatisticPlayersInMatch/{id}")]
        public async Task<IActionResult> UpdateStatisticPlayersInMatch(int id,[FromBody]UpdateStatisticPlayersInMatch playersStatistic) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _matchService.UpdateStatsInMatch(id, playersStatistic.playersStatisticInMatch);
            return NoContent(); 
        }

        [HttpGet("{id}")]
        [Route("GetPlayersStatisticInMatch/{id}")]
        public async Task<IActionResult> GetPlayersStatisticInMatch(int id)   
        {
            var players = await _matchService.GetListPlayersStatistics(id);
            return Json(players);
        }
    }
}