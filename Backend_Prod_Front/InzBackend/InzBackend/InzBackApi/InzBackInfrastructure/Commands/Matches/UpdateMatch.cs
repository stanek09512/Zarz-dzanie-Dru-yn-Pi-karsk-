using System;
using System.Collections.Generic;
using System.Text;

namespace InzBackInfrastructure.Commands.Matches
{
    public class UpdateMatch
    {
        public string OpponentTeam { get;  set; }
        public DateTime MatchDate { get;  set; }
        public string Place { get;  set; }
        public int ScoreFirstTeam { get;  set; }
        public int ScoreSecondTeam { get;  set; }
   
    }
}
