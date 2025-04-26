namespace Ara3D.Services.Experimental
{
    public interface IPlugin 
    {
        string Name { get; }
        void Initialize(IServiceManager api);
        IServiceManager Api { get; }
    }
}
