using System.Collections.Generic;
using Ara3D.Domo;

namespace Ara3D.Services
{
    /// <summary>
    /// This is the interface that bridges services, repositories, and events. 
    /// </summary>
    public interface IApi 
    {
        IEnumerable<IService> GetServices();
        IEnumerable<IRepository> GetRepositories();
        void AddService(IService service);
        void AddRepository(IRepository repository);
        IEventBus EventBus { get; } 
    }
}
