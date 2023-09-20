using Base.Data;

namespace Base.States;

internal class GetValuesState : StateBase
{
    private float _weight;
    private float _width;
    private float _height;
    private float _length;
    private UserData _userData;

    public GetValuesState(StateMachine stateMachine, UserData userData) : base(stateMachine)
    {
        _userData = userData;
    }

    public override void Enter()
    {
        do
        {
            Reset();

            _weight = GetValue<float>("Введите вес (кг)\nПример: 0,5");
            ClearAndPrintValues();

            _width = GetValue<float>("Введите ширину (см)\nПример: 55");
            ClearAndPrintValues();

            _height = GetValue<float>("Введите высоту (см)\nПример: 35,5");
            ClearAndPrintValues();

            _length = GetValue<float>("Введите длину (см)\nПример: 77");
            ClearAndPrintValues();

        } while (CheckForNextStep());

        _userData.weight = _weight;
        _userData.width = _width;
        _userData.height = _height;
        _userData.length = _length;

        _stateMachine.Enter<SelectCitiesState>();
    }

    public override void Exit()
    {
        Reset();
    }

    private void ClearAndPrintValues()
    {
        Console.Clear();
        Console.WriteLine("Введенные характеристики:");
        Console.Write($"{_width}x{_height}x{_length} вес {_weight}кг\n");
    }

    private void Reset()
    {
        _weight = default;
        _width = default;
        _height = default;
        _length = default;
    }
}
