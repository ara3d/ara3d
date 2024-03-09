using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Utils;

namespace Ara3D.Geometry.Tests
{
    public static class GeometryTests
    {
        [SetUp]
        public static void SetUp()
        {
            if (!MoldFlowDirectoryPath.Exists())
                throw new Exception($"Could not find data directory {MoldFlowDirectoryPath}");
        }

        public static DirectoryPath MoldFlowDirectoryPath =
            SourceCodeLocation.GetFolder()
                .RelativeFolder("..", "..", "..", "3d-format-shootout", "data", "files", "moldflow");
    }
}