using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InzBackCore.Domain;
using InzBackInfrastructure.DTO;
using InzBackInfrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using InzBackInfrastructure.Commands.Players;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace InzBackApi.Controllers
{
    [EnableCors("AllowClientOrigin")]
    [Route("[controller]")]

    public class PlayersController : Controller
    {
 
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        
        [HttpGet("{id}")]
        [Route("GetPlayer/{id}")]
        public async Task<IActionResult> GetPlayer(int id)  // metoda pobierająca zawodnika
        {
            var player = await _playerService.GetAsyncPlayer(id);   //wywolujemy metode z z serwisu asynchronicznie pobierającą zawodnika
            return Json(player);    //przesyła pobranego zawodnika jako json
        }

  
        [HttpGet("{userId}")]
        [Route("GetAllPlayers/{userId}")]
        public async Task<IActionResult> GetAllPlayers(int userId)  //metoda pobierająca listę wszystkich zawodnikow dla odpowiedniego użytkownika
        {
            var players = await _playerService.GetAsyncAllPlayers(userId);   //wywolujemy metode z serwisu pobierającą asynchronicznie wszystkich zawodników
            return Json(players);    //wyrzucany Json jak wynik
        }

        //[EnableCors("AllowClientOrigin")]
        [HttpPost("{userId}")]        
        [Route("PostPlayer/{userId}")]
        public async Task<IActionResult> PostPlayer([FromBody]PostPlayer playerData, int userId)  //metoda tworząca nowego zawodnika dla odpowiedniego konta
        {
            // sprawdza czy walidacja pobieranej klasy jako "FromBody" powiodła się
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // wywołanie metody tworzącej zawodnika ze zwrotką uzyskanego przez niego id
            int playerId = await _playerService.CreateAsyncPlayer(playerData, userId);  

            // pobranie nowo utworzonego zawodnika
            var player = await _playerService.GetAsyncPlayer(playerId);

            //zwrotka statusu kodu "201" wraz z adresem do utworzonego zawodnika i jego obiektem w postaci json
            return Created($"https://localhost:44356/"+"Players/GetPlayer/{playerId}/",Json(player)); 
        }



        [HttpPut("{playerId}")] 
        [Route("PutPlayer/{playerId}")]
        public async Task<IActionResult> PutPlayer(int playerId, [FromBody]PutPlayer playerData)    // edytuje wybranego zawodnika
        {
            // sprawdza czy walidacja pobieranej klasy jako "FromBody" powiodła się
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //wywołuje metodę edytującą z serwisu
            await _playerService.UpdateAsyncPlayer(playerData, playerId);

            return NoContent(); // zwraca status kodu "204"  
        }


        [HttpDelete("{playerId}")] //tak bedzie bo bedziemy przekazywac id przez adres url
        [Route("DeletePlayer/{playerId}")]
        public async Task<IActionResult> DeletePlayer(int playerId)
        {
            //wywołuje metodę usuwającą z serwisu
            await _playerService.DeleteAsyncPlayer(playerId);

            return NoContent(); // zwraca status kodu "204"  
        }

        [HttpGet("{userId}")]
        [Route("GetAllPlayersStatistics/{userId}")]    // metoda pobierająca statystyki wszystkich zawodnikow konkretnego uzytkownika
        public async Task<IActionResult> GetAllPlayersStatistics(int userId)
        {
            //wywołuje metodę pobierajaca statystyki wszystkich zawodnikow z serwisu
            var players = await _playerService.GetPlayersListStats(userId);

            // zwraca wynik zapytania w postaci json
            return Json(players);
        }
}
}