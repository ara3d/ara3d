namespace Ara3D.Services.Experimental
{
    public interface IPlugin 
    {
        string Name { get; }
        void Initialize(IApplication api);
        IApplication Api { get; }
    }
}
