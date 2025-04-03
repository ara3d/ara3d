﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Ara3D.Collections;

namespace Ara3D.Serialization.G3D
{
    public static class GeometryAttributesExtensions
    {
        public static int ExpectedElementCount(this IGeometryAttributes self, AttributeDescriptor desc)
        {
            switch (desc.Association)
            {
                case Association.assoc_all:
                    return 1;
                case Association.assoc_none:
                    return 0;
                case Association.assoc_vertex:
                    return self.NumVertices;
                case Association.assoc_face:
                    return self.NumFaces;
                case Association.assoc_corner:
                    return self.NumCorners;
                case Association.assoc_edge:
                    return self.NumCorners;
                case Association.assoc_mesh:
                    return self.NumMeshes;
                case Association.assoc_instance:
                    return self.NumInstances;
                case Association.assoc_shapevertex:
                    return self.NumShapeVertices;
                case Association.assoc_shape:
                    return self.NumShapes;
            }
            return -1;
        }

        public static IArray<string> AttributeNames(this IGeometryAttributes g)
            => g.Attributes.Select(attr => attr.Name);

        public static GeometryAttribute<T> GetAttribute<T>(this IGeometryAttributes g, string attributeName) where T : unmanaged
            => g.GetAttribute(attributeName)?.AsType<T>();

        public static GeometryAttribute DefaultAttribute(this IGeometryAttributes self, string name)
            => self.DefaultAttribute(AttributeDescriptor.Parse(name));

        public static GeometryAttribute DefaultAttribute(this IGeometryAttributes self, AttributeDescriptor desc)
            => desc.ToDefaultAttribute(self.ExpectedElementCount(desc));

        public static GeometryAttribute GetOrDefaultAttribute(this IGeometryAttributes self, AttributeDescriptor desc)
            => self.GetAttribute(desc.ToString()) ?? desc.ToDefaultAttribute(self.ExpectedElementCount(desc));

        public static IEnumerable<GeometryAttribute> NoneAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_none);

