using System.Diagnostics.Contracts;
using Base.Data;
using Services;

namespace Base.States;
internal class SelectCitiesState : StateBase
{
    private readonly UserData _userData;
    private readonly CityManager _cityManager;

    public SelectCitiesState(StateMachine stateMachine, UserData userData, CityManager cityManager) : base(stateMachine)
    {
        _userData = userData;
        _cityManager = cityManager;
    }

    public override void Enter()
    {
        City from, to;
        do
        {
            from = SelectCity("Выберите город отправки");
            to = SelectCity("Выберите город доставки");
            
            Console.WriteLine($"Откуда: {from.cityName}, {from.country}, {from .region}");
            Console.WriteLine($"Куда: {to.cityName}, {to.country}, {to .region}");
        } while (CheckForNextStep());

        _userData.cityFrom = from;
        _userData.cityTo = to;

        _stateMachine.Enter<CalcDeliveryState>();
    }

    public override void Exit()
    {
    }

    private City SelectCity(string text)
    {
        do
        {
            string? cityName;
            cityName = GetValue<string>(text);
            var cities = GetCities(cityName);

            if (cities is null || cities.Length == 0)
            {
                Console.WriteLine("Ой, такого города не найдено. Попробуйте ввести ещё раз.");
                continue;
            }

            if (cities.Length == 1) return cities[0];

            Console.WriteLine("Нашлось больше 1 города\nВыберите из списка:");
            return GetConcreteCity(cities);
        } while (true);
    }

    private City[] GetCities(string? cityName) =>
        _cityManager.GetCities(cityName ?? string.Empty);

    private City GetConcreteCity(City[] cities)
    {
        for (int i = 0; i < cities.Length; i++)
        {
            var city = cities[i];
            Console.WriteLine($"{i + 1}. {city.cityName}, {city.country}, {city .region}");
        }

        int index;
        while (true)
        {
            if (!TryConvertValue(Console.ReadLine() ?? string.Empty, out index))
                continue;

            index--;
            if (index >= 0 || index < cities.Length)
                break;
        }

        return cities[index];
    }
}