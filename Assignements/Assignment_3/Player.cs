using System;
using Newtonsoft.Json;

namespace dotnetKole
{
    public class Player
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set;}
        [JsonProperty("Score")]
        public int Score{get; set;}
        [JsonProperty("Level")]
        public int Level {get;set;}
        [JsonProperty("IsBanned")]
        public bool IsBanned{set;get;}
        [JsonProperty("CreationTime")]
        public DateTime CreationTime{get;set;}

    }
}