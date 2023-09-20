using Base.Data;
using Base.States;
using Services;

namespace Base;

internal class Bootstrap
{
    private CityManager _cityManager;
    private DeliveryService _deliveryService;
    private UserData _userData;
    private StateMachine _stateMachine;

    public Bootstrap()
    {
        _userData = new();
        _cityManager = new CityManager();
        _deliveryService = new DeliveryService(_cityManager);
        _stateMachine = new StateMachine(_userData, _cityManager, _deliveryService);
    }

    public void StartProgram()
    {
        Console.WriteLine("ДОБРО ПОЖАЛОВАТЬ В ПРОГРАММУ РАСЧЕТА ЦЕН ДОСТАВКИ");

        _stateMachine.Enter<GetValuesState>();
    }
}