        public static IEnumerable<GeometryAttribute> CornerAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_corner);

        public static IEnumerable<GeometryAttribute> EdgeAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_edge);

        public static IEnumerable<GeometryAttribute> FaceAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_face);

        public static IEnumerable<GeometryAttribute> VertexAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_vertex);

        public static IEnumerable<GeometryAttribute> InstanceAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_instance);

        public static IEnumerable<GeometryAttribute> MeshAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_mesh);

        public static IEnumerable<GeometryAttribute> SubMeshAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_submesh);

        public static IEnumerable<GeometryAttribute> WholeGeometryAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_all);

        public static IEnumerable<GeometryAttribute> ShapeVertexAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_shapevertex);

        public static IEnumerable<GeometryAttribute> ShapeAttributes(this IGeometryAttributes g)
            => g.Attributes.Where(a => a.Descriptor.Association == Association.assoc_shape);

        public static bool HasSameAttributes(this IGeometryAttributes g1, IGeometryAttributes g2)
            => g1.Attributes.Count == g2.Attributes.Count && g1.Attributes.Indices().All(i => g1.Attributes[i].Name == g2.Attributes[i].Name);

        public static int FaceToCorner(this IGeometryAttributes g, int f)
            => f * g.NumCornersPerFace;

        /// <summary>
        /// Given a set of face indices, creates an array of corner indices
        /// </summary>
        public static IArray<int> FaceIndicesToCornerIndices(this IGeometryAttributes g, IArray<int> faceIndices)
            => (faceIndices.Count * g.NumCornersPerFace)
                .Select(i => g.FaceToCorner(faceIndices[i / g.NumCornersPerFace]) + i % g.NumCornersPerFace);

        /// <summary>
        /// Given a set of face indices, creates an array of indices of the first corner in each face
        /// </summary>
        public static IArray<int> FaceIndicesToFirstCornerIndices(this IGeometryAttributes g, IArray<int> faceIndices)
            => faceIndices.Select(f => f * g.NumCornersPerFace);

        public static int CornerToFace(this IGeometryAttributes g, int c)
            => c / g.NumCornersPerFace;

        public static IArray<int> CornersToFaces(this IGeometryAttributes g)
            => g.NumCorners.Select(g.CornerToFace);

        public static int CornerNumber(this IGeometryAttributes g, int c)
            => c % g.NumCornersPerFace;

        public static IGeometryAttributes ToGeometryAttributes(this IEnumerable<GeometryAttribute> attributes)
            => new GeometryAttributes(attributes);

        public static IGeometryAttributes ToGeometryAttributes(this IArray<GeometryAttribute> attributes)
            => attributes.ToEnumerable().ToGeometryAttributes();

        public static IGeometryAttributes AddAttributes(this IGeometryAttributes attributes, params GeometryAttribute[] newAttributes)
            => attributes.Attributes.ToEnumerable().Concat(newAttributes).ToGeometryAttributes();

        public static GeometryAttribute GetAttributeOrDefault(this IGeometryAttributes g, string name)
            => g.GetAttribute(name) ?? g.DefaultAttribute(name);

     
        public static IGeometryAttributes SetAttribute(this IGeometryAttributes self, GeometryAttribute attr)
            => self.Attributes.Where(a => !a.Descriptor.Equals(attr.Descriptor)).Append(attr).ToGeometryAttributes();

        public static IGeometryAttributes SetAttribute<ValueT>(this IGeometryAttributes self, IArray<ValueT> values, AttributeDescriptor desc) where ValueT : unmanaged
            => self.SetAttribute(values.ToAttribute(desc));

        public static IEnumerable<GeometryAttribute> SetFaceSizeAttribute(this IEnumerable<GeometryAttribute> attributes, int numCornersPerFaces)
            => (numCornersPerFaces <= 0)
                   ? attributes
                   : attributes
                        .Where(attr => attr.Descriptor.Semantic != Semantic.FaceSize)
                        .Append(new[] { numCornersPerFaces }.ToObjectFaceSizeAttribute());

        public static G3D ToG3d(this IEnumerable<GeometryAttribute> attributes, G3dHeader? header = null)
            => new G3D(attributes, header);

        public static G3D ToG3d(this IArray<GeometryAttribute> attributes, G3dHeader? header = null)
            => attributes.ToEnumerable().ToG3d(header);

        public static IArray<int> IndexFlippedRemapping(this IGeometryAttributes g)
            => g.NumCorners.Select(c => ((c / g.NumCornersPerFace) + 1) * g.NumCornersPerFace - 1 - c % g.NumCornersPerFace);

        public static bool IsNormalAttribute(this GeometryAttribute attr)
            => attr.IsType<Vector3>() && attr.Descriptor.Semantic == "normal";

        public static IArray<int> DefaultMaterials(this IGeometryAttributes self)
            => (-1).Repeat(self.NumFaces);

        public static IArray<Vector4> DefaultColors(this IGeometryAttributes self)
            => Vector4.Zero.Repeat(self.NumVertices);

        public static IArray<Vector2> DefaultUvs(this IGeometryAttributes self)
            => Vector2.Zero.Repeat(self.NumVertices);

        public static IGeometryAttributes Replace(this IGeometryAttributes self, Func<AttributeDescriptor, bool> selector, GeometryAttribute attribute)
            => self.Attributes.Where(a => !selector(a.Descriptor)).Append(attribute).ToGeometryAttributes();

        public static IGeometryAttributes Remove(this IGeometryAttributes self, Func<AttributeDescriptor, bool> selector)
            => self.Attributes.Where(a => !selector(a.Descriptor)).ToGeometryAttributes();

        public static G3D ToG3d(this IGeometryAttributes self)
            => G3D.Create(self.Attributes.ToArray());
    }
}
