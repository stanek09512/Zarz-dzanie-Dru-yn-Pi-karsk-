using InzBackCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace InzBackCore.Repositories
{
    public interface IMatchRepository
    {
        Task<Matchh> GetAsyncMatch(int id);    //asynchroniczna metoda pobierajaca zawodnika po id
        Task<IEnumerable<Matchh>> GetAsyncMatches(int userId);  //metoda pobierająca zawodników którzy w nazwisku zawierają nasz string
        Task<int> AddAsyncMatch(Matchh match, int userId); //metoda asynchronicznie dodajaca zawodnika 
        Task UpdateAsyncMatch(Matchh match);
        Task DeleteAsyncMatch(Matchh match);
        Task<Matchh> GetAsyncPlayersInMatch(int id);

        Task AddAsyncPlayerToMatch(int matchId, MatchhPlayer player);
        Task DeleteAsyncListPlayersInMatch(int matchId, MatchhPlayer players);
        Task AddAsyncPlayerStatisticToMatch(int matchId, MatchhPlayer player);
        //Task<IEnumerable<MatchhPlayer>> GetAsyncListPlayersStatisticsInMatch(int id);


    }
}
