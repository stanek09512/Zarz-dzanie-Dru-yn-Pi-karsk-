using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using InzBackCore.Domain;
using InzBackCore.Repositories;
using InzBackInfrastructure.Commands.Matches;
using InzBackInfrastructure.DTO;
using InzBackInfrastructure.Repositories;
using System.Linq;

namespace InzBackInfrastructure.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        public MatchService(IMatchRepository matchRepository, IMapper mapper, IPlayerRepository playerRepository, IAuthRepository authRepository)
        {
            _matchRepository = matchRepository;
            _authRepository = authRepository;
            _mapper = mapper;
            _playerRepository = playerRepository;
        }
        public async Task<Matchh> GetAsyncMatch(int id) //metoda pobiera mecz
        {
            var match = await _matchRepository.GetAsyncMatch(id);
            if(match == null)
            {
                throw new Exception($"Match with this id {id} does not exist");
            }
            return match;
        }
        public async Task<IEnumerable<Matchh>> GetAsyncMatchesFuture(int userId)  //metoda pobiera mecze ktore sa zaplanowane na przyszłosc
        {
            //pobiera wszystkie mecze danego uzytkownika i konwertuje do listy
            var matches = await _matchRepository.GetAsyncMatches(userId);
           
            List<Matchh> matchesList = matches.ToList();
            
            for (int i = 0; i < matchesList.Count(); i++)
            {
                // warunek- jezeli data jest niższa od terazniejszej to mecz zostaje usuniety z listy
                DateTime? date = matchesList[i].MatchDate;
                if (date < DateTime.Now)  
                {
                    matchesList.Remove(matchesList[i]);
                    // iteracja zostaje cofnieta o jeden element poniewaz element zostal usuniety a stos obsunąl sie o element
                    i--;    
                }
            }
            // sortuje liste meczy po dacie
            matchesList = matchesList.OrderBy(x => x.MatchDate).ToList(); 
            return matchesList;
        }
        public async Task<IEnumerable<Matchh>> GetAsyncMatchesPast(int userId) // pobiera mecze z przeszlosci
        {
            var matches = await _matchRepository.GetAsyncMatches(userId);
            List<Matchh> matchesList = matches.ToList();

            for (int i = 0; i < matchesList.Count(); i++)
            {
                // warunek- jezeli data jest wyzsza od terazniejszej to mecz zostaje usuniety z listy
                DateTime? date = matchesList[i].MatchDate;
                if (date > DateTime.Now)  
                {
                    matchesList.Remove(matchesList[i]);
                    i--;   
                }
            }
            // sortuje liste meczy po dacie
            matchesList = matchesList.OrderBy(x => x.MatchDate).ToList(); 
            return matchesList;
        }

        public async Task<IEnumerable<Matchh>> GetNearestMatch(int userId) //metoda pobiera najblizszy/najblizsze mecze
        {
            var matches = await GetAsyncMatchesFuture(userId);    
            List<Matchh> matchh = matches.ToList();

            // warunek - jezeli lista nie jest pusta, sa zaplanowane jakies mecze na przyszlosc
            if (matchh.Count != 0)   
            {
                // przypisuje pierwszy obiekt z listy
                Matchh nearestMatch = matchh[0];

                // przechodzi przez cala liste nie liczac pierwszego elementu
                for ( int i = 1; i < matchh.Count(); i++)   
                {
                    //warunek - jezeli data jest wieksza to przypisuje ta mniejsza czyli blizsza czasu terazniejszego
                    if (nearestMatch.MatchDate > matchh[i].MatchDate)   
                    {
                        nearestMatch = matchh[i];
                    }
                }

                for (int i = 0; i < matchh.Count(); i++)    
                {
                    // warunek - jezeli znajduje sie jakas data bardziej na przyszlosc w liscie to ja usuwa, przez to ze moga byc np 2 mecze w tym samym czasie
                    if (nearestMatch.MatchDate < matchh[i].MatchDate)  
                    {
                        matchh.Remove(matchh[i]);   
                        i--;    
                    }
                }
                return matchh;
            }
            return null;
        }

        public async Task<List<double>> GetAsyncTimeToMatchValue(int userId)    //pobiera liste wartosci do obsłuzenia paska postepu czasu do najblizszego meczu
        {
            // deklaracja listy wynikow potrzebnych 
            List<double> results = new List<double>();

            // pobiera najbliższy mecz
            //warunek - jezeli nie ma meczu konczymy obliczenia
            var matches =await GetNearestMatch(userId); 
            if( matches == null )
            {
                return null;
            }
            // zmienia na liste bo moga byc np 2 w tym samym czasie
            List<Matchh> matchObj = matches.ToList();

            // data terazniejsza - data meczu zaplanowanego

            // konwertuje date tego meczu
            DateTime matchDate = Convert.ToDateTime(matchObj[0].MatchDate);
            // pobiera date terazniejsza
            DateTime nowDate = Convert.ToDateTime(DateTime.Now);
            // odejmuje date najblizszego meczu od daty terazniejszej i uzyskuje wynik, ile godzin pozostalo do meczu
            TimeSpan finalResultFutureMatch = matchDate.Subtract(nowDate);
            // umiesza wynik w liscie wynikow na miejscu 0
            results.Add(finalResultFutureMatch.TotalHours);

            // data terazniejsza - data meczu ostatniego/najbliższego przeszlego

            // pobiera wszystkie mecze z przeszlosci
            var matchesPast = await GetAsyncMatchesPast(userId);   
            List<Matchh> matchesPastList = matchesPast.ToList();
            // sortuje po dacie
            matchesPastList = matchesPastList.OrderBy(x => x.MatchDate).ToList();
            // deklaracja zmiennej na date meczu przeszlego najblizszego
            DateTime pastMatch; 
            TimeSpan finalResultPastMatch;
            //warunek - jezeli mecze przeszle istnieja
            if (matchesPastList.Count != 0)
            {
                // konwertuje date z ostatniego po sortowaniu czyli najblizszemu terazniejszosci
                pastMatch = Convert.ToDateTime(matchesPastList[matchesPastList.Count - 1].MatchDate);
                // odejmuje od daty terazniejszej date meczu przeszlego, uzyskujemy wynik czasu 
                finalResultPastMatch = nowDate.Subtract(pastMatch);    
            }
            else
            {
                // jezeli nie ma meczu w przeszlosci to liczy od czasu terazniejszego tzn czas terazniejszy - 0
                finalResultPastMatch = TimeSpan.Zero;
            }
            //wynik czasu data terazniejsza - mecz przeszly umieszcza na miejscu 1 w liscie
            results.Add(finalResultPastMatch.TotalHours);   

            // wynik czasu mecz przeszly do daty terazniejszej + mecz przyszly do daty terazniejszej umieszcza na miejscu 2 w liscie
            results.Add(results[0] + results[1]);
            // liczy procent z czasu ile zostalo do meczu przez sume czasu od ostatniego do nastepnego i dodaje na pozycji 3 w liscie
            results.Add(100-((results[0] / results[2])*100));  
            results[0] = Convert.ToInt32(results[0]); 

            return results; 
    }

        public async Task<int> CreateAsyncMatch(CreateMatch matchData, int userId)  //metoda tworzaca nowy mecz 
        {
            var user = await _authRepository.UserAccount(userId);
            //zera to wynik meczu zadeklarowane z gory
            var match = new Matchh(matchData.OpponentTeam, matchData.MatchDate, matchData.Place);    
            match.user = user;
            int matchId = await _matchRepository.AddAsyncMatch(match, userId);
            return matchId;
        }

        public async Task UpdateAsyncMatch(int id, UpdateMatch matchData)   //metoda edytujaca dane meczu 
        {
            var match = await _matchRepository.GetAsyncMatch(id);
            if (match == null)
            {
                throw new Exception($"Match with this id {id} does not exist");
            }
            match.SetNameOpponentTeam(matchData.OpponentTeam);
            match.SetMatchDate(matchData.MatchDate);
            match.SetPlace(matchData.Place);
            match.SetScoreFirstTeam(matchData.ScoreFirstTeam);
            match.SetScoreSecondTeam(matchData.ScoreSecondTeam);
            await _matchRepository.UpdateAsyncMatch(match);
        }

        public async Task DeleteAsyncMatch(int id)  // metoda usuwa mecz
        {
            var match = await _matchRepository.GetAsyncMatch(id);
            if (match == null)
            {
                throw new Exception($"Match with this id {id} does not exist");
            }
            await _matchRepository.DeleteAsyncMatch(match);
        }

        public async Task AddPlayersToMatch(int MatchId, IEnumerable<int> PlayersId)    // metoda dodaje zawodnikow do meczu
        {
            var match = await _matchRepository.GetAsyncMatch(MatchId);
            if (match == null)
            {
                throw new Exception($"Match with this id {MatchId} does not exist");
            }
            //await _matchRepository.AddAsyncPlayerToMatch(MatchId,PlayersId);
            var playersListInMatch = await _matchRepository.GetAsyncPlayersInMatch(MatchId);
            foreach (int playerId in PlayersId)   //przechodze przez wszystkie id playersow ktorych chce dodac do meczu
            {
                var plr2matches = new MatchhPlayer(); //deklaruje obiekt tablicy posredniej
               
                bool IsInList = false;
                IsInList = playersListInMatch.Players2Match.Exists(x => x.PlayerId == playerId);  // jezeli jest juz przypisany to tu zmienia na prawde

                if (IsInList == false) //dzieki temu widzimy czy nalezy jezeli falsz czyli nie to dodajemy
                {
                    
                    var player = await _playerRepository.GetAsyncPlayer(playerId);   //pobieram zawodnika o wskazanym id
                    if (player == null) continue;
                    else
                    {
                        plr2matches.Matchh = match;   //wypelniamy obiekt tabeli posredniej
                        plr2matches.Player = player;
                        await _matchRepository.AddAsyncPlayerToMatch(MatchId, plr2matches);
                       // match.Players2Match.Add(plr2matches);

                    }
                }
            }

            await Task.CompletedTask;
        }

        

        public async Task<IEnumerable<PlayerDto>> GetPlayersInMatch(int MatchId)    //pobiera liste zawodnikow w meczu, pelnych obiektow zawodnik
        {
            // pobiera mecz z lista zawodnikow dopisanych do niego
            var playersInMatch = await _matchRepository.GetAsyncPlayersInMatch(MatchId);
            if (playersInMatch == null)
            {
                throw new Exception($"Match with this id {MatchId} does not exist");
            }

            //przechodzi przez liste dopisanych zawodnikow do meczu, pobiera id nastepnie pobiera zawodnika i caly obiekt zawodnika dodaje do listy
            var playerList = new List<Player>();
            for (int i = 0; i < playersInMatch.Players2Match.Count; i++)
            {
                int playerId = Convert.ToInt32(playersInMatch.Players2Match[i].PlayerId);
                var player = await _playerRepository.GetAsyncPlayer(playerId);
                playerList.Add(player);
            }
            return _mapper.Map<IEnumerable<PlayerDto>>(playerList);
        }
        public async Task<IEnumerable<PlayerDto>> GetPlayersOutMatch(int Matchid, int userId)
        {
            //sprawdza czy mecz istnieje
            var match = await _matchRepository.GetAsyncMatch(Matchid);
            if (match == null)
            {
                throw new Exception($"Match with this id {Matchid} does not exist");
            }

            //pobiera wszytskich zawodnikow uzytkownika i mapuje na obiektDto a nastepnie rzutuje na liste
            var allUserPlayers = await _playerRepository.GetAsyncAllPlayers(userId);
            var allUserPlayersDto = _mapper.Map<IEnumerable<PlayerDto>>(allUserPlayers);
            List<PlayerDto> allUserPlayersDtoList = allUserPlayersDto.ToList(); 

            //pobiera liste zawodnikow dodanych do meczu
            var playersInMatch = await GetPlayersInMatch(Matchid);
            List<PlayerDto> playersInMatchList = playersInMatch.ToList();

            // usuwa z listy wszystkich zawodnikow ktorzy sa w kadrze
            foreach (var player in playersInMatch)
            {
                var plrToRemove = allUserPlayersDtoList.FirstOrDefault( x => x.Id == player.Id);
                if(plrToRemove != null) allUserPlayersDtoList.Remove(plrToRemove);
            }

            return allUserPlayersDtoList;
        }
        public async Task DeletePlayersInMatch(int Matchid, IEnumerable<int> PlayersId)
        {
        
            var playersListInMatch = await _matchRepository.GetAsyncPlayersInMatch(Matchid);  //pobiera mecz z zawarta lista zawodnikow przypisana do niego
            if (playersListInMatch == null)
            {
                throw new Exception($"Match with this id {Matchid} does not exist");
            }

            foreach (var player in PlayersId) //rpzechodzimy kolejno po indeksach ktore wskazuja ktorych zawodnikow nie chcemy w druzynie
            {
                var plr = playersListInMatch.Players2Match.SingleOrDefault(x => x.PlayerId == player);  //pobieramy zawdonika pod indeksem
                if (plr == null) continue;  // jezeli nie ma takiego to kontynuujemy
                else
                {
                    await _matchRepository.DeleteAsyncListPlayersInMatch(Matchid, plr);
                }
            }
            await Task.CompletedTask;

        }
        public async Task UpdateStatsInMatch(int Matchid, IEnumerable<GetPlayersStatisticInMatchDto> PlayersStats)
        {
            var playersListInMatch = await _matchRepository.GetAsyncPlayersInMatch(Matchid); //pobieram mecz a w nim liste z jakimi playerami sie łączy
            if (playersListInMatch == null)
            {
                throw new Exception($"Match with this id {Matchid} does not exist");
            }
            var matchhPlayer = _mapper.Map<IEnumerable<MatchhPlayer>>(PlayersStats);
            List<MatchhPlayer> matchhPlayerList = matchhPlayer.ToList();

            foreach (var playerS in PlayersStats)
            {
                var plr = playersListInMatch.Players2Match.SingleOrDefault(x => x.PlayerId == playerS.PlayerId);    //pobieramy zawodnika o id
               // playersListInMatch.Players2Match.Remove(plr);   //usuwamy go na chwile bo nie mozna albo nie wiem jak nadpisac
                await _matchRepository.DeleteAsyncListPlayersInMatch(Matchid, plr);
                //deklaruje obiekt tablicy posredniej
                var match = await _matchRepository.GetAsyncMatch(Matchid);   //pobieram mecz o wskazanym id
                var player = await _playerRepository.GetAsyncPlayer(Convert.ToInt32(playerS.PlayerId));    //pobieram zawodnika o wskazanym id  
                if (player != null)
                {
                    var plr2matches = new MatchhPlayer(Convert.ToInt16(playerS.Goals), Convert.ToInt16(playerS.Assists), Convert.ToInt16(playerS.YellowCard), Convert.ToInt16(playerS.RedCard), Convert.ToBoolean(playerS.PlayInMatch), match, player);
                    await _matchRepository.AddAsyncPlayerStatisticToMatch(Matchid, plr2matches);
                }

            }
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<MergedPlayersStatisticsDto>> GetListPlayersStatistics(int Matchid)   // pobiera statysyki zawodnikow w konkretnym meczu
        {
           

            var match = await _matchRepository.GetAsyncMatch(Matchid);
            if (match == null)
            {
                throw new Exception($"Match with this id {Matchid} does not exist");
            }
     

            
            var playersListInMatch = await _matchRepository.GetAsyncPlayersInMatch(Matchid);   //pobieram mecz a w nim liste z jakimi playerami sie łączy 
            var playerList = new List<MatchhPlayer>();
            for (int i = 0; i < playersListInMatch.Players2Match.Count; i++)
            {
                 var player = new MatchhPlayer(Convert.ToInt16(playersListInMatch.Players2Match[i].Goals), Convert.ToInt16(playersListInMatch.Players2Match[i].Assists), Convert.ToInt16(playersListInMatch.Players2Match[i].YellowCard), Convert.ToInt16(playersListInMatch.Players2Match[i].RedCard), Convert.ToBoolean(playersListInMatch.Players2Match[i].PlayInMatch));   //wyszukuje go
                 player.PlayerId = Convert.ToInt16(playersListInMatch.Players2Match[i].PlayerId); //wybieram zawodnika dopisanego do meczu
                 playerList.Add(player); //i dodaje go do listy
            }



            List<MergedPlayersStatisticsDto> AllInfoListPlrs = new List<MergedPlayersStatisticsDto>(); 
            List<MatchhPlayer> playersSList = playerList.ToList();    // konwertuje ienumerable do listy zeby moc wykonac swoje dzialania
            for(int i =0; i< playersSList.Count; i++)
            {
                MergedPlayersStatisticsDto playerStatsInMatchFull = new MergedPlayersStatisticsDto();
                var plr = _playerRepository.GetAsyncPlayer(Convert.ToInt16(playersSList[i].PlayerId));
                Player plaObj = plr.Result;
                playerStatsInMatchFull.PlayerId = Convert.ToInt16(playersSList[i].PlayerId);
                playerStatsInMatchFull.Name = plaObj.Name;
                playerStatsInMatchFull.Surname = plaObj.Surname;
                playerStatsInMatchFull.Position = plaObj.Position;
                playerStatsInMatchFull.Goals = Convert.ToInt32(playersSList[i].Goals);
                playerStatsInMatchFull.Assists = Convert.ToInt32(playersSList[i].Assists);
                playerStatsInMatchFull.YellowCard = Convert.ToInt32(playersSList[i].YellowCard);
                playerStatsInMatchFull.RedCard = Convert.ToInt32(playersSList[i].RedCard);
                playerStatsInMatchFull.PlayInMatch = playersSList[i].PlayInMatch;
                AllInfoListPlrs.Add(playerStatsInMatchFull);
            }

            //return _mapper.Map<IEnumerable<GetPlayersStatisticInMatchDto>>(playersS);
            AllInfoListPlrs = SortPlayers(AllInfoListPlrs);
            return AllInfoListPlrs;
        }

        public List<MergedPlayersStatisticsDto> SortPlayers(List<MergedPlayersStatisticsDto> Plrs) // metoda sortuje zawodnikow po pozycji
        {
            List<string> positions = new List<string> { "Bramkarz", "Obronca", "Pomocnik", "Napastnik" };
            List<MergedPlayersStatisticsDto> FinalListPlrs = new List<MergedPlayersStatisticsDto>();

            foreach (string elem in positions)
            {
                foreach (MergedPlayersStatisticsDto elemP in Plrs)
                {
                    if (elemP.Position == elem)
                    {
                        FinalListPlrs.Add(elemP);
                    }
                }
            }
            return FinalListPlrs;
        }

    }
}
