namespace Ara3D.Mathematics
{
    public interface IAlgebraicSet<T>
    {
        IAlgebraicSet<T> Union(IAlgebraicSet<T> other);
        IAlgebraicSet<T> Complement { get; }
        IAlgebraicSet<T> Intersection(IAlgebraicSet<T> other);
    }
}
