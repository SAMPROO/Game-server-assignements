using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace dotnetKole
{
    class Program
    {
        static void Main(string[] args)
        {
            string searchTerm = args[1];
            if(args[0]=="realtime")
            {
                RealTimeCityBikeDataFetcher rtcbdf = new RealTimeCityBikeDataFetcher();
                var result = rtcbdf.GetBikeCountInStation(searchTerm);
                if(result.Result == -1)
                    throw new NotFoundException("Not found");
                else
                    Console.WriteLine(result.Result);
            }
            else if(args[0]=="offline")
            {
                string path = "OfflineBikeData.txt";
                List<Stations> stations = new List<Stations>();
                if(File.Exists(path))
                {
                    using(StreamReader sr = new StreamReader(path))
                    {
                        while(sr.Peek() >= 0)
                        {
                            string[] temp = sr.ReadLine().Split(" : ");
                            Stations newStation = new Stations();
                            newStation.name = temp[0];
                            int bikes = int.Parse(temp[1]);
                            newStation.bikesAvaiLable = bikes;
                            stations.Add(newStation);
                        }
                    }
                }
                foreach(var station in stations)
                {
                    if(station.name == searchTerm)
                    {
                        Console.WriteLine(station.bikesAvaiLable);
                    }
                }
            }
            
        }
    }

    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }

    class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        public async Task<int> GetBikeCountInStation(string name)
        {
            
            for (int i = 0; i < name.Length; i++)
            {
                if(Char.IsNumber(name[i]))
                {
                    throw new ArgumentException("Invalid argument");
                }
            }

            Task<string> data = GetData();
            string s = data.Result;
            
            BikeRentalStationList list = Newtonsoft.Json.JsonConvert.DeserializeObject<BikeRentalStationList>(s);
            foreach (var item in list.stations)
            {
                if(item.name == name)
                return item.bikesAvaiLable;
            }
            return -1;
        }

        public async Task<string> GetData()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri("http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental");
            string result = await client.GetStringAsync(uri);
            return result;
        }
        
    }
    class BikeRentalStationList
    {
        [Newtonsoft.Json.JsonProperty("stations")]
        public Stations[] stations;
    }
    class Stations
    {
        [Newtonsoft.Json.JsonProperty("name")]
        public string name;
        [Newtonsoft.Json.JsonProperty("bikesAvailable")]
        public int bikesAvaiLable;
    }
    
    public class NotFoundException : System.Exception
    {
        public NotFoundException() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected NotFoundException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
