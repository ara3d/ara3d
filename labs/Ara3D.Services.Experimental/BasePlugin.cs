namespace Ara3D.Services.Experimental
{
    /// <summary>
    /// A plugin can subscribe to messages explicitly via the EventBus, or via the convenience methods declared below, or implicitly
    /// by implementing an "ISubscriber" interface. 
    /// </summary>
    public class BasePlugin : IPlugin
    {
        public IApplication Api { get; private set; }

        public virtual string Name => GetType().Name;

        public virtual void Initialize(IApplication api)
        {
            Api = api;
            Api.GetService<LoggingService>().Log($"Initializing plugin {Name}");
        }
    }
}
