namespace Ara3D.Geometry
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Prism_(geometry)
    /// https://mathworld.wolfram.com/Prism.html
    /// </summary>
    public static class Prisms
    {
        public static GridMesh TriangularPrism = Polygons.Triangle.Extrude();
        public static GridMesh SquarePrism = Polygons.Square.Extrude();
        public static GridMesh PentagonalPrism = Polygons.Pentagon.Extrude();
        public static GridMesh HexagonalPrism = Polygons.Hexagon.Extrude();
        public static GridMesh OctagonalPrism = Polygons.Octagon.Extrude();
        public static GridMesh DecagonalPrism = Polygons.Decagon.Extrude();
        public static GridMesh PentagramalPrism = Polygons.Pentagram.Extrude();
        public static GridMesh HexagramalPrism = Polygons.Hexagram.Extrude();
        public static GridMesh OctagramalPrism = Polygons.Octagram.Extrude();
    }
}