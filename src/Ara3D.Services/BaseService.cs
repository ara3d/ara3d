using System;

namespace Ara3D.Services
{
    public class BaseService : IService
    {
        protected BaseService(IApi api)
        {
            Api = api;
            api.AddService(this);
        }

        public IApi Api { get; }

        public void Dispose()
        {
            Disposing?.Invoke(this, EventArgs.Empty);
            Disposing = null;
        }

        public event EventHandler Disposing;
    }
}