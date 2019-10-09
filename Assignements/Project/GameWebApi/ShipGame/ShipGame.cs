using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;


namespace ShipGame
{
    public class Match
    {
        public Match(Player _player1 ,Player _player2)
        {
            InProgress = true;
            Player1 = _player1;
            Player2 = _player2;

        }
        public Match()
        {
            InProgress = true;
        }

        public Guid Id                  {get; set;}
        public bool InProgress          {get; set;}   
        public Player Player1           {get; set;}
        public Player Player2           {get; set;}
    }
    public class Player
    {
        public Player()
        {

        }
        public Player(string _name, List<Ship> _ships)
        {
            Id = Guid.NewGuid();
            Name = _name;
            Ships = _ships;
        }
        
        public Guid Id {get; set;}
        public string Name {get; set;}
        public List<Ship> Ships {get; set;}
    }
    public class NewPlayer
    {
        public string Name {get;set;}
    }
    public class Ship
    {

        public Ship(Coordinate start, Coordinate end)
        {
            ShipParts = new List<Coordinate>();
            CreateShip(start,end);
            
        }
        public Ship()
        {
            ShipParts = new List<Coordinate>();
        }
        public void CreateShip(Coordinate start, Coordinate end)
        {
            if(start.X == end.X || start.Y == end.Y)
            {
                int xDelta = Math.Abs(end.X - start.X);
                int yDelta = Math.Abs(end.Y - start.Y);
                if(yDelta>0)
                    yDelta += 1;
                if(xDelta>0)
                    xDelta += 1;
                Coordinate lower = Coordinate.GetLower(start, end);
                Console.WriteLine(lower.X +" "+lower.Y);
                int xStart = lower.X;
                int yStart = lower.Y;
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

        public List<Coordinate> ShipParts {get; set;}
    }
    public class Coordinate
    {   
 
        public Coordinate(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }
        public Coordinate()
        {

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
    public class ActionReport
    {
        public ActionReport(string content)
        {
            this.Content = content;
        }
        public ActionReport(bool hit)
        {
            if(hit)
            {
                this.Content = "You hit a target";
            }
            else
            {
                this.Content = "You miss";
            }
        }
        public ActionReport(int type)
        {
            if(type == 0)
            {
                this.Content = "You miss";
            }
            if(type == 1)
            {
                this.Content = "You hit a target";
            }
            if(type == 2)
            {
                this.Content = "You win";
            }

        }
        
        public string Content {get; private set;}
    }
}