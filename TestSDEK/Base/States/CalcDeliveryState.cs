using System.Security.Cryptography;
using Base.Data;
using Services;

namespace Base.States;

internal class CalcDeliveryState : StateBase
{
    private const int KG = 1000;
    private const int MM = 100;
    private readonly UserData _userData;
    private readonly DeliveryService _deliveryService;
    private readonly CityManager _cityManager;

    public CalcDeliveryState(StateMachine stateMachine, UserData userData,
                                DeliveryService deliveryService, CityManager cityManager) : base(stateMachine)
    {
        _userData = userData;
        _deliveryService = deliveryService;
        _cityManager = cityManager;
    }

    public override void Enter()
    {
        int weight = (int)(_userData.weight * KG);
        int width = (int)(_userData.width * MM);
        int height = (int)(_userData.height * MM);
        int length = (int)(_userData.length * MM);

        var answer = _deliveryService.CalculatePrice(weight, width, height, length, _userData.cityFrom, _userData.cityTo);

        Console.Clear();
        Console.WriteLine($"По параметрам {_userData.width}x{_userData.height}x{_userData.length} {_userData.weight}кг");
        Console.WriteLine($"Откуда: {_userData.cityFrom.cityName}, куда: {_userData.cityTo.cityName}");

        if (answer.result.errors.text is not null)
            Console.WriteLine(answer.result.errors.text);
        else
            Console.WriteLine($"Цена составила: {answer.result.price}р.");

        if (CheckForNextStep())
            _stateMachine.Enter<GetValuesState>();
    }

    public override void Exit()
    {
    }
}
