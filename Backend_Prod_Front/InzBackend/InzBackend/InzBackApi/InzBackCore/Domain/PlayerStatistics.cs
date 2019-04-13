using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InzBackCore.Domain
{
    public class PlayersStatictics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Matches { get; protected set; }
        public int? Goals { get; protected set; }
        public int? Assists { get; protected set; }
        public int? YellowCard { get; protected set; }
        public int? RedCard { get; protected set; }
        [Key]
        public int? PlayerId { get; set; }
        public Player player { get; set; }
        public PlayersStatictics()
        { }
        public PlayersStatictics( int Matches, int Goals, int Assists, int YellowCard, int RedCard, Player player)
        {
            this.Matches = Matches;
            this.Goals = Goals;
            this.Assists = Assists;
            this.YellowCard = YellowCard;
            this.RedCard = RedCard;
            this.player = player;
        }
        public void SetMatches(int matches)
        {
            if (matches < 0)
            {
                throw new Exception($"Matches count must be minimum 0");
            }
            Matches = matches;
        }
        public void SetGoals(int goals)
        {
            if (goals < 0)
            {
                throw new Exception($"Goals count must be minimum 0");
            }
            Goals = goals;
        }
        public void SetAssists(int assists)
        {
            if (assists < 0)
            {
                throw new Exception($"Assists count must be minimum 0");
            }
            Assists = assists;
        }
        public void SetYellowCards(int yellowCard)
        {
            if (yellowCard < 0)
            {
                throw new Exception($"YellowCard count must be minimum 0");
            }
            YellowCard = yellowCard;
        }
        public void SetRedCard(int redCard)
        {
            if (redCard < 0)
            {
                throw new Exception($"RedCard count must be minimum 0");
            }
            RedCard = redCard;
        }
    }
    
}
