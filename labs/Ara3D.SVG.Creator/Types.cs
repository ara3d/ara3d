using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara3D.Collections;

namespace Ara3D.SVG.Creator
{
    public interface IVector
    {
        double X { get; }
        double Y { get; }
    }
    
    public class Lines { }

    public class FilledShape { }

    public class Path { }

    public interface IFunction<TInput, TOuput>
    {
        public TInput InputMin { get; }
        public TInput InputMax { get; }
        public TInput Output { get; }
    }

    //==

    public class FunctionToPoints { }

    //==
}
