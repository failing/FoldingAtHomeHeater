using System;
using Newtonsoft.Json;

namespace FoldingAtHomeHeater.Entites
{
    public class WeatherResponse
    {
        [JsonProperty("LocalObservationDateTime")]
        public DateTimeOffset LocalObservationDateTime { get; set; }

        [JsonProperty("EpochTime")]
        public long EpochTime { get; set; }

        [JsonProperty("WeatherText")]
        public string WeatherText { get; set; }

        [JsonProperty("WeatherIcon")]
        public long WeatherIcon { get; set; }

        [JsonProperty("HasPrecipitation")]
        public bool HasPrecipitation { get; set; }

        [JsonProperty("PrecipitationType")]
        public object PrecipitationType { get; set; }

        [JsonProperty("IsDayTime")]
        public bool IsDayTime { get; set; }

        [JsonProperty("Temperature")]
        public Temperature Temperature { get; set; }

        [JsonProperty("MobileLink")]
        public Uri MobileLink { get; set; }

        [JsonProperty("Link")]
        public Uri Link { get; set; }
    }

    public partial class Temperature
    {
        [JsonProperty("Metric")]
        public Imperial Metric { get; set; }

        [JsonProperty("Imperial")]
        public Imperial Imperial { get; set; }
    }

    public partial class Imperial
    {
        [JsonProperty("Value")]
        public double Value { get; set; }

        [JsonProperty("Unit")]
        public string Unit { get; set; }

        [JsonProperty("UnitType")]
        public long UnitType { get; set; }
    }
}

