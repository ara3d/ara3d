namespace PathTracer
{
    public interface IPathTracer
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int SamplesCount { get; set; }
        public int BounceCount { get; set; }
        public (float, float, float) Eval(int x, int y);
    }
}