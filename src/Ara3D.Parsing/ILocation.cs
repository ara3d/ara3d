namespace Parakeet
{
    /// <summary>
    /// An abstract notion of location. This could be specific parser location, a range within some parser input,
    /// but it can also be the result of some generated code, or refactoring, and express a relationship to
    /// something else. 
    /// </summary>
    public interface ILocation
    { }

    public class Location : ILocation
    {
    }

    public static class LocationExtensions
    {
    }
}