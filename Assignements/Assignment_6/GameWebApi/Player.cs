using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace dotnetKole
{
    public class Player
    {

        public Player()
        {
            Items = new List<Item>();
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set;}
        [JsonProperty("Tag")]
        public Tag Tag { get; set; }
        [JsonProperty("Score")]
        public int Score{get; set;}
        [JsonProperty("Level")]
        public int Level {get;set;}
        [JsonProperty("IsBanned")]
        public bool IsBanned{set;get;}
        [JsonProperty("CreationTime")]
        public DateTime CreationTime{get;set;}
        [JsonProperty("Items")]
        public List<Item> Items { get; set; }
        

    }
}