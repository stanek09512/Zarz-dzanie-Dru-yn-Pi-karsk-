﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InzBackInfrastructure.Commands.Matches
{
    public class GetPlayersStatisticInMatchDto
    {
        public int PlayerId { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCard { get; set; }
        public int RedCard { get; set; }
        public bool PlayInMatch { get; set; }
    }
}
