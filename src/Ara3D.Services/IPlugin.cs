namespace Ara3D.Services
{
    public interface IPlugin 
    {
        string Name { get; }
        void Initialize(IApi api);
        IApi Api { get; }
    }
}
