using Base.Data;
using Base.States;
using Services;

internal class StateMachine
{
    Dictionary<Type, StateBase> _states;
    private StateBase? _activeState;

    public StateMachine(UserData userData, CityManager cityManager, DeliveryService deliveryService)
    {
        _states = new Dictionary<Type, StateBase>
        {
            [typeof(GetValuesState)] = new GetValuesState(this, userData),
            [typeof(SelectCitiesState)] = new SelectCitiesState(this, userData, cityManager),
            [typeof(CalcDeliveryState)] = new CalcDeliveryState(this, userData, deliveryService, cityManager)
        };
    }

    public void Enter<T>() where T : StateBase
    {
        _activeState?.Exit();
        _activeState = GetState<T>();
        _activeState?.Enter();
    }


    private T? GetState<T>() where T : StateBase
    {
        var state = typeof(T);

        if (_states.ContainsKey(state))
            return _states[state] as T;

        throw new KeyNotFoundException(state.Name + " not found");
    }
}