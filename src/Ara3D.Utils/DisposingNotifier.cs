using System;

namespace Ara3D.Utils
{
    public interface IDisposingNotifier : IDisposable
    {
        event EventHandler Disposing;
    }

    /// <summary>
    /// A convenient class for those implementing DisposingNotifier.
    /// A parent class may be supplied, in which case this class will dispose itself
    /// if the parent is disposed. 
    /// </summary>
    public class DisposingNotifier : IDisposingNotifier
    {
        public event EventHandler Disposing;
        public IDisposingNotifier Parent { get; private set; }

        public void Dispose()
        {
            if (Parent != null)
            {
                Parent.Disposing -= ParentOnDisposing;
            }

            Parent = null;
            Disposing?.Invoke(this, EventArgs.Empty);
            Disposing = null;
        }

        public DisposingNotifier(IDisposingNotifier parent = null)
        {
            Parent = parent;
            if (Parent != null)
                Parent.Disposing += ParentOnDisposing;
        }

        private void ParentOnDisposing(object sender, EventArgs e)
            => Dispose();
    }
}