using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Robot.Common
{
    public sealed class Owner
    {
        public string Name {
            get { return Algorithm.Author; }
        }
        public IRobotAlgorithm Algorithm { get; set; }

        public Owner Copy()
        {
            { return new Owner(){Algorithm = Algorithm}; }
        }
        
    }
}
