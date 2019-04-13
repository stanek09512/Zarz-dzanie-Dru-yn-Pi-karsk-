using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InzBackCore.Domain
{
    public class MatchhPlayer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        public int? Goals { get; set; }
        public int? Assists { get; set; }
        public int? YellowCard { get; set; }
        public int? RedCard { get; set; }
        public bool? PlayInMatch { get; set; }
        [Key]
        public int? MatchhId { get; set; }
        public Matchh Matchh { get; set; }
        [Key]
        public int? PlayerId { get; set; }
        public Player Player { get; set; }
        //public int? userId { get; set; }
        //public User user { get; set; }

        public MatchhPlayer()
        { }

        public MatchhPlayer(int goals, int assists, int yellowC, int redC, bool playInMatch, Matchh matchh, Player player)
        {
            Goals = goals;
            Assists = assists;
            YellowCard = yellowC;
            RedCard = redC;
            PlayInMatch = playInMatch;
            Matchh = matchh;
            Player = player;
        }
        public MatchhPlayer(int goals, int assists, int yellowC, int redC, bool playInMatch)
        {
            Goals = goals;
            Assists = assists;
            YellowCard = yellowC;
            RedCard = redC;
            PlayInMatch = playInMatch;
        }
        public MatchhPlayer(int goals, int assists, int yellowC, int redC)
        {
            Goals = goals;
            Assists = assists;
            YellowCard = yellowC;
            RedCard = redC;
        }
    }
}
