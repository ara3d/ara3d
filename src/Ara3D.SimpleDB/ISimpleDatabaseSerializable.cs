using System.Collections.Generic;
using Ara3D.Utils;

namespace Ara3D.SimpleDB
{
    /// <summary>
    /// All objects placed in the database should implement this interface.
    /// If a class cannot be modified, you can use a proxy that implements the interface
    /// that reads and writes objects of the desired type. 
    /// </summary>
    public interface ISimpleDatabaseSerializable
    {
        int Size();
        object Read(byte[] bytes, ref int offset, IReadOnlyList<string> strings);
        void Write(byte[] bytes, ref int offset, IndexedSet<string> strings);
    }
}