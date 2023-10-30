using System.Diagnostics;
using Ara3D.Math;
using Ara3D.Utils;
using Ara3D.Graphics;

// https://gist.github.com/mattwarren/d17a0c356bd6fdb9f596bee6b9a5e63c
// https://fabiensanglard.net/postcard_pathtracer/index.html
// https://mattwarren.org/2019/03/01/Is-CSharp-a-low-level-language/

// https://fabiensanglard.net/postcard_pathtracer/index.html
// https://fabiensanglard.net/postcard_pathtracer/formatted_full.html

// https://graphics.pixar.com/library/indexAuthorAndrew_Kensler.html

// https://news.ycombinator.com/item?id=6425965
// http://tog.acm.org/resources/RTNews/html/ (Somewhat dead, but the archives are great)
// http://ompf2.com/ (Active, lots of ray tracing specific discussion)
// http://www.realtimerendering.com/ (I'm fond of the book as well)
// http://kesen.realtimerendering.com/ (Especially the sections for "Symposium on Interactive Ray Tracing" and it's successor, "High Performance Graphics")

namespace PathTracer
{
    public static class PathTracerProgram
    {
        // https://fabiensanglard.net/postcard_pathtracer/index.html
        // 2 minutes, 58 seconds in C++.
        // divisor = 1, samplesCount = 16;

        public const int Divisor = 8;
        public const int SamplesCount = 64;
        public const int Width = 960 / Divisor;
        public const int Height = 540 / Divisor;
        public const int BounceCount = 3;
        public const float SkyHeight = 19.9f;
        public const float AttenuationFactor = 0.2f;
        public const float MinMarchingDistance = 0.01f;
        public const float PlankDistance = 8;
        public const float MaxDistance = 100;

        public static readonly Vector3 SunColor = (50, 80, 100);
        public static readonly Vector3 Position = (-22, 5, 25);
        public static readonly Vector3 Goal = (new Vector3(-3, 4, 0) + Position * -1).Normalize();
        public static readonly Vector3 Left = new Vector3(Goal.Z, 0, -Goal.X).Normalize() * (1.0f / Width);
        public static readonly Vector3 Up = Goal.Cross(Left);
        public static readonly Vector3 WallHitColor = (500, 400, 100);

        public static readonly AABox LowerRoomBounds = (MakeVector(-30, -0.5f, -30), MakeVector(30, 18, 30));
        public static readonly AABox UpperRoomBounds = (MakeVector(-25, 17, -25), MakeVector(25, 20, 25));
        public static readonly AABox CeilingBounds = (MakeVector(1.5f, 18.5f, -25), MakeVector(6.5f, 20, 25));


        public static readonly Vector3 LightDirection = MakeVector(.6f, .6f, 1f).Normalize();

        private static readonly Random _random = new();
        
        public static Vector3 MakeVector(float a, float b, float c = 0) => new(a, b, c);
        public static float Min(float l, float r) => Math.Min(l, r);
        public static float RandomVal() => (float)_random.NextDouble();

        // Rectangle CSG equation. Returns minimum signed distance from space carved by
        // lowerLeft vertex and opposite rectangle vertex upperRight.    
        public static float BoxTest(Vector3 position, AABox box)
        {
            var lowerLeft = position + (box.Min * -1.0f);
            var upperRight = box.Max + (position * -1.0f);
            return -Min(Min(
                Min(lowerLeft.X, upperRight.X), 
                Min(lowerLeft.Y, upperRight.Y)), 
                Min(lowerLeft.Z, upperRight.Z));
        }

        public const int HIT_NONE = 0;
        public const int HIT_LETTER = 1;
        public const int HIT_WALL = 2;
        public const int HIT_SUN = 3;

        public static readonly char[] LetterData =              
            // 15 two points lines
            // P (without curve)   
            ("5O5_" + "5W9W" + "5_9_" +        
            // I
            "AOEO" + "COC_" + "A_E_" +        
            // X
             "IOQ_" + "I_QO" +                  
            // A
            "UOY_" + "Y_]O" + "WW[W" +          
            // R (without curve)    
            "aOa_" + "aWeW" + "a_e_" + "cWiO") 
            .ToCharArray();

        // Two Curves (for P and R in PixaR) with hard-coded locations.
        public static readonly Vector3[] Curves =
        {
            MakeVector(-11, 6), 
            MakeVector(11, 6)
        };

        public static readonly Line[] Lines = Enumerable.Range(0, 15).Select(ComputeLine).ToArray();

        public static Line ComputeLine(int n)
        {
            var i = n * 4;
            var a = MakeVector(LetterData[i] - 79, LetterData[i + 1] - 79) * 0.5f;
            var b = MakeVector(LetterData[i + 2] - 79, LetterData[i + 3] - 79) * 0.5f - a;
            return new Line(a, b);
        }

        // Sample the world using Signed Distance Fields.
        public static float QuerySDF(Vector3 position, out int hitType)
        {
            var distance2 = 1e9f; 
            
            // Flattened position
            var f = position.SetZ(0);

            // Hit-test to lines 
            for (var i = 0; i < Lines.Length; i++)
            {
                var line = Lines[i];
                var o = f -(line.A + line.B * 
                    Min(
                        -Min((line.A - f).Dot(line.B) / line.B.Dot(line.B), 0), 1));
                distance2 = Min(distance2, o.Dot(o)); 
            }

            // Compute real distance 
            var distance = distance2.Sqrt();

            // Hit test curves
            for (var i = 1; i >= 0; i--)
            {
                var o = f - Curves[i];
                float temp;
                if (o.X > 0)
                {
                    temp = (o.Dot(o).Sqrt() - 2).Abs();
                }
                else
                {
                    o.SetY(o.Y > 0 ? o.Y - 2 : o.Y + 2);
                    temp = o.Dot(o).Sqrt();
                }
                distance = Min(distance, temp);
            }

            distance = (distance.Pow(8) + position.Z.Pow(8)).Pow(0.125f) - 0.5f;
            hitType = HIT_LETTER;

            var roomDist =
                // Min(A,B) = Union with Constructive solid geometry
                Min(
                    //-Min carves an empty space
                    -Min(
                        BoxTest(position, LowerRoomBounds),
                        BoxTest(position, UpperRoomBounds)
                    ),
                BoxTest( 
                    // Ceiling "planks" spaced 8 units apart.
                    MakeVector(position.X.Abs() % PlankDistance,
                        position.Y,
                        position.Z),
                    CeilingBounds));

            if (roomDist < distance)
            {
                distance = roomDist; 
                hitType = HIT_WALL;
            };

            var sun = SkyHeight - position.Y; 
            if (sun < distance)
            {
                distance = sun;
                hitType = HIT_SUN;
            }

            return distance;
        }

        // Perform signed sphere marching
        // Returns hitType 0, 1, 2, or 3 and update hit position/normal
        public static int RayMarching(Vector3 origin, Vector3 direction, ref Vector3 hitPos, ref Vector3 hitNorm)
        {
            var noHitCount = 0;
            float d; // distance from closest object in world.

            // Signed distance marching
            for (float total_d = 0; total_d < MaxDistance; total_d += d)
                if ((d = QuerySDF(hitPos = origin + direction * total_d, out var hitType)) < MinMarchingDistance
                        || ++noHitCount > 99)
                {
                    hitNorm =
                       MakeVector(QuerySDF(hitPos + MakeVector(.01f, 0), out _) - d,
                            QuerySDF(hitPos + MakeVector(0, .01f), out _) - d,
                            QuerySDF(hitPos + MakeVector(0, 0, .01f), out _) - d)
                           .Normalize();
                    return hitType;
                }
            return 0;
        }

        public static Vector3 Trace(Vector3 origin, Vector3 direction)
        {
            var sampledPosition = Vector3.Zero;
            var normal = Vector3.Zero;
            var color = Vector3.Zero; 
            var attenuation = Vector3.One;

            for (var bounceCount = BounceCount; bounceCount > 0; bounceCount--)
            {
                var hitType = RayMarching(origin, direction, ref sampledPosition, ref normal);
                if (hitType == HIT_NONE) 
                    break; 

                if (hitType == HIT_LETTER)
                {
                    // Specular bounce on a letter. No color acc.
                    direction += normal * (normal.Dot(direction) * -2);
                    origin = sampledPosition + direction * 0.1f;
                    attenuation *= AttenuationFactor; // Attenuation via distance traveled.
                }
                else if (hitType == HIT_WALL)
                {
                    // Wall hit uses color yellow?
                    var incidence = normal.Dot(LightDirection);
                    var p = 6.283185f * RandomVal();
                    var c = RandomVal();
                    var s = (1 - c).Sqrt();
                    var g = normal.Z < 0 ? -1f : 1f;
                    var u = -1 / (g + normal.Z);
                    var v = normal.X * normal.Y * u;
                    direction = MakeVector(v, g + normal.Y.Sqr() * u, -normal.Y) * (p.Cos() * s) +
                                MakeVector(1 + g * normal.X * normal.X * u, g * v, -g * normal.X) * (p.Sin() * s) + normal * c.Sqrt();
                    origin = sampledPosition + direction * .1f;
                    attenuation *= AttenuationFactor;
                    
                    if (incidence > 0 &&
                        RayMarching(sampledPosition + normal * .1f,
                                    LightDirection,
                                    ref sampledPosition,
                                    ref normal) == HIT_SUN)
                        color += attenuation * WallHitColor * incidence;
                }
                else if (hitType == HIT_SUN)
                { 
                    color += attenuation * SunColor; 
                    break; 
                }
            }
            return color;
        }

        public static ColorRGBA Eval(int x, int y)
        {
            var color = Vector3.Zero;
            for (var p = SamplesCount - 1; p >= 0; p--)
            {
                color += Trace(Position, 
                    (Goal + Left * 
                        (x - Width / 2 + RandomVal()) + Up * 
                        (y - Height / 2 + RandomVal())).Normalize());
            }

            color *= (1.0f / SamplesCount) + 14.0f / 241;
            return ColorRGBA.FromVector(ReinhardToneMapping(color));
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Tone_mapping
        /// </summary>
        public static Vector3 ReinhardToneMapping(Vector3 v)
            => v / (v + 1);

        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var bitmap = new Bitmap(Width, Height);

            Parallelizer.ForLoop(Width * Height, i =>
            {
                var x = i % Width;
                var y = i / Width;
                var rgb = Eval(x, y);
                bitmap.SetPixel(Width - 1 - x, y, rgb);
            });
                
            sw.OutputTimeElapsed("Creating bitmap");
            var file = new FilePath(Path.GetTempFileName());
            file = file.ChangeExtension("bmp");
            bitmap.Save(file); 
            file.OpenFile();
        }
    }
}
