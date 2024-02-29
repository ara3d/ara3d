using System;
using Ara3D.Math;

namespace Ara3D.Geometry
{
    public static class PolarFunctions
    {
        public static Func<float, float> Circle = 
            t => 1;
        
        // https://en.wikipedia.org/wiki/Lima%C3%A7on
        public static Func<float, float> Limacon(float a, float b) 
            => t => b + a * t.Cos();

        // https://en.wikipedia.org/wiki/Cardioid
        public static Func<float, float> Cardoid = 
            Limacon(1, 1);

        // https://en.wikipedia.org/wiki/Rose_(mathematics)
        public static Func<float, float> Rose(int k) => 
            t => k * t.Cos();

        // https://en.wikipedia.org/wiki/Archimedean_spiral
        public static Func<float, float> ArchmideanSpiral(float a, float b) => 
            t => a + b * t;

        // https://en.wikipedia.org/wiki/Conic_section
        public static Func<float, float> ConicSection(float eccentricity, float semiLatusRectum) 
            => t => semiLatusRectum / (1 - eccentricity * t.Cos());
    }
}