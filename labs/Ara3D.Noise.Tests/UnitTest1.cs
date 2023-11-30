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
            var x1 = Noise.cnoise(f2);
            var x2 = Noise.cnoise(f3);
            var x3 = Noise.cnoise(f4);
            var x5 = Noise.pnoise(f2, f2);
            var x6 = Noise.pnoise(f3, f3);
            var x7 = Noise.pnoise(f4, f4);
            var x8 = Noise.cellular(f2);
            var x9 = Noise.cellular(f3);
            var x10 = Noise.cellular2x2(f2);
            var x11 = Noise.Cellular2X2X2(f3);
            var x12 = Noise.psrdnoise(f2, f2);
            var x13 = Noise.psrdnoise(f2, f2, f);
            var x14 = Noise.psrnoise(f2, f2);
            var x15 = Noise.psrnoise(f2, f2, f);
            var x16 = Noise.snoise(f2);
            var x17 = Noise.snoise(f3);
            var x18 = Noise.snoise(f3, out f3);
            var x19 = Noise.snoise(f4);
            var x20 = Noise.srdnoise(f2);
            var x21 = Noise.srdnoise(f2, f);
            var x22 = Noise.srnoise(f2);
            var x23 = Noise.srnoise(f2, f);
        }
    }
}