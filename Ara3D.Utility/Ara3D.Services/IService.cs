using Ara3D.Utils;

namespace Ara3D.Services
{
    public interface IService : IDisposingNotifier
    {
        IApi Api { get; }
    }
}