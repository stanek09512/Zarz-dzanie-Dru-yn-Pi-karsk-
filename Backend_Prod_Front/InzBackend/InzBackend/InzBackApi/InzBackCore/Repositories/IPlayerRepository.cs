using InzBackCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InzBackCore.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetAsyncPlayer(int id);    //asynchroniczna metoda pobierajaca zawodnika po id
        Task<IEnumerable<Player>> GetAsyncAllPlayers(int userId);  //metoda pobierająca zawodników którzy w nazwisku zawierają nasz string
        Task<int> AddAsyncPlayer(Player player, int userId); //metoda asynchronicznie dodajaca zawodnika 
        Task UpdateAsyncPlayer(Player player);
        Task DeleteAsyncPlayer(Player player);
        Task<IEnumerable<Player>> GetAsyncPlayersListStats(int userId);
        Task UpdateAsyncPlayerStatistic(PlayersStatictics player);
        Task<IEnumerable<PlayersStatictics>> GetAsyncPlayersStatistics(int userId);
        Task<PlayersStatictics> GetAsyncPlayerStatistic(int playerId);
    }
}
