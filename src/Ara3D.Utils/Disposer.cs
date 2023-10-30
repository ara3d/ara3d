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

        public static Disposer Create(Action action)
            => new Disposer(action);

        public static Disposer Create(Action beforeAction, Action afterAction)
        {
            beforeAction();
            return new Disposer(afterAction);
        }
    }
}
