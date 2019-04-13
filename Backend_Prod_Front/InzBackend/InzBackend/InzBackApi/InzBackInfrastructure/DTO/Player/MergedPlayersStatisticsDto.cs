using System;
using System.Collections.Generic;
using System.Text;

namespace InzBackInfrastructure.DTO
{
    public class MergedPlayersStatisticsDto
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Position { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? YellowCard { get; set; }
        public int? RedCard { get; set; }
        public bool? PlayInMatch { get; set; }
        public int? Matches { get; set; }   // ilosc zagranych meczy dla klubu


        public MergedPlayersStatisticsDto()
        {

        }

        public MergedPlayersStatisticsDto(int PlayerId, string Name, string Surname,
            string Position, int Goals, int Assists, int YellowCard, int RedCard, int Matches)
        {
            this.PlayerId = PlayerId;
            this.Name = Name;
            this.Surname = Surname;
            this.Position = Surname;
            this.Matches = Matches;
            this.Goals = Goals;
            this.Assists = Assists;
            this.YellowCard = YellowCard;
            this.RedCard = RedCard;
        }

        public MergedPlayersStatisticsDto(int PlayerId, string Name, string Surname,
            string Position, int Goals, int Assists, int YellowCard, int RedCard, bool PlayInMatch, int Matches)
        {
            this.PlayerId = PlayerId;
            this.Name = Name;
            this.Surname = Surname;
            this.Position = Position;
            this.Matches = Matches;
            this.Goals = Goals;
            this.Assists = Assists;
            this.YellowCard = YellowCard;
            this.RedCard = RedCard;
            this.PlayInMatch = PlayInMatch;

        }


    }
}
