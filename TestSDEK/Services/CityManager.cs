using System.Text.Json;
using Base.Data;

namespace Services;

internal class CityManager
{
    private HashSet<City> _cities;

    public CityManager()
    {
        _cities = new();
    }

    public City[] GetCities(string cityName)
    {
        return Task.Run(async () => await GetCitiesAsync(cityName)).Result;
    }

    private async Task<City[]> GetCitiesAsync(string cityName)
    {
        var cities = _cities
            .Where(c => c.cityName.Equals(cityName, StringComparison.CurrentCultureIgnoreCase))
            .ToArray();

        if (cities.Length == 0)
        {
            await LoadAsyncCitiesFromSDEK(cityName);
            cities = _cities
                .Where(c => c.cityName.Equals(cityName, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();
        }

        return cities;
    }

    private async Task LoadAsyncCitiesFromSDEK(string cityName = "", string country = "RU", int count = 5)
    {
        using var client = new HttpClient();
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"http://integration.cdek.ru/v1/location/cities/json?size={count}&cityName={cityName}&countryCode={country}"),
            Method = HttpMethod.Get
        };

        var task = await client
            .SendAsync(request)
            .ContinueWith(async msg =>
            {
                var response = msg.Result;
                if (response.StatusCode != System.Net.HttpStatusCode.OK) return;

                var json = await response.Content.ReadAsStringAsync();
                _cities = JsonSerializer.Deserialize<HashSet<City>>(json);
            });
    }
}