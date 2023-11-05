using NUnit.Framework;

namespace Ezrx.Rhino.Test
{
    public class Tests
    {
        [Test]
        public static void Test1()
        {
            var tmp1 = Evaluator.MakeCube();
            var tmp2 = Evaluator.MakeCubeViaEval();
            Assert.AreEqual(tmp1.Vertices.Count, tmp2.Vertices.Count);
        }
    }
}