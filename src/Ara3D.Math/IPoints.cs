namespace Ara3D.Mathematics
{
    public interface IPoints
    {
        int NumPoints { get; }
        Vector3 GetPoint(int n);
    }

    public interface IPoints2D
    {
        int NumPoints { get; }
        Vector2 GetPoint(int n);
    }
}
