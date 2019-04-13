using InzBackCore.Repositories;
using InzBackInfrastructure.InzBackDb;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using InzBackCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace InzBackInfrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private InzBackContext _context;
        private readonly IPlayerRepository _playerRepository;
        public MatchRepository(InzBackContext context, IPlayerRepository playerRepository)
        {
            _context = context;
            _playerRepository = playerRepository;
        }

        public async Task<Matchh> GetAsyncMatch(int id) //metoda pobiera mecz z bazy
        {
            var matches = _context.Matches.SingleOrDefault(x => x.Id == id);
            return await Task.FromResult(matches);
        }
        public async Task<IEnumerable<Matchh>> GetAsyncMatches(int userId)  // metoda pobiera mecze z bazy danego uzytkownika
        {
            var matches = _context.Matches.Where(x => x.UserId == userId).AsEnumerable();

            return await Task.FromResult(matches);
        }

        public async Task<int> AddAsyncMatch(Matchh match, int userId)  // metoda dodaje mecz do bazy
        {
            _context.Matches.Add(match);    
            _context.SaveChanges();
            await Task.CompletedTask;

            //zwraca matchId utworzonego meczu
            int matchId = match.Id;
            return await Task.FromResult(matchId);
        }


        public async Task UpdateAsyncMatch(Matchh match)    // metoda edytuje dane meczu w bazie
        {
            _context.Matches.Update(match);
            _context.SaveChanges();
            await Task.CompletedTask;

        }
        public async Task DeleteAsyncMatch(Matchh match)    // metoda usuwa mecz w bazie
        {
            _context.Matches.Remove(match);
            _context.SaveChanges();
            await Task.CompletedTask;
        }
       
        public async Task<Matchh> GetAsyncPlayersInMatch(int id)    // pobiera mecz ale z "dostepem" do tabeli MatchhPlayer, czyli z lista zawodnikow przypisana do niego
        {
            var playersListInMatch = _context.Matches.Include(x => x.Players2Match).ThenInclude(e => e.Player).SingleOrDefault(x => x.Id == id);//pobieram mecz a w nim liste z jakimi playerami sie łączy
            return await Task.FromResult(playersListInMatch);
        }
        
        public async Task AddAsyncPlayerToMatch(int matchId, MatchhPlayer player)
        {

            var match = await GetAsyncMatch(matchId);   //pobieram mecz o wskazanym matchId
            match.Players2Match.Add(player);
            _context.SaveChanges();
            await Task.CompletedTask;

        }

        public async Task<IEnumerable<Player>> GetAsyncListPlayersFullInMatch(int id)   // pobiera liste zawodnikow dopisana do meczu ale, nie tylko matchId a liste calych obiektow zawodnikow
        {
            //pobiera mecz z lista zawodnikow dopisanych do niego
            var playersInMatch = await GetAsyncPlayersInMatch(id);

            //przechodzi przez liste dopisanych zawodnikow do meczu, pobiera matchId nastepnie pobiera zawodnika i caly obiekt zawodnika dodaje do listy
            var playerList = new List<Player>(); 
            for (int i = 0; i < playersInMatch.Players2Match.Count; i++) 
            {
                int playerId = Convert.ToInt16(playersInMatch.Players2Match[i].PlayerId);
                var player = await _playerRepository.GetAsyncPlayer(playerId);
                playerList.Add(player);
            }
            return await Task.FromResult(playerList);
        }

        public async Task<IEnumerable<Player>> GetAsyncListPlayersOutMatch(int id, int userId)  // pobiera liste zawodnikow poza kadra meczowa 
        {

            //pobiera wszytskich zawodnikow uzytkownika i rzutuje na liste
            var allUserPlayers = await _playerRepository.GetAsyncAllPlayers(userId);
            var playersList = allUserPlayers.ToList();

            //pobiera liste zawodnikow dodanych do meczu
            var playersInMatch = await GetAsyncListPlayersFullInMatch(id);

            // usuwa z listy wszystkich zawodnikow ktorzy sa w kadrze
            foreach (Player player in playersInMatch)
            { 
                playersList.Remove(player); 
            }
            return await Task.FromResult(playersList);
        }
        public async Task DeleteAsyncListPlayersInMatch(int id, MatchhPlayer player)
        {
            var playersListInMatch = await GetAsyncPlayersInMatch(id);  //pobiera mecz z zawarta lista zawodnikow przypisana do niego
            playersListInMatch.Players2Match.Remove(player);   //jezeli jest to usuwamy i zapisujemy zmiany
            _context.SaveChanges();
            await Task.CompletedTask;
        }

        public async Task AddAsyncPlayerStatisticToMatch(int matchId, MatchhPlayer player)
        {   //tu dostajemy tez tablice intow czyli matchId playerow bo nie mozna bylo przeslac rzutujac na MatchPlayer 
            var match = await GetAsyncMatch(matchId); //pobieram mecz a w nim liste z jakimi playerami sie łączy
          
            match.Players2Match.Add(player);   //tworzymy na nowo obiekt o odpowiednich matchId i z wszystkimi informacjami i ponownie ale juz uzupelnionego dodajemy do bazy
            _context.SaveChanges(); 

            await Task.CompletedTask;
        }
       
        //public async Task<IEnumerable<MatchhPlayer>> GetAsyncListPlayersStatisticsInMatch(int id)
        //{
        //    var playersListInMatch = await GetAsyncPlayersInMatch(id);   //pobieram mecz a w nim liste z jakimi playerami sie łączy 
        //    var playerList = new List<MatchhPlayer>();
        //    for (int i = 0; i < playersListInMatch.Players2Match.Count; i++)
        //    {
        //        var player = new MatchhPlayer(Convert.ToInt16(playersListInMatch.Players2Match[i].Goals), Convert.ToInt16(playersListInMatch.Players2Match[i].Assists), Convert.ToInt16(playersListInMatch.Players2Match[i].YellowCard), Convert.ToInt16(playersListInMatch.Players2Match[i].RedCard), Convert.ToBoolean(playersListInMatch.Players2Match[i].PlayInMatch));   //wyszukuje go
        //        player.PlayerId = Convert.ToInt16(playersListInMatch.Players2Match[i].PlayerId); //wybieram zawodnika dopisanego do meczu
        //        playerList.Add(player); //i dodaje go do listy
        //    }
        //    return await Task.FromResult(playerList);
        //}

    }
}
