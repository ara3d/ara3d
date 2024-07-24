using System.Collections.Generic;
using Ara3D.Utils;

namespace Ara3D.SimpleDB
{
    public interface ISimpleDatabaseSerializable
    {
        int Size();
        object Read(byte[] bytes, ref int offset, IReadOnlyList<string> strings);
        void Write(byte[] bytes, ref int offset, IndexedSet<string> strings);
    }
}