using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Robot.Common
{
    public sealed class EnergyStation
    {
        public const int MaxEnergy = 1000;

        /// <summary>
        /// How many energy is restored for the one full round. Full round is when all robots do a move. 
        /// </summary>
        public int RecoveryRate;
        public int Energy;
        public Position Position;
    }
}
