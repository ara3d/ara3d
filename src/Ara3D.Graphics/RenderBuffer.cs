using System;
using System.IO;
using Ara3D.Buffers;
using Ara3D.Collections;
using Ara3D.Mathematics;
using Ara3D.Utils;

namespace Ara3D.Graphics
{
    public class RenderBuffer<T> : IRenderBuffer<T> where T: unmanaged
    {
        public IBuffer<T> Buffer { get; }

        public RenderBuffer(IBuffer<T> buffer, string semantic)
        {
            Buffer = buffer;
            Semantic = semantic;
            switch (semantic)
            {
                case Semantics.Index:
                    Verifier.AssertEquals(typeof(T), typeof(Int3), "Only Int3 supported for index buffers");
                    Association = ElementAssociation.Face;
                    Arity = 3;
                    PrimitiveType = PrimitiveType.Int32;
                    break;
                case Semantics.Color:
                    Verifier.AssertEquals(typeof(T), typeof(ColorRGBA), "Only ColorRGBA supported for position buffers");
                    Association = ElementAssociation.Vertex;
                    Arity = 4;
                    PrimitiveType = PrimitiveType.Int8;
                    break;
                case Semantics.Normal:
                    Verifier.AssertEquals(typeof(T), typeof(Vector3), "Only Vector3 supported for position buffers");
                    Association = ElementAssociation.Vertex;
                    Arity = 3;
                    PrimitiveType = PrimitiveType.Float32;
                    break;
                case Semantics.UV:
                    Verifier.AssertEquals(typeof(T), typeof(Vector2), "Only Vector2 supported for UV buffers");
                    Association = ElementAssociation.Vertex;
                    Arity = 2;
                    PrimitiveType = PrimitiveType.Float32;
                    break;
                case Semantics.Position:
                    Verifier.AssertEquals(typeof(T), typeof(Vector3), "Only Vector3 supported for position buffers");
                    Association = ElementAssociation.Vertex;
                    Arity = 3;
                    PrimitiveType = PrimitiveType.Float32;
                    break;
                default:
                    throw new Exception("Not a recognized render buffer");
            }
        }

        public void WithPointer(Action<IntPtr> action)
            => Buffer.WithPointer(action);

        public int PrimitiveSize
        {
            get
            {
                switch (PrimitiveType)
                {
                    case PrimitiveType.Int8: return 3;
                    case PrimitiveType.Int32: return 4;
                    case PrimitiveType.Float32: return 4;
                    case PrimitiveType.Float64: return 8;
                    default: throw new Exception("Not a recognized primitive type");
                }
            }
        }

        public int ElementSize
            => PrimitiveSize * Arity;

        public void Write(Stream stream)
        {
            // TODO: this should not be part of the interfaces 
            throw new NotImplementedException();
        }

        Array IBuffer.Data => Buffer.Data;
        public string Name => Semantic;
        public ElementAssociation Association { get; }
        public int Arity { get; }
        public string Semantic { get; }
        public PrimitiveType PrimitiveType { get; }
        public IArray<T> Array => Buffer.GetTypedData().ToIArray();
    }
}