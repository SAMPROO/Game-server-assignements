using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Extensions;

namespace dotnetKole
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1
            var players = CreatePlayers(10);
            foreach (var player in players)
            {
                Console.WriteLine(player.Id);
            }

            Player p = players[0];
            Player ip = new Player();

            // 2
            Console.WriteLine(p.GetHighestLevelItem().Level);
            
            // 3
            Console.WriteLine(GetItems(p).Length);
            Console.WriteLine(GetItemsWithLinq(p).Length);
            
            // 4
            if (FirstItem(ip) == null)
            {
                Console.WriteLine("EROROR ERROR");
            }
            else
            {
                Console.WriteLine(FirstItem(ip));
            }
            
            if (FirstItemWithLinq(ip) == null)
            {
                Console.WriteLine("EROROR WITH ERROR");
            }
            else
            {
                Console.WriteLine(FirstItemWithLinq(ip));
            }
            
            // 5
            ProcessEachItem(p, PrintItem);
            
            // 6
            //p = PrintItem => ProcessEachItem;
            ProcessEachItem = PrintItem => p;
            

        }
        
        public static List<Player> CreatePlayers(int amount)
        {   
            var players = new List<Player>();
            var usedGuids = new List<Guid>();
            
            for (int i = 0; i < amount; i++)
            {
                Player player = new Player();

                var items = new List<Item>();

                for (int j = 0; j < 5; j++)
                {
                    Item item = new Item();
                    Random rnd = new Random();
                    Guid itemGuid = Guid.NewGuid();
                    item.Level = rnd.Next(0, 100);
                    item.Id = itemGuid;
                    
                    items.Add(item);
                }

                player.Items = items;
                Guid newGuid = Guid.NewGuid();

                foreach (var guid in usedGuids)
                {
                    if (newGuid == guid)
                    {
                        newGuid = Guid.NewGuid();
                    }
                }
                
                usedGuids.Add(newGuid);
                player.Id = newGuid;

                players.Add(player);
                
            }

            return players;
        }

        public static Item[] GetItems(Player player)
        {
            var items = new Item[player.Items.Count];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = player.Items[i];
            }

            return items;
        }
        
        public static Item[] GetItemsWithLinq(Player player)
        {
            var items = player.Items.ToArray();

            return items;
        }
        
        public static Item FirstItem(Player player)
        {
            if (player.Items == null || player.Items.Count == 0)
                return null;
            else
                return player.Items[0];
        }
        
        public static Item FirstItemWithLinq(Player player)
        {
            if (player.Items == null || player.Items.Count == 0)
                return null;
            else
                return player.Items.FirstOrDefault();
        }

        public static void ProcessEachItem(Player player, Action<Item> process)
        {
            foreach (var item in player.Items)
            {
                process(item);
            }
        }

        public static void PrintItem(Item item)
        {
            Console.WriteLine("-------------------");
            Console.WriteLine("Item ID: " + item.Id);
            Console.WriteLine("Item Level: " + item.Level);
        }
    }
}