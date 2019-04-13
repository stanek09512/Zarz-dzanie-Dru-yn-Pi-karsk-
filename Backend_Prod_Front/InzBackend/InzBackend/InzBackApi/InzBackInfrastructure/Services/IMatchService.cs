using InzBackCore.Domain;
using InzBackInfrastructure.Commands.Matches;
using InzBackInfrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InzBackInfrastructure.Services
{
    public interface IMatchService
    {
        Task<Matchh> GetAsyncMatch(int id);
        Task<IEnumerable<Matchh>> GetAsyncMatchesPast(int userId);
        Task<IEnumerable<Matchh>> GetAsyncMatchesFuture(int userId);
        Task<int> CreateAsyncMatch(CreateMatch match, int userId);
        Task UpdateAsyncMatch(int id, UpdateMatch match);
        Task DeleteAsyncMatch(int id);
        Task AddPlayersToMatch(int MatchId, IEnumerable<int> PlayersId);
        Task<IEnumerable<PlayerDto>> GetPlayersInMatch(int id);
        Task<IEnumerable<PlayerDto>> GetPlayersOutMatch(int id, int userId);
        Task DeletePlayersInMatch(int MatchId, IEnumerable<int> PlayersId);

        Task UpdateStatsInMatch(int Matchid, IEnumerable<GetPlayersStatisticInMatchDto> PlayersStats);
        Task<IEnumerable<MergedPlayersStatisticsDto>> GetListPlayersStatistics(int id);

        Task<IEnumerable<Matchh>> GetNearestMatch(int userId);
        Task<List<double>> GetAsyncTimeToMatchValue(int userId);
    }
}
