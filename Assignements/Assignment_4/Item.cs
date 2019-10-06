using System;

namespace dotnetKole
{
    public class Item
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public ItemType ItemType { get; set; }
    }
}
