using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Robot.Common
{
    
    class Variant
    {
        internal int MaxEnergyGrowth ;
        internal int MinEnergyGrowth = 5;
        internal int EnergyStationForAttendant = 5;
        internal int EnergyForPoint = 1000;

        //If more needed start energy can be added
        internal Variant(int variantNum)
        {
            switch (variantNum)
            {
                case 1:
                    //Normal
                    MaxEnergyGrowth = 10;
                    MinEnergyGrowth = 5;
                    EnergyStationForAttendant = 5;
                    break;
                case 2:
                    //War
                    //EnergyForPoint = 2000;
                    MaxEnergyGrowth = 20;
                    MinEnergyGrowth = 10;
                    EnergyStationForAttendant = 5;
                    break;
                case 3:
                    //weak
                    EnergyForPoint = 500;
                    MaxEnergyGrowth = 10;
                    MinEnergyGrowth = 5;
                    EnergyStationForAttendant = 2;
                    break;
                case 4:
                    //War
                    MaxEnergyGrowth = 25;
                    MinEnergyGrowth = 10;
                    EnergyStationForAttendant = 2;
                    break;
                default:
                    throw new Exception("Not supported variant");
                    
            }

        }
    }
}
