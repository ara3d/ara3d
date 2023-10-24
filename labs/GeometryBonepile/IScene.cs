using System.Collections.Generic;
using Ara3D.Collections;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    /// <summary>
    /// An IScene is a generic representation of a 3D scene graph.
    /// </summary>
    public interface IScene
    {
        IArray<ISceneNode> Nodes { get; }
        IArray<IMesh> Meshes { get; }
    }

    /// <summary>
    /// A node in a scene graph. 
    /// </summary>
    public interface ISceneNode
    {
        int Id { get; }
        IScene Scene { get; }
        Matrix4x4 Transform { get; }
        int MeshIndex { get; }
        IMesh GetMesh();
        ISceneNode Parent { get; }

        // TODO: DEPRECATE: this needs to be removed, currently only used in Vim.Max.Bridge.
        IArray<ISceneNode> Children { get; }
    }

    public class SceneNodeComparer : EqualityComparer<ISceneNode>, System.Collections.Generic.IComparer<ISceneNode>
    {
        public static readonly SceneNodeComparer Instance = new SceneNodeComparer();

        public int Compare(ISceneNode x, ISceneNode y)
            => x.Id - y.Id;
        public override bool Equals(ISceneNode x, ISceneNode y)
            => x.Id == y.Id;
        public override int GetHashCode(ISceneNode obj)
            => obj.Id;
    }

    public class NullNode : ISceneNode
    {
        public static NullNode Instance = new NullNode();
        public static List<ISceneNode> ListInstance = new List<ISceneNode>() { Instance };
        public int Id => -1;
        public IScene Scene => null;
        public Matrix4x4 Transform => Matrix4x4.Identity;
        public int MeshIndex => 0;
        public ISceneNode Parent => null;
        public IArray<ISceneNode> Children => null;
        public IMesh GetMesh() => null;
    }

    public class IdNode : ISceneNode
    {
        public int Id { get; set; }
        public IScene Scene => null;
        public Matrix4x4 Transform => Matrix4x4.Identity;
        public int MeshIndex => 0;
        public ISceneNode Parent => null;
        public IArray<ISceneNode> Children => null;
        public IMesh GetMesh() => null;
    }
}
