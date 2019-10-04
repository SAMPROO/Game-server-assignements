using System;

namespace dotnetKole
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set;}
        public int Score{get; set;}
        public int Level {get;set;}
        public bool IsBanned{set;get;}
        public DateTime CreationTime{get;set;}

    }
}