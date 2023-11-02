using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Ara3D.Noise.Tests
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
            var f = 3.1f;
            var f2 = new float2(3, 4);
            var f3 = new float3(5, 6, 8);
            var f4 = new float4(f2, f2);
            var x1 = noise.cnoise(f2);
            var x2 = noise.cnoise(f3);
            var x3 = noise.cnoise(f4);
            var x5 = noise.pnoise(f2, f2);
            var x6 = noise.pnoise(f3, f3);
            var x7 = noise.pnoise(f4, f4);
            var x8 = noise.cellular(f2);
            var x9 = noise.cellular(f3);
            var x10 = noise.cellular2x2(f2);
            var x11 = noise.cellular2x2x2(f3);
            var x12 = noise.psrdnoise(f2, f2);
            var x13 = noise.psrdnoise(f2, f2, f);
            var x14 = noise.psrnoise(f2, f2);
            var x15 = noise.psrnoise(f2, f2, f);
            var x16 = noise.snoise(f2);
            var x17 = noise.snoise(f3);
            var x18 = noise.snoise(f3, out f3);
            var x19 = noise.snoise(f4);
            var x20 = noise.srdnoise(f2);
            var x21 = noise.srdnoise(f2, f);
            var x22 = noise.srnoise(f2);
            var x23 = noise.srnoise(f2, f);
        }
    }
}