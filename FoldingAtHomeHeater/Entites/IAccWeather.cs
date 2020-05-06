using System;
using System.Threading.Tasks;
using RestEase;

namespace FoldingAtHomeHeater.Entites
{
    public interface IAccWeather
    {
        [Get("{weatherLocationId}")]
        Task<WeatherResponse[]> GetWeatherConditions([Path] long weatherLocationId, [Query] string apikey);
    }
}
