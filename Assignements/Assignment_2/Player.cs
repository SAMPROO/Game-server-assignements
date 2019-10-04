using System;
using System.Collections.Generic;

namespace dotnetKole
{
    public class Player : IPlayer
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public List<Item> Items { get; set; }
    }
}
