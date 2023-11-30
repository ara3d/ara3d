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
    }
}
