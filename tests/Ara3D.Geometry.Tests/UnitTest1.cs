namespace Ara3D.Geometry.Tests
{
    public interface IDazzler
    {
        IDazzler Razzle();
    }

    public class Dazzler : IDazzler
    {
        public IDazzler Razzle()
        {
            Console.WriteLine("Dazzlers will Razzle");
            return this;
        }
    }

    public class Razzler : IDazzler
    {
        public IDazzler Razzle()
        {
            Console.WriteLine("Razzlers will Razzle");
            return this;
        }
    }

    public static class Extensions
    {
        public static T Razzle<T>(this T self) where T : IDazzler
        {
            Console.WriteLine("Everyone will Razzle");
            return (T)self.Razzle();
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var a = new Dazzler();
            var b = new Razzler();
            a.Razzle<Dazzler>();
            b.Razzle<Razzler>();
        }
    }
}