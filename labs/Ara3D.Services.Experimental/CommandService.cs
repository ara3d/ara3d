using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services.Experimental
{
    /// <summary>
    /// Used on service or plugin functions to identify them as commands.
    /// These functions, bound to the plug-in or service, are used as commands.
    /// An additional function or property name can be identified, that can be queried
    /// to determine if the command is available. A repository type can be be specified
    /// which is monitored for changes. When that repository changes, the "can execute"
    /// status of the command is requeried by the system. 
    /// </summary>
    public class CommandAttribute : Attribute
    {
        public string CommandName { get; }
        public string CanExecuteFunctionName { get; }
        public Type RepositoryType { get; }

        public CommandAttribute(string commandName, string canExecuteFunctionName = null, Type repositoryType = null)
        {
            CommandName = commandName;
            CanExecuteFunctionName = canExecuteFunctionName;
            RepositoryType = repositoryType;
        }
    }

    public struct CommandRecord
    {
        public string Name { get; }

        public CommandRecord(string name)
            => Name = name;
    }


    public class CommandRepo : AggregateRepository<CommandRecord>
    { }


    public class CommandService : BaseService
    {
        public List<INamedCommand> Commands = new List<INamedCommand>();
        public CommandRepo CommandRepo { get; }

        public CommandService(IApplication app, CommandRepo repo) : base(app)
        {
            CommandRepo = repo;
        }

        public INamedCommand GetCommand(string name)
            => Commands.FirstOrDefault(cmd => cmd.Name == name);

        public INamedCommand AddCommand(string name, Action action, Func<bool> canExecute = null,
            IRepository repo = null)
        {
            var r = new NamedCommand(name, action, canExecute);
            if (repo != null)
                repo.RepositoryChanged += (_sender, _args) => r.NotifyCanExecuteChanged();
            CommandRepo.Add(new CommandRecord(name));
            Commands.Add(r);
            return r;
        }

        /// <summary>
        /// Automatically create commands by looking at the attributes of an object .
        /// </summary>
        public void AddAttributeCommands(object service)
        {
            var t = service.GetType();
            foreach (var m in t.GetMethods())
            {
                var commandAttributes =
                    m.GetCustomAttributes(typeof(CommandAttribute)).OfType<CommandAttribute>().ToList();
                if (commandAttributes.Count > 0)
                {
                    var commandAttribute = commandAttributes[0];
                    var commandFunc = m.InvokableMethod(service);

                    Func<bool> canExecuteFunction = () => true;
                    if (commandAttribute.CanExecuteFunctionName != null)
                    {
                        var canExecuteMethod = t.GetMethod(commandAttribute.CanExecuteFunctionName);

                        if (canExecuteMethod == null)
                        {
                            var prop = t.GetProperty(commandAttribute.CanExecuteFunctionName);
                            if (prop == null)
                                throw new Exception(
                                    $"Could not find CanExecute method or property {commandAttribute.CanExecuteFunctionName}");
                            if (prop.PropertyType != typeof(bool))
                                throw new Exception(
                                    $"Return type of CanExecute property is {prop.PropertyType} not bool");
                            canExecuteFunction = prop.InvokableProperty<bool>(service);
                        }
                        else
                        {
                            if (canExecuteMethod.ReturnType != typeof(bool))
                                throw new Exception(
                                    $"Return type of CanExecute method is {canExecuteMethod.ReturnType} not bool");
                            if (canExecuteMethod.GetParameters().Length != 0)
                                throw new Exception($"CanExecute method should expect no parameters");
                            canExecuteFunction = canExecuteMethod.InvokableMethod<bool>(service);
                        }
                    }

                    var cmd = AddCommand(commandAttribute.CommandName, commandFunc, canExecuteFunction);

                    if (commandAttribute.RepositoryType != null)
                    {
                        var repo = App.GetRepositories().FirstOrDefault(r => r.GetType() == commandAttribute.RepositoryType);
                        if (repo != null)
                            repo.RepositoryChanged += (sender, args) 
                                => cmd.NotifyCanExecuteChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Automatically create commands by looking at the attributes of services or plugins.
        /// </summary>
        public void AddAttributeCommands(IEnumerable<object> services)
        {
            foreach (var service in services)
            {
                AddAttributeCommands(service);
            }
        }
    }
}