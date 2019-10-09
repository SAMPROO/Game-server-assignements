using System;
using System.Collections.Generic;


namespace ShipGame
{
    public class Match
    {
        public Match(Player _player1 ,Player _player2)
        {
            Player1 = _player1;
            Player2 = _player2;
            MatchId = Guid.NewGuid();
        }
        public readonly Guid MatchId;
        public Player Player1;
        public Player Player2;
    }
    public class Player
    {
        public Player(string _name,Ship[] _ships)
        {
            Name = _name;
            Ships = _ships;
        }
        public string Name {get; private set;}
        public Ship[] Ships {get; private set;}
    }
    public class Ship
    {
        public Ship(Coordinate start, Coordinate end)
        {
            ShipParts = new List<Coordinate>();

            if(start.X == end.X || start.Y == end.Y)
            {
                int xDelta = Math.Abs(end.X - start.X);
                int yDelta = Math.Abs(end.Y - start.Y);
                int xStart = Coordinate.GetLower(start, end).X;
                int yStart = Coordinate.GetLower(start, end).Y;
                if(xDelta == 0 || yDelta == 0)
                {
                    if(xDelta>4 || yDelta>4)
                    {
                        Console.WriteLine("Illegal");
                        // throw new IllegalShipException;
                    }
                    for(int i = 0; i < xDelta; i++)
                    {
                        ShipParts.Add(new Coordinate(xStart+i,yStart));
                    }
                    for(int i = 0; i < yDelta; i++)
                    {
                        ShipParts.Add(new Coordinate(xStart,yStart+i));
                    }
                }
                else
                {
                    Console.WriteLine("Illegal");
                    // throw new IllegalShipException;
                }
            }
            else
            {
                Console.WriteLine("Illegal");
                // throw new IllegalShipException;
            }
        }
        public Coordinate[] GetShipParts()
        {
            return ShipParts.ToArray();
        }

        public List<Coordinate> ShipParts;
    }
    public struct Coordinate
    {
        public Coordinate(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        public static Coordinate GetLower(Coordinate coord1,Coordinate coord2)
        {
            double coord1Magnitude = Math.Sqrt((Math.Pow((float)coord1.X,2f) + Math.Pow((float)coord1.Y,2f)));
            double coord2Magnitude = Math.Sqrt((Math.Pow((float)coord2.X,2f) + Math.Pow((float)coord2.Y,2f)));
            if(coord1Magnitude < coord2Magnitude)
            {
                return coord1;
            }
            else
            {
                return coord2;
            }
        }
        public int X {get; set;}
        public int Y {get; set;}
    }
}