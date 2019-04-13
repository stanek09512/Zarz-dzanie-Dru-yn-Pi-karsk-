using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InzBackCore.Domain;
using InzBackCore.Repositories;
using InzBackInfrastructure.Commands.Players;
using InzBackInfrastructure.DTO;
using InzBackInfrastructure.InzBackDb;

namespace InzBackInfrastructure.Services
{
    public class PlayerService : IPlayerService
    {
        //Zastosowanie "wstrzykiwania" (ang. dependency injection)
        private readonly IPlayerRepository _playerRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;

        public PlayerService(IPlayerRepository playerRepository, IMapper mapper, IAuthRepository authRepository)    
        {
            //wstrzykujemy repozytorium interfejsu zawodnikow który bedzie "przekierowywał" nas do metod w klasie implementacyjnej interfejs
            _playerRepository = playerRepository;

            _authRepository = authRepository;

            //wstrzykujemy mapper ktory mapuje obiekty
            _mapper = mapper;

        }
        public async Task<PlayerDto> GetAsyncPlayer(int id)
        {
            //pobiera zawodnika
            var player = await _playerRepository.GetAsyncPlayer(id); 
            
            //jeżeli nie istnieje to zwraca odpowieni błąd
            if (player == null)
            {
                throw new Exception($"Player with this id: {id} not exist");
            }
            
            //mapuje zawodnika i zwraca zmapowanego
            return _mapper.Map<PlayerDto>(player); 
        }
        public async Task<IEnumerable<PlayerDto>> GetAsyncAllPlayers(int userId)
        {
            //pobiera wszystkich zawodnikow, nie uwzględniając użytkownika
            var players = await _playerRepository.GetAsyncAllPlayers(userId);

            //sortuje pobranych zawodnikow po pozycji (tzn: "Bramkarz","Obrońca",... itd)
            players = SortPlayers(players.ToList());  

            return _mapper.Map<IEnumerable<PlayerDto>>(players);
        }

        public async Task<int> CreateAsyncPlayer(PostPlayer playerData, int userId)
        {
            
            //pobieramy uzytkownika z bazy aby utworzyc pelny obiekt player do dodania do bazy
            var user = await _authRepository.UserAccount(userId);
            // tworzy objekt Player
            var player = new Player();
            player.user = user;
            player.SetSurname(playerData.Surname);
            player.SetName(playerData.Name);
            player.SetAge(playerData.Age);
            player.SetPosition(playerData.Position);
            var PlayersStatictics = new PlayersStatictics(0, 0, 0, 0, 0, player);
            player.Statictics = PlayersStatictics;

            // przekazuje id użytkownika, który chce stworzyć zawodnika i samego zawodnika do metody z repozytorium
            // a wywołanie jest przypisane do zmiennej ponieważ metoda z repozytorium zwraca id nowo utworzonego zawodnika
            int createdPlayerId = await _playerRepository.AddAsyncPlayer(player, userId);

            return createdPlayerId;
        }


        public async Task UpdateAsyncPlayer(PutPlayer playerData, int id)
        {
            //pobiera zawodnika is prawdza czy taki zawodnik istnieje 
            var player = await _playerRepository.GetAsyncPlayer(id);  

            //jezeli nie istnieje to wysyła błąd to takiej treści
            if (player == null) throw new Exception($"Player with id:" + id + "not exist");

            //zamienia pola w obiekcie tak jak chcemy
            player.SetSurname(playerData.Surname);
            player.SetName(playerData.Name);
            player.SetAge(playerData.Age);
            player.SetPosition(playerData.Position);

            //wywoluje metode do edycji zawodnika z repozytorium i przesyla do niej juz edytowanego zawodnika
            await _playerRepository.UpdateAsyncPlayer(player);
        }

