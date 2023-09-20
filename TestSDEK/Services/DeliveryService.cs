using System.Text;
using System.Text.Json;
using Base.Data;

namespace Services;

internal class DeliveryService
{
    private const int KG = 1000;
    private const int MM = 100;
    private readonly CityManager _cityManager;

    public DeliveryService(CityManager cityManager)
    {
        _cityManager = cityManager;
    }

    // /// <summary>
    // /// Получение стоимости доставки
    // /// </summary>
    // /// <param name="weight">Вес в граммах</param>
    // /// <param name="width">Ширина в мм</param>
    // /// <param name="height">Высота в мм</param>
    // /// <param name="length">Длина в мм</param>
    // /// <param name="fiasCodeSender">ФИАС код города отправления</param>
    // /// <param name="fiasCodeReceiver">ФИАС код города получения</param>
    // public Answer CalculatePrice(int weight, int width, int height, int length,
    //                             string? fiasCodeSender, string? fiasCodeReceiver)
    // {
    //     var citySender = _cityManager.GetCities(fiasCodeSender)[0];
    //     var cityReceiver = _cityManager.GetCities(fiasCodeReceiver)[0];

    //     return CalculatePrice(weight, width, height, length, citySender, cityReceiver);
    // }

    /// <summary>
    /// Получение стоимости доставки
    /// </summary>
    /// <param name="weight">Вес в граммах</param>
    /// <param name="width">Ширина в мм</param>
    /// <param name="height">Высота в мм</param>
    /// <param name="length">Длина в мм</param>
    /// <param name="cityFrom">Город отправления</param>
    /// <param name="cityTo">Город получения</param>
    public Answer CalculatePrice(int weight, int width, int height, int length, City cityFrom, City cityTo)
    {
        weight /= KG;
        width /= MM;
        height /= MM;
        length /= MM;
        return Task.Run(async () => await CalculatePriceAsync(
                                weight, width, height, length, cityFrom.cityCode, cityTo.cityCode)).Result;
    }

    private async Task<Answer> CalculatePriceAsync(int weight, int width, int height, int length,
                                                    string? cityCodeSender, string? cityCodeReceiver)
    {
        Answer? answer = null;

        using var client = new HttpClient();
        var jsonRequest = "{\"version\":\"1.0\", \"senderCityId\":\"" + 
                cityCodeSender + "\", \"receiverCityId\":\"" +
                cityCodeReceiver + "\"," +
                "\"tariffId\":\"480\", \"goods\": [{\"weight\":\"" +
                weight + "\", \"length\":\"" +
                length + "\", \"width\":\"" +
                width + "\", \"height\":\"" + 
                height + "\" }]}";

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri("http://api.cdek.ru/calculator/calculate_tarifflist.php"),
            Method = HttpMethod.Post,
            Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json")
        };

        var task = await client.SendAsync(request)
            .ContinueWith(async msg =>
            {
                var response = msg.Result;
                var json = await response.Content.ReadAsStringAsync();
                answer = JsonSerializer.Deserialize<Answer>(json);
            });

        return answer ?? new Answer(new Result());
    }
}
