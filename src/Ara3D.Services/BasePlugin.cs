using System;
using System.Collections.Generic;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services
{
    /// <summary>
    /// A plugin can subscribe to messages explicitly via the EventBus, or via the convenience methods declared below, or implicitly
    /// by implementing an "ISubscriber" interface. 
    /// </summary>
    public class BasePlugin : IPlugin
    {
        public IApi Api { get; private set; }

        public virtual string Name => GetType().Name;

        public virtual void Initialize(IApi api)
        {
            Api = api;
            Api.GetService<LoggingService>().Log($"Initializing plugin {Name}");
        }

        public event EventHandler Disposing;

        protected virtual void Dispose(bool disposing)
        {
            Api.GetService<LoggingService>().Log($"Disposing plugin {Name}");
            if (disposing)
            {
                Disposing?.Invoke(this, EventArgs.Empty);
            }
            Api = null;
            Disposing = null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        public void OnModelsChanged<T>(Action<IReadOnlyList<IModel<T>>> action)
            => Api.EventBus.OnModelsChanged(action, this);

        public void OnModelChanged<T>(Action<IModel<T>> action)
            => Api.EventBus.OnModelChanged(action, this);

        public void OnModelAdded<T>(Action<IModel<T>> action)
            => Api.EventBus.Subscribe<ModelChangedEvent<T>>(evt =>
            {
                if (evt.Args.ChangeType == RepositoryChangeType.ModelAdded)
                    action(evt.Model);
            }, this);
    }
}
