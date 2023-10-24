using System;

namespace Ara3D.Utils
{
    public sealed class Disposer : IDisposable
    {
        readonly Action OnDispose;

        public Disposer(Action onDispose)
            => OnDispose = onDispose;

        public void Dispose()
            => OnDispose();
    }
}
