﻿using g3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ara3D.Collections;
using Ara3D.Geometry;
using Ara3D.Mathematics;

namespace Ara3D
{
    public static class G3SharpBridge
    {
        // https://github.com/gradientspace/geometry3Sharp/issues/3
        public static DMesh3 Compact(this DMesh3 mesh, bool compactFlag = true)
            => compactFlag ? new DMesh3(mesh, true) : mesh;    

        public static DMesh3 Slice(this DMesh3 self, Plane plane, bool compact = true)
        {
            var normal = plane.Normal;
            var origin = normal * -plane.D;
            var cutter = new MeshPlaneCut(self, origin.ToVector3D(), normal.ToVector3D());
            var result = cutter.Cut();
            Console.WriteLine($"Cutting result = {result}");
            Console.WriteLine($"Cut loops = {cutter.CutLoops.Count}");
            Console.WriteLine($"Cut spans = {cutter.CutSpans.Count}");
            Console.WriteLine($"Cut faces = {cutter.CutFaceSet?.Count ?? 0}");
            return cutter.Mesh.Compact(compact);
        }

        public static DMesh3 ReduceWithProjection(this DMesh3 mesh, float percent, bool compactResult = true)
            => mesh.Reduce(percent, compactResult, mesh.AABBTree());

        public static DMesh3 Reduce(this DMesh3 mesh, float percent, bool compactResult = true, ISpatial target = null)
        {
            // TODO: not sure what triggers this
            //if (!mesh.CheckValidity(eFailMode: FailMode.ReturnOnly)) return mesh;

            var r = new Reducer(mesh);

            if (target != null)
            {
                r.SetProjectionTarget(new MeshProjectionTarget(mesh, target));

                // http://www.gradientspace.com/tutorials/2017/8/30/mesh-simplification
                // r.ProjectionMode = Reducer.TargetProjectionMode.Inline;
            }

            return r.Reduce((int)(mesh.VertexCount * percent / 100.0), compactResult);
        }


        public static DMesh3 Reduce(this Reducer reducer, int newVertexCount, bool compactResult = true)
        {
            reducer.ReduceToVertexCount(newVertexCount);
            return reducer.Mesh.Compact(compactResult);
        }

        public static DMeshAABBTree3 AABBTree(this DMesh3 mesh)
        {
            var tree = new DMeshAABBTree3(mesh);
            tree.Build();
            return tree;
        }

        public static double? DistanceToTree(this DMeshAABBTree3 tree, Ray3d ray)
        {
            var hit_tid = tree.FindNearestHitTriangle(ray);
            if (hit_tid == DMesh3.InvalidID) return null;
            var intr = MeshQueries.TriangleIntersection(tree.Mesh, hit_tid, ray);
            return ray.Origin.Distance(ray.PointAt(intr.RayParameter));
        }

        public static Vector3d NearestPoint(this DMeshAABBTree3 tree, Vector3d point)
        {
            var tid = tree.FindNearestTriangle(point);
            if (tid == DMesh3.InvalidID)
                throw new Exception("Could not find nearest triangle");

            var dist = MeshQueries.TriangleDistance(tree.Mesh, tid, point);
            return dist.TriangleClosest;
        }

        public static double DistanceToTree(this DMeshAABBTree3 tree, Vector3d point)
        {
            var tid = tree.FindNearestTriangle(point);
            if (tid == DMesh3.InvalidID)
                throw new Exception("Could not find nearest triangle");

            var dist = MeshQueries.TriangleDistance(tree.Mesh, tid, point);
            return dist.GetSquared().Sqrt();
        }

        public static IArray<Vector3d> NearestPoints(this ITriMesh self, IArray<Vector3d> points)
        {
            var tree = self.ToG3Sharp().AABBTree();
            return points.Select(tree.NearestPoint);
        }

        public static ITriMesh ToAra3DMesh(this List<DMesh3> meshes)
            => meshes.ToIArray().Select(ToAra3D).Merge();

        public static List<DMesh3> LoadMeshes(string path)
        {
            var builder = new DMesh3Builder();
            var reader = new StandardMeshReader {MeshBuilder = builder};
            var result = reader.Read(path, ReadOptions.Defaults);
            if (result.code == IOCode.Ok)
                return builder.Meshes;
            return null;
        }

        public static void WriteFile(this DMesh3 mesh, string filePath)
            => mesh.WriteFile(filePath, WriteOptions.Defaults);

        public static void WriteFileBinary(this DMesh3 mesh, string filePath)
            => mesh.WriteFile(filePath, new WriteOptions {bWriteBinary = true});

        public static void WriteFileAscii(this DMesh3 mesh, string filePath)
            => mesh.WriteFile(filePath, new WriteOptions { bWriteBinary = false });

       public static void WriteFile(this DMesh3 mesh, string filePath, WriteOptions opts)
        {
            var writer = new StandardMeshWriter();
            var m = new WriteMesh(mesh);
            var result = writer.Write(filePath, new List<WriteMesh> { m }, opts);
            if (!result.Equals(IOWriteResult.Ok))
                throw new Exception($"Failed to write file to {filePath} with result {result.ToString()}");
        }
        public static IArray<Vector3> ToAra3D(this DVector<double> self)
        {
            return (self.Length / 3).Select(i => new Vector3((float)self[i * 3], (float)self[i * 3 + 1], (float)self[i * 3 + 2]));
        }

        public static Vector3 ToAra3D(this Vector3d self)
        {
            return new Vector3((float)self.x, (float)self.y, (float)self.z);
        }

        public static AABox ToAra3D(this AxisAlignedBox3d box)
        {
            return (box.Min.ToAra3D(), box.Max.ToAra3D());
        }

        public static ITriMesh ToAra3D(this DMesh3 self)
        {
            var verts = self.Vertices().Select(ToAra3D).ToIArray();
            var indices = self.TrianglesBuffer.ToIArray();
            return verts.ToTriMesh(indices);
        }

        public static Vector3d ToVector3D(this Vector3 self)
            => new Vector3d(self.X, self.Y, self.Z);        

        public static Vector3d ToG3Sharp(this Vector3 self)
            => new Vector3d(self.X, self.Y, self.Z);        

        public static DMesh3 ToG3Sharp(this ITriMesh self)
        {
            var r = new DMesh3();
            foreach (var v in self.Points.ToEnumerable())
                r.AppendVertex(v.ToVector3D());
            var indices = self.Indices();
            for (var i = 0; i < indices.Count; i += 3)
            {
                var result = r.AppendTriangle(indices[i], indices[i + 1], indices[i + 2]);
                if (result < 0)
                {
                    if (result == DMesh3.NonManifoldID)
                        throw new Exception("Can't create non-manifold mesh");
                    if (result == DMesh3.InvalidID)
                        throw new Exception("Invalid vertex ID");
                    throw new Exception("Unknown error creating mesh");
                }
            }
            Debug.Assert(r.CheckValidity(false, FailMode.DebugAssert));
            return r;
        }

        public static ITriMesh Reduce(this ITriMesh self, float percent)
            => self.ToG3Sharp().Reduce(percent).ToAra3D();        
    }
}
