using InzBackCore.Domain;
using InzBackInfrastructure.Commands.Players;
using InzBackInfrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InzBackInfrastructure.Services
{
    public interface IPlayerService
    {
        Task<PlayerDto> GetAsyncPlayer(int id);
        Task<IEnumerable<PlayerDto>> GetAsyncAllPlayers(int userId);
        Task<int> CreateAsyncPlayer(PostPlayer playerData, int userId); //nie przekazalem inta bo przekazalem w modelu ze baza go generuje
        Task UpdateAsyncPlayer(PutPlayer playerData, int id);
        Task DeleteAsyncPlayer(int id);
        Task<IEnumerable<MergedPlayersStatisticsDto>> GetPlayersListStats(int userId);
    }
}
