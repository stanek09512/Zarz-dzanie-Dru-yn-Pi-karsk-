using System;
using System.Collections.Generic;
using System.Text;

namespace InzBackInfrastructure.DTO
{
    public class PlayerInMatchStats
    {
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCard { get; set; }
        public int RedCard { get; set; }
        public bool PlayInMatch { get; set; }
        public int PlayerId { get; set; }

    }
}
