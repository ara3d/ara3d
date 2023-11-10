using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public interface ISingletonModelBackedService<T> : IService, INotifyPropertyChanged
    {
        ISingletonRepository<T> Repository { get; }
        IModel<T> Model { get; }
    }

    public interface IAggregateModelBackedService<T> : IService, INotifyCollectionChanged
    {
        IAggregateRepository<T> Repository { get; }
        IReadOnlyList<IModel<T>> Models { get; }
    }

    // TODO: this used to be in IService
    public interface ICommandable
    {
        IReadOnlyList<INamedCommand> GetCommands();
    }

    public class DomoService : BaseService
    {
        public DomoService(IApi api)
            : base(api)
        { }

        public Dictionary<string, INamedCommand> Commands = new Dictionary<string, INamedCommand>();

        public INamedCommand RegisterCommand(Delegate execute, Delegate canExecute, IRepository repository)
        {
            var r = new NamedCommand(execute, canExecute);
            Commands.Add(r.Name, r);

            if (repository != null)
                repository.RepositoryChanged += (_1, _2) => r.NotifyCanExecuteChanged();

            return r;
        }

        public INamedCommand GetCommand(string name)
            => Commands[name];

        public IReadOnlyList<INamedCommand> GetCommands()
            => Commands.Values.ToList();
    }

    public class SingletonModelBackedService<TModel> : DomoService, ISingletonModelBackedService<TModel>
        where TModel: new()
    {
        public SingletonModelBackedService(IApi api)
            : base(api)
        {
            Repository = new SingletonRepository<TModel>();
            Repository.RepositoryChanged += OnRepositoryChanged;
        }

        protected virtual void OnRepositoryChanged(object sender, RepositoryChangeArgs e)
        { }

        public event PropertyChangedEventHandler PropertyChanged
        {
            add => Model.PropertyChanged += value;
            remove => Model.PropertyChanged -= value;
        }

        public ISingletonRepository<TModel> Repository { get; }
        public IModel<TModel> Model => Repository.Model;

        public TModel Value
        {
            get => Model.Value;
            set => Model.Value = value;
        }
    }

    public class AggregateModelBackedService<TModel> : DomoService, IAggregateModelBackedService<TModel>
        where TModel : new()
    {
        public AggregateModelBackedService(IApi api)
            : base(api)
        {
            Repository = new AggregateRepository<TModel>();
            Repository.RepositoryChanged += OnRepositoryChanged;
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => Repository.CollectionChanged += value;
            remove => Repository.CollectionChanged -= value;
        }

        protected virtual void OnRepositoryChanged(object sender, RepositoryChangeArgs e)
        { }

        public IAggregateRepository<TModel> Repository { get; }
        public IReadOnlyList<IModel<TModel>> Models => Repository.GetModels();
    }

}