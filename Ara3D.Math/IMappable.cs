using System;

namespace Ara3D.Math
{
    public interface IMappable<TContainer, TPart>
    {
        TContainer Map(Func<TPart, TPart> f);
    }
}
