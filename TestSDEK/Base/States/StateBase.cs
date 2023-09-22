namespace Base.States;

internal abstract class StateBase
{
    protected readonly StateMachine _stateMachine;

    public StateBase(StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Exit();

    protected bool TryConvertValue<T>(string value, out T? result)
    {
        try
        {
            result = (T)Convert.ChangeType(value.Replace('.', ','), typeof(T));
            return true;
        }
        catch (Exception)
        {
            result = default;
            return false;
        }
    }

    protected T? GetValue<T>(string text)
    {
        bool isSuccess;
        string? value;
        T? result;

        Console.WriteLine(text);

        do
        {
            value = Console.ReadLine();
            value ??= string.Empty;

            isSuccess = TryConvertValue(value, out result);
            if (!isSuccess) Console.WriteLine("Ой, что-то пошло не так. Попробуйте ещё раз");

        } while (!isSuccess);

        return result;
    }

    protected bool CheckForNextStep()
    {
        Console.WriteLine("Заменить данные?");
        Console.WriteLine("Да(Д)/Нет(Н)");
        
        char[] chars = {'д', 'н'};
        char key;

        do
        {
            key = char.ToLower(Console.ReadKey().KeyChar);
        } while (!chars.Any(s => s == key));

        Console.WriteLine();
        return key == 'д';
    }
}
