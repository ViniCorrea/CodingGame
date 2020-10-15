using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Player
{
    private static int wait_need = 0;
    private static int nbFloors;
    private static int width;
    private static int nbRounds;
    private static int exitFloor;
    private static int exitPos;
    private static int nbTotalClones;
    private static int nbAdditionalElevators;
    private static int nbElevators;
    private static List<Elevator> elevators = new List<Elevator>();

    static void Main(string[] args)
    {
        string[] inputs;
        var readline = Console.ReadLine();
        Console.Error.WriteLine(readline);
        inputs = readline.Split(' ');
        nbFloors = int.Parse(inputs[0]); // number of floors
        width = int.Parse(inputs[1]); // width of the area
        nbRounds = int.Parse(inputs[2]); // maximum number of rounds
        exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        nbTotalClones = int.Parse(inputs[5]); // number of generated clones
        nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
        nbElevators = int.Parse(inputs[7]); // number of elevators
        for (int i = 0; i < nbElevators; i++)
        {
            readline = Console.ReadLine();
            Console.Error.WriteLine(readline);
            inputs = readline.Split(' ');
            elevators.Add(new Elevator
            {
                ElevatorFloor = int.Parse(inputs[0]),// floor on which this elevator is found
                ElevatorPos = int.Parse(inputs[1])// position of the elevator on its floor
            });
        }

        var clone = new Clone();
        // game loop
        while (true)
        {
            readline = Console.ReadLine();
            clone.ReadNow(readline);

            if (wait_need > 0)
            {
                wait_need--;
                Console.WriteLine("WAIT");
            }
            else
            {
                var direction = Direction(clone);
                if (direction == "STOP")
                {
                    wait_need = 1;
                    Console.WriteLine("WAIT");
                }
                else if ((clone.Direction == "LEFT" || clone.Direction == "RIGHT") && clone.Direction != direction)
                {
                    Console.WriteLine("BLOCK");
                    wait_need = 3;
                }
                else
                {
                    Console.WriteLine("WAIT");
                }
            }
        }
    }

    public static string Direction(Clone clone)
    {
        // Exit is in this floor?
        var needPosition = (clone.CloneFloor == exitFloor) ? exitPos : -1;
        if (needPosition >= 0)
        {
            if (needPosition < clone.ClonePos)
            {
                return "LEFT";
            }
            else if (needPosition > clone.ClonePos)
            {
                return "RIGHT";
            }
        }
        else
        {
            var elevator = elevators.FirstOrDefault(e => e.ElevatorFloor == clone.CloneFloor);
            if (elevator == null)
                return Direction(clone);

            var whereToGo = width > Math.Abs(elevator.ElevatorPos - clone.ClonePos) ? elevator.ElevatorPos : 0;

            if (whereToGo < clone.ClonePos)
                return "LEFT";

            if (whereToGo > clone.ClonePos)
                return "RIGHT";
        }

        return "STOP";
    }
}

public class Clone
{

    public int ClonePos { get; set; }
    public int CloneFloor { get; set; }
    public string Direction { get; set; }
    public int CurrentFloor { get; set; }
    public void ReadNow(string readline)
    {
        var inputs = readline.Split(' ');
        Console.Error.WriteLine(readline);
        CloneFloor = int.Parse(inputs[0]); // floor of the leading clone
        ClonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
        Direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT
        CurrentFloor = 0;
    }
}

public class Elevator
{
    public int ElevatorFloor { get; set; }
    public int ElevatorPos { get; set; }
}