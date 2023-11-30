using System.Collections.Generic;
using Ara3D.Domo;
using Ara3D.Utils;

namespace Ara3D.Services
{
    public class ServiceRegisteredEvent : IEvent
    {
        public IService Service { get; }
        public ServiceRegisteredEvent(IService service)
            => Service = service;
    }
    
    public sealed class Api : IApi
    {
        private readonly List<IService> _services = new List<IService>();
        private readonly List<IRepository> _repositories = new List<IRepository>();

        public IEnumerable<IService> GetServices() => _services;
        public IEnumerable<IRepository> GetRepositories() => _repositories;
        
        public IEventBus EventBus { get; } 
        public Synchronizer Synchronizer { get; } 

        public Api()
        {
            Synchronizer = Synchronizer.Create();
            EventBus = new EventBus(Synchronizer);
        }

        public void AddService(IService service) 
        {
            _services.Add(service);
            EventBus.Publish(new ServiceRegisteredEvent(service));
        }

        public void AddRepository(IRepository repository) 
        {
            _repositories.Add(repository);
            EventBus.AddRepositoryAsPublisher(repository);
            EventBus.Publish(new RepositoryChangedEvent(new RepositoryChangeArgs()
            {
                Repository = repository,
                ChangeType = RepositoryChangeType.RepositoryAdded,
            }));
        }
    }
}
