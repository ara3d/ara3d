using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara3D.Collections;

namespace Ara3D.SVG.Creator
{
    public interface IPoints : IArray<IPoint>, IPrimitive
    {
    }

    public interface IPoint
    {
        double X { get; }
        double Y { get; }
    }

    public interface IVector
    {
        double X { get; }
        double Y { get; }
    }
    
    public interface ILine
    {
    }

    public interface IPrimitive { }

    public class Lines { }

    public class FilledShape { }

    public class Path { }

    public interface IFunction<TInput, TOuput>
    {
        public TInput InputMin { get; }
        public TInput InputMax { get; }
        public TInput Output { get; }
    }

    public class Function1D { }

    public class PolarFunction { }

    public class Function2D { }

    public class Function3D { }

    //==

    public class Component { }

    public class FunctionRenderer : Component { }

    public class PointsRenderer : Component { }

    public class FunctionToPoints { }

    //==
}
