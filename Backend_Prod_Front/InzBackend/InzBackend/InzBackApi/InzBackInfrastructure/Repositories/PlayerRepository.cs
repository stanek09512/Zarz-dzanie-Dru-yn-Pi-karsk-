using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InzBackCore.Domain;
using InzBackCore.Repositories;
using InzBackInfrastructure.InzBackDb;
using InzBackInfrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace InzBackInfrastructure.Repositories
{
    public class PlayerRepository:IPlayerRepository
    {

        private InzBackContext _context;


        public PlayerRepository(InzBackContext context)
        {
            _context = context;

        }

        public async Task<Player> GetAsyncPlayer(int id)    // metoda pobiera z bazy zawodnika o odpowiednim id
        {
            var player = _context.Players.SingleOrDefault(x => x.Id == id);
            return await Task.FromResult(player);
        }

        public async Task<IEnumerable<Player>> GetAsyncAllPlayers(int userId)   //metoda pobiera wszystkich zawodnikow dla uzytkownika
        { 
            var players = _context.Players.Where(x => x.UserId == userId).AsEnumerable();
            return await Task.FromResult(players);       
        }
        public async Task<int> AddAsyncPlayer(Player player, int userId)
        {
           
            _context.Players.Add(player);
            _context.SaveChanges();
            await Task.CompletedTask;   //to musi byc w asynchronicznych bo by zwracal warningi.
            int playerId= player.Id;
            return playerId;
           
        }

        public async Task UpdateAsyncPlayer(Player player)  //metoda edytuje zawdnika w bazie
        {
            _context.Players.Update(player);
            _context.SaveChanges();
            await Task.CompletedTask;
        }
        public async Task DeleteAsyncPlayer(Player player)  //metoda usuwa zawodnika z bazy
        {
            _context.Players.Remove(player);
            _context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Player>> GetAsyncPlayersListStats(int userId) // metoda pobiera liste zawodnikow ale z meczami w ktorych brali udział, czyli wchodzi w tabele MatchhPlayer
        {
            var players = _context.Players.Where(x => x.UserId == userId).Include(x => x.Matchhs2Player).ThenInclude(e => e.Matchh).AsEnumerable();
            return await Task.FromResult(players);
              
        }
        public async Task<PlayersStatictics> GetAsyncPlayerStatistic(int playerId)  //metoda pobiera statystyki zawodnika z tabeli statystyk ogolnych
        {
            var player  = _context.PlrStats.FirstOrDefault(x => x.PlayerId == playerId);
            if (player == null) return null;//
            return await Task.FromResult(player);  
        }
        public async Task UpdateAsyncPlayerStatistic(PlayersStatictics playerS) //metoda edytuje statystyki ogolne w tabeli PlrStats
        {
            _context.PlrStats.Update(playerS);
            _context.SaveChanges();
            await Task.CompletedTask;   
        }
        public async Task<IEnumerable<PlayersStatictics>> GetAsyncPlayersStatistics(int userId) //metoda pobiera statystyki ogolne dla wszystkich zawodnikow dla odpowiedniego uzytkownika
        {
            var playerStatistics = _context.PlrStats.Where(x => x.player.UserId == userId);
            return await Task.FromResult(playerStatistics);
        }




    }
}
