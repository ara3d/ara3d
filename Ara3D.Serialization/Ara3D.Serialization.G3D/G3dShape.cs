﻿using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Serialization.G3D
{
    public class G3dShape
    {
        public readonly G3D G3D;
        public readonly int Index;
        public readonly IArray<Vector3> Vertices;

        public int ShapeVertexOffset => G3D.ShapeVertexOffsets[Index];
        public int NumVertices => G3D.ShapeVertexCounts[Index];
        public Vector4 Color => G3D.ShapeColors[Index];
        public float Width => G3D.ShapeWidths[Index];

        public G3dShape(G3D parent, int index)
        {
            (G3D, Index) = (parent, index);
            Vertices = G3D.ShapeVertices?.SubArray(ShapeVertexOffset, NumVertices);
        }
    }
}
