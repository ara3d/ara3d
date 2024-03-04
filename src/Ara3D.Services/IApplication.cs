using Ara3D.Domo;
using System.Collections.Generic;

namespace Ara3D.Services
{
    /// <summary>
    /// This is the interface that bridges services, repositories,
    /// and events.
    /// In effect this is the infrastructure of an application.
    /// For simplicity, services and repositories can be added,
    /// but never removed.
    /// If you need that, just recreate the application. 
    /// </summary>
    public interface IApplication : IRepositoryManager
    {
        IReadOnlyList<IService> GetServices();
        void AddService(IService service);
        void AddRepository(IRepository repository);
        IEventBus EventBus { get; } 
    }
}
