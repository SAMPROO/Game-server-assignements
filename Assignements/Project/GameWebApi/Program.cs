using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShipGame;
namespace GameWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Ship ship = new Ship(new Coordinate(0,3),new Coordinate(0,0));

            Console.WriteLine("SHIP");
            var parts = ship.GetShipParts();
            foreach(var part in parts)
            {
                Console.WriteLine(part.X+" "+part.Y);
            }
            CreateHostBuilder(args).Build().Run();

            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}
