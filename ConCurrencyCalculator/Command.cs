namespace ConCurrencyCalculator;

public sealed class Command : ICommand
{
    private readonly Action<object?> _execute;

    private readonly Func<object?, bool> _canExecute;

    public event EventHandler? CanExecuteChanged;

    public Command(Action<object?> execute, Func<object?, bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public Command(Action<object?> execute) : this(execute, _ => true)
    {
    }

    public bool CanExecute(object? parameter) => _canExecute(parameter);

    public void Execute(object? parameter) => _execute(parameter);

    public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
