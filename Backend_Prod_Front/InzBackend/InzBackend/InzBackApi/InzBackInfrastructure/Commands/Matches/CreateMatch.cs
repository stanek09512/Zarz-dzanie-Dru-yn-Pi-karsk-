using System;
using System.Collections.Generic;
using System.Text;

namespace InzBackInfrastructure.Commands.Matches
{
    public class CreateMatch
    {
        public string OpponentTeam { get; set; }
        public DateTime MatchDate { get; set; }
        public string Place { get; set; }
    }
}
