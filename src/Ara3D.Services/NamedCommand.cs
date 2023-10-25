namespace Ara3D.Services
{
    /*
    public class NamedCommand : INamedCommand
    {
        public NamedCommand(Delegate execute, Delegate canExecute = null, IRepository repository = null)
            : this(execute.Method?.Name ?? "", execute, canExecute, repository)
        { }

        public NamedCommand(string name, Delegate execute, Delegate canExecute = null, IRepository repository = null)
        {
            Name = name;
            ExecuteDelegate = execute;
            CanExecuteDelegate = canExecute;
            if (repository != null)
            {
                repository.RepositoryChanged +=
                    (_1, _2) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter = null)
            => CanExecuteDelegate == null ? true : (bool?)CanExecuteDelegate.DynamicInvoke(parameter) != false;

        public void Execute(object parameter = null)
        {
            var nParams = ExecuteDelegate.Method.GetParameters().Length;
            if (nParams == 0)
                ExecuteDelegate.DynamicInvoke();
            else if (nParams == 1)
                ExecuteDelegate.DynamicInvoke(parameter);
            else
                throw new Exception($"Invalid number of arguments received 1, expected {nParams}");
        }

        public Delegate CanExecuteDelegate { get; }
        public Delegate ExecuteDelegate { get; }
        public event EventHandler CanExecuteChanged;
        public string Name { get; }
    }
    */
}