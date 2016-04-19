using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Robot.Common
{
    public sealed class Robot{

        public const int MaxEnergy = 4000;

        public Position Position { get; set; }
        public int Energy { get; set; }
        public Owner Owner { get; set; }
    }
}