        public async Task DeleteAsyncPlayer(int id)
        {
            //pobiera zawodnika o tym id 
            var player = await _playerRepository.GetAsyncPlayer(id);

            //jezeli nie istnieje to zwraca błąd
            if (player == null) throw new Exception($"Player with id:{id} not exist");

            await _playerRepository.DeleteAsyncPlayer(player);
        }
        public async Task<IEnumerable<MergedPlayersStatisticsDto>> GetPlayersListStats(int userId)   //metoda pobierajaca liste zawodnikow z ich statystykami
        {
            //pobiera liste statystyk zawodnikow w meczach (to zawodnik z polaczeniem tabeli ZawodnikMecz)
            var playersStatisticListInMatches = await _playerRepository.GetAsyncPlayersListStats(userId);  

            //na nowo sa liczone statystyki kazdego zawodnika
            foreach (var plr in playersStatisticListInMatches)   //leci po zawodnikach
            {   
                // zerujemy ilosc w kazdej liczonej zmiennej statystyk
                int Matches = 0;
                int Goals = 0;
                int Assists = 0;
                int YellowCard = 0;
                int RedCard = 0;

                //pobiera statystyke zawodnika z kazdego meczu w ktorym znalazl sie w kadrze i dodaje do juz pobranych
                foreach (var match in plr.Matchhs2Player)   
                {
                    if (match.Goals != null)  Goals = Convert.ToInt16(Goals + match.Goals);
                    if (match.Assists != null)  Assists = Convert.ToInt16(Assists + match.Assists);
                    if (match.YellowCard != null)  YellowCard = Convert.ToInt16(YellowCard + match.YellowCard);
                    if (match.RedCard != null)  RedCard = Convert.ToInt16(RedCard + match.RedCard);
                    if (match.PlayInMatch == true) Matches++;
                }

                //pobiera statystyke zawodnika
                PlayersStatictics playerToUpdate = await _playerRepository.GetAsyncPlayerStatistic(plr.Id);

                //edytuje wartosci ktore powyzej zostaly na nowo wyliczone
                playerToUpdate.SetMatches(Matches);
                playerToUpdate.SetGoals(Goals);
                playerToUpdate.SetAssists(Assists);
                playerToUpdate.SetYellowCards(YellowCard);
                playerToUpdate.SetRedCard(RedCard);
                
                //wywołuje metode edytujaca w bazie statystyke zawodnika
                await _playerRepository.UpdateAsyncPlayerStatistic(playerToUpdate);  
            }


            //pobiera juz edytowana, nowa liste statystyk(ta ogolna)
            var playerStatsIEnumerable = await _playerRepository.GetAsyncPlayersStatistics(userId);  
            
            //zamienia IEnumerable uzyskany z powyższej metody na liste
            List<PlayersStatictics> playersStatisticsList = playerStatsIEnumerable.ToList();

            //pobiera podstawową listę zawodnikow
            var players = await _playerRepository.GetAsyncAllPlayers(userId);  
            List<Player> playersList = players.ToList();   

            //sortuje listę po pozycjach
            playersList =  SortPlayers(playersList);


            //deklaracja listy obiektow scalonych z tabeli Player i Statistics
            var mergedPlayersStatisticsList = new List<MergedPlayersStatisticsDto>();

            //uzupenianie scalonej listy
            for (int i = 0; i < playersList.Count(); i++)  
            {
                //pobiera id zawodnika
                int playerId = playersList[i].Id;

                //pobiera jego statystyki po jego id
                PlayersStatictics statistic = playersStatisticsList.FirstOrDefault(x => x.PlayerId == playerId);

                //tworzy nowy obiekt w oparciu o zawodnika i statystyki
                var mergedPlayer = new MergedPlayersStatisticsDto(playerId, playersList[i].Name, playersList[i].Surname, playersList[i].Position,
                    Convert.ToInt32(statistic.Goals), Convert.ToInt32(statistic.Assists), Convert.ToInt32(statistic.YellowCard),
                    Convert.ToInt32(statistic.RedCard), Convert.ToInt32(statistic.Matches));

                //dodaje nowy obiekt do listy scalonych
                mergedPlayersStatisticsList.Add(mergedPlayer);
            }

            //zwraca cala gotowa liste
            return mergedPlayersStatisticsList;
        }


        public List<Player> SortPlayers(List<Player> Plrs) // metoda sortuje zawodnikow po pozycji
        {
            //deklaruje kolejno pozycja po jakich ma sortowac zawodnikow
            List<string> positions = new List<string> { "Bramkarz", "Obronca", "Pomocnik", "Napastnik" };

            //deklaruje liste wynikow, posortowanych
            List<Player> ResultList = new List<Player>();

            //fetle zagniezdzone przechodza po zawodnikach i jezeli zawodnik ma odpowiednia pozycja to dodaje go do listy
            foreach (string elem in positions)
            {
                foreach (Player elemP in Plrs)
                {
                    if (elemP.Position == elem)
                    {
                        ResultList.Add(elemP);
                    }
                }
            }
            return ResultList;
        }

    }
}
