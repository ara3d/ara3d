using System;
using System.IO;
using Ara3D.Serialization.G3D;
using UnityEditor.AssetImporters;
using UnityEngine;
using Matrix4x4 = Ara3D.Mathematics.Matrix4x4;
using Transform = UnityEngine.Transform;

namespace Ara3D.UnityBridge
{
    public static class Util
    {
        /// <summary>
        /// Imports a G3D file
        /// </summary>
        public static GameObject Load3DGameObject(string filePath)
        {
            var name = Path.GetFileName(filePath);
            return CreateGameObject(name, LoadG3DMesh(filePath));
        }
         
        public static Mesh ToUnity(this G3D g3d)
        {
            // TODO: finish this .
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a Unity mesh loaded from the G3D file at filePath.
        /// </summary>
        public static Mesh LoadG3DMesh(string filePath, string name = null)
        {
            var g3d = G3D.Read(filePath);
            var mesh = g3d.ToUnity();
            mesh.name = filePath != null ? Path.GetFileName(filePath) : name;
            return mesh;
        }

        public static void ImportG3D(this AssetImportContext ctx)
        {
            var root = Load3DGameObject(ctx.assetPath);
            ctx.AddObjectToAsset(root.name, root);
            ctx.SetMainObject(root);
        }

        /// <summary>
        /// Creates a game object whose transform matches that of the given matrix.
        /// </summary>
        public static GameObject CreateGameObject(Matrix4x4 matrix, string name, Transform parent = null, bool worldPositionStays = true)
        {
            var obj = new GameObject(name);
            obj.transform.SetFromMatrix(matrix);
            if (parent != null)
                obj.transform.SetParent(parent, worldPositionStays);
            obj.isStatic = true;
            return obj;
        }

        /// <summary>
        /// Builds a renderable game object from mesh and material
        /// </summary>
        public static GameObject CreateGameObject(string name, Mesh mesh = null, Material mtl = null)
        {
            var gameObject = new GameObject(name);
            if (mesh != null)
            {
                gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
                //gameObject.AddComponent<MeshCollider>().sharedMesh = mesh;
                var mr = gameObject.AddComponent<MeshRenderer>();
                // TODO: shared material? 
                if (mtl != null) { mr.material = mtl; }
                // https://answers.unity.com/questions/42187/sharing-a-generated-mesh-between-multiple-game-obj.html
                // https://answers.unity.com/questions/63313/difference-between-sharedmesh-and-mesh.html
            }
            gameObject.isStatic = true;
            return gameObject;
        }

        /// <summary>
        /// Creates an instance of the game object based on the given node's transform.
        /// </summary>
        public static Transform CreateInstance(this GameObject obj, string name, Matrix4x4 matrix, bool worldPositionStays = true)
        {
            var transform = UnityEngine.Object.Instantiate(obj.transform, obj.transform.parent, worldPositionStays);
            transform.SetFromMatrix(matrix);
            transform.gameObject.name = name;
            return transform;
        }

        public static void AssignTransform(this Transform dest, Transform src)
        {
            dest.position = src.position;
            dest.rotation = src.rotation;
            // (!) About "lossyScale" (from the Unity docs):
            //
            // "Please note that if you have a parent transform with scale and a child that is arbitrarily rotated,
            // the scale will be skewed.Thus scale can not be represented correctly in a 3 component vector but only a
            // 3x3 matrix. Such a representation is quite inconvenient to work with however. lossyScale is a convenience
            // property that attempts to match the actual world scale as much as it can.If your objects are not skewed
            // the value will be completely correct and most likely the value will not be very different if it contains
            // skew too."
            dest.localScale = src.lossyScale;
        }

        public static void AssignTransform(this GameObject obj, Transform transform)
            => obj.transform.AssignTransform(transform);

        /// <summary>
        /// Creates a game object whose transform matches the world space transform of another given object.
        /// </summary>
        public static GameObject CreateInWorldSpace(this GameObject obj, string name)
        {
            var gameObject = new GameObject(name);
            gameObject.isStatic = true;
            gameObject.transform.AssignTransform(obj.transform);
            return gameObject;
        }
    }
}
