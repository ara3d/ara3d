using System;

namespace Ara3D.Services
{
    public class BaseService : IService
    {
        public BaseService(IApi api)
            => Api = api;

        public IApi Api { get; }
        public IEventBus EventBus => Api.EventBus;

        public void Dispose()
        {
            Disposing?.Invoke(this, EventArgs.Empty);
            Disposing = null;
        }

        public event EventHandler Disposing;
    }
}