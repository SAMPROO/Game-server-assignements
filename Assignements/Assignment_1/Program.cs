using System;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json; 
using Newtonsoft.Json.Serialization;

namespace Assignment_1
{
    class Program
    {
        static void Main(string[] args)
        {
            RealTimeCityBikeDataFetcher fetcher = new RealTimeCityBikeDataFetcher();
            var result = fetcher.GetBikeCountInStation(args[0]);
            Console.WriteLine(result);
        }
    }

    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string station_name);
    }

    class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        string api = "http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental";
        HttpClient http_client = new HttpClient();

        public async Task<int> GetBikeCountInStation(string station_name)
        {
            var data = await http_client.GetStringAsync(api);
            var convert_from_json = JsonConvert.DeserializeObject<BikeRentalStationList>(data);

            Console.WriteLine(convert_from_json);
            
            return 0;
        }
    }

    class BikeRentalStationList
    {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("bikesAvailable")]
        public string BikesAvailable { get; }
    }
}
