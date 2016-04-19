using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Robot.Common
{
    public delegate void RobotStepCompletedEventHandler(object sender, RobotStepCompletedEventArgs e);

    public abstract class RobotCommand
    {
        public abstract RobotStepCompletedEventArgs ChangeModel(IList<Robot> robots, int currentIndex, Map map);
        public void Apply(IList<Robot> robots, int currentIndex, Map map)
        {
            InvokeRobotStepCompleted(ChangeModel(robots, currentIndex, map));
        }

        public event RobotStepCompletedEventHandler RobotStepCompleted;

        public void InvokeRobotStepCompleted(RobotStepCompletedEventArgs e)
        {
            RobotStepCompletedEventHandler handler = RobotStepCompleted;
            if (handler != null) handler(this, e);
        }

        public string Description { get; set; }
    }




    public class RobotStepCompletedEventArgs
    {
        public Owner Owner;
        public Position RobotPosition;
        public Position NewRobotPosition;
        //Father+ son energy change
        public int TotalEnergyChange;


        public List<Position> MovedFrom;
        public List<Position> MovedTo;
    }


    public sealed class MoveCommand : RobotCommand
    {
        public Position NewPosition { get; set; }
        public const int ShoveEnergyLoss = 100;

        public override RobotStepCompletedEventArgs ChangeModel(IList<Robot> robots, int currentIndex, Map map)
        {
            var result = new RobotStepCompletedEventArgs();

            //skip movement if not valid
            if (!map.IsValid(NewPosition))
            {
                Description = string.Format("FAILED: {0} position not valid ", NewPosition);
                return result;
            }
            

            var myRobot = robots[currentIndex];
            var oldPosition = robots[currentIndex].Position;
            int moveEnergyLoss = (int)(Math.Pow(NewPosition.X - oldPosition.X, 2) + Math.Pow(NewPosition.Y - oldPosition.Y, 2));

            Robot movedRobot = null;


            foreach (var robot in robots)
            {

                    if ((robot != myRobot) && (robot.Position == NewPosition))
                    {

                        //moving 
                        //var energyOfFightLoss =  Math.Min(robot.Energy, myRobot.Energy);
                        moveEnergyLoss += ShoveEnergyLoss;
                        movedRobot = robot;
                    }
            }


            //if not enough energy than skip the movement
            if (myRobot.Energy < moveEnergyLoss)
            {
                Description = "FAILED: not enough energy to move";
                return new RobotStepCompletedEventArgs();
            }
            


            result.TotalEnergyChange = -moveEnergyLoss;
            result.MovedFrom = new List<Position>() { oldPosition };
            result.MovedTo = new List<Position>() { NewPosition };

            myRobot.Energy -= moveEnergyLoss;
            myRobot.Position = NewPosition;
            Description = String.Format("MOVE: {0}-> {1}", oldPosition, NewPosition);

            if (movedRobot != null)
            {
                
                movedRobot.Position = map.FindFreeCell(NewPosition, robots);
                result.MovedFrom.Insert(0, NewPosition);
                result.MovedTo.Insert(0, movedRobot.Position);
                Description += string.Format(" . Shoved {0} robot", movedRobot.Owner.Name);
            }
           
            return result;
        }
    }

    public sealed class CreateNewRobotCommand : RobotCommand
    {
        public const int MinEnergyToCreateNewRobot = 400;
        public const int EnergyLossToCreateNewRobot = 300;
        public const int NewRoborEnergy = 100;



        public override RobotStepCompletedEventArgs ChangeModel(IList<Robot> robots, int currentIndex, Map map)
        {
            var result = new RobotStepCompletedEventArgs();
            var myRobot = robots[currentIndex];
            Robot newRobot = null;
            if (myRobot.Energy > MinEnergyToCreateNewRobot)
            {
                var position = map.FindFreeCell(myRobot.Position, robots);
                newRobot = new Robot() { Position = position, Energy = NewRoborEnergy, Owner = myRobot.Owner };
                robots.Add(newRobot);
                myRobot.Energy -= EnergyLossToCreateNewRobot;

                result.NewRobotPosition = position;
                result.TotalEnergyChange = -EnergyLossToCreateNewRobot;

                Description = String.Format("New: {0}", result.NewRobotPosition);
            }
            else
            {
                Description = String.Format("FAILED: not enough energy to create new robot");
            }

            
            return result;
        }
    }

    public sealed class CollectEnergyCommand : RobotCommand
    {

        public const int MaxEnergyToEat = 50;
        public override RobotStepCompletedEventArgs ChangeModel(IList<Robot> robots, int currentIndex, Map map)
        {
            var result = new RobotStepCompletedEventArgs();

            var myRobot = robots[currentIndex];
            var resource = map.GetResource(myRobot.Position);
            if (resource != null)
            {
                var energy = Math.Min(resource.Energy, MaxEnergyToEat);
                myRobot.Energy += energy;
                resource.Energy -= energy;
                result.TotalEnergyChange = energy;
                Description = String.Format("COLLECT: {0}", energy);
            }
            else
            {
                Description = String.Format("FAILED: no resource to collect energy");
            }

            return result;
        }
    }
}
