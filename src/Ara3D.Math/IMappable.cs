using System;

namespace Ara3D.Mathematics
{
    public interface IMappable<TContainer, TPart>
    {
        TContainer Map(Func<TPart, TPart> f);
    }
}
