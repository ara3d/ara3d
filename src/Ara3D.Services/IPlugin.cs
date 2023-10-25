using Ara3D.Utils;

namespace Ara3D.Services
{
    public interface IPlugin : IDisposingNotifier
    {
        string Name { get; }
        void Initialize(IApi api);
        IApi Api { get; }
    }
}
