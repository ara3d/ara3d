using System.Diagnostics;
using System.Runtime.CompilerServices;
using Plato;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace PathTracer
{

    public static class LocalExtensions
    {
        [MethodImpl(AggressiveInlining)] public static float Sqr(this float n) => n * n;
        [MethodImpl(AggressiveInlining)] public static Number Sqr(this Number n) => n * n;
        [MethodImpl(AggressiveInlining)] public static Vector3 SetX(this Vector3 v, float n) => new(n, v.Y, v.Z);
        [MethodImpl(AggressiveInlining)] public static Vector3 SetY(this Vector3 v, float n) => new(v.X, n, v.Z);
        [MethodImpl(AggressiveInlining)] public static Vector3 SetZ(this Vector3 v, float n) => new(v.X, v.Y, n);
        [MethodImpl(AggressiveInlining)] public static Vector2 XY(this Vector3 v) => new(v.X, v.Y);
    }

    public class PathTracerPlato : IPathTracer
    {
        public readonly struct AABox
        {
            public readonly Vector3 Min;
            public readonly Vector3 Max;

            [MethodImpl(AggressiveInlining)]
            public AABox(Vector3 min, Vector3 max)
            {
                Min = min;
                Max = max;
            }
        }

        public readonly struct Line2D
        {
            public readonly Vector2 A;
            public readonly Vector2 B;
            [MethodImpl(AggressiveInlining)]
            public Line2D(Vector2 a, Vector2 b)
            {
                A = a;
                B = b;
            }
        }

        // https://fabiensanglard.net/postcard_pathtracer/index.html
        // 2 minutes, 58 seconds in C++.
        // divisor = 1, samplesCount = 16;

        public const int MinNoHitCount = 99;
        public const float SkyHeight = 19.9f;
        public const float AttenuationFactor = 0.2f;
        public const float MinMarchingDistance = 0.01f;
        public const float PlankDistance = 8;
        public const float MaxDistance = 100; // 50 makes an interesting effect. Can't see the walls.

        public static readonly Vector3 SunColor = new(50, 80, 100);
        public static readonly Vector3 Position = new(-22, 5, 25);
        public static readonly Vector3 Goal = (new Vector3(-3, 4, 0) + Position * -1).Normalize;
        public Vector3 Left => new Vector3(Goal.Z, 0, -Goal.X).Normalize * (1.0f / Width);
        public Vector3 Up => Goal.Cross(Left);
        public static readonly Vector3 WallHitColor = new(500, 400, 100);

        public static readonly AABox LowerRoomBounds = new(MakeVector(-30, -0.5f, -30), MakeVector(30, 18, 30));
        public static readonly AABox UpperRoomBounds = new(MakeVector(-25, 17, -25), MakeVector(25, 20, 25));
        public static readonly AABox CeilingBounds = new(MakeVector(1.5f, 18.5f, -25), MakeVector(6.5f, 20, 25));

        public static readonly Vector3 LightDirection = MakeVector(.6f, .6f, 1f).Normalize;

        private static readonly Random _random = new Random(99);

        public static Vector3 MakeVector(float a, float b, float c = 0) => new Vector3(a, b, c);
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

        // Two Curves (for P and R in PixaR) with hard-coded locations.
        public static readonly Vector3[] Curves =
        {
            MakeVector(-11, 6),
            MakeVector(11, 6)
        };

        public static Line2D[] Lines = 
        {
            // P without curve
            new(new(-26, 0), new(-26, 16)),
            new(new(-26, 8), new(-22, 8)),
            new(new(-26, 16), new(-22, 16)),
            // I 
            new(new(-14, 0), new(-10, 0)),
            new(new(-12, 0), new(-12, 16)),
            new(new(-14, 16), new(-10, 16)),
            // X 
             new(new(-6, 0), new(2, 16)),
            new(new(-6, 16), new(2, 0)),
            // A 
            new(new(6, 0), new(10, 16)),
            new(new(10, 16), new(14, 0)),
            new(new(8, 8), new(12, 8)),
            // R without curve
            new(new(18, 0), new(18, 16)),
            new(new(18, 8), new(22, 8)),
            new(new(18, 16), new(22, 16)),
            new(new(20, 8), new(26, 0)),
        };

        public enum HitType
        {
            HIT_NONE,
            HIT_LETTER,
            HIT_WALL,
            HIT_SUN
        };

        // Sample the world using Signed Distance Fields.
        public static float QuerySDF(Vector3 position, out HitType hitType)
        {
            var distance2 = 1e9f;

            // Flattened position
            var f = position.SetZ(0);

            // Hit-test to lines 
            foreach (var line in Lines)
            {
                var a = line.A * 0.5f;
                var b = line.B * 0.5f - a;
                var o = f.XY() - (a + b * Min(-Min((a - f.XY()).Dot(b) / b.Dot(b), 0), 1));
                distance2 = Min(distance2, o.Dot(o));
            }

            // Compute real distance 
            var distance = distance2.Sqrt();

            // Hit test curves
            for (var i = 1; i >= 0; i--)
            {
                var o = f - Curves[i];
                if (o.X > 0)
                {
                    distance = Min(distance, (o.Dot(o).SquareRoot - 2).Abs);
                }
                else
                {
                    o = o.SetY(o.Y > 0 ? o.Y - 2 : o.Y + 2);
                    distance = Min(distance, o.Dot(o).SquareRoot);
                }
            }

            distance = (distance.Pow(8) + position.Z.Pow(8)).Pow(0.125f) - 0.5f;
            hitType = HitType.HIT_LETTER;

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
                        MakeVector(position.X.Abs % PlankDistance,
                            position.Y,
                            position.Z),
                        CeilingBounds));

            if (roomDist < distance)
            {
                distance = roomDist;
                hitType = HitType.HIT_WALL;
            }

            var sun = SkyHeight - position.Y;
            if (sun < distance)
            {
                distance = sun;
                hitType = HitType.HIT_SUN;
            }

            return distance;
        }

        // Perform signed sphere marching
        // Returns hitType 0, 1, 2, or 3 and update hit position/normal
        public static HitType RayMarching(Vector3 origin, Vector3 direction, ref Vector3 hitPos, ref Vector3 hitNorm)
        {
            var noHitCount = 0;
            float d; // distance from closest object in world.

            // Signed distance marching
            for (float totalDistance = 0; totalDistance < MaxDistance; totalDistance += d)
                if ((d = QuerySDF(hitPos = origin + direction * totalDistance, out var hitType)) < MinMarchingDistance
                    || ++noHitCount > MinNoHitCount)
                {
                    hitNorm =
                        MakeVector(QuerySDF(hitPos + MakeVector(.01f, 0), out _) - d,
                                QuerySDF(hitPos + MakeVector(0, .01f), out _) - d,
                                QuerySDF(hitPos + MakeVector(0, 0, .01f), out _) - d)
                            .Normalize;
                    return hitType;
                }

            return 0;
        }

        public static readonly Vector3 Zero = new(0);
        public static readonly Vector3 One = new(1);

        public Vector3 Trace(Vector3 origin, Vector3 direction)
        {
            var sampledPosition = Zero;
            var normal = Zero;
            var color = Zero;
            var attenuation = One;

            for (var bounceCount = BounceCount; bounceCount > 0; bounceCount--)
            {
                var hitType = RayMarching(origin, direction, ref sampledPosition, ref normal);
                if (hitType == HitType.HIT_NONE)
                    break;

                if (hitType == HitType.HIT_LETTER)
                {
                    // Specular bounce on a letter. No color acc.
                    direction += normal * (normal.Dot(direction) * -2);
                    origin = sampledPosition + direction * 0.1f;
                    attenuation *= AttenuationFactor; // Attenuation via distance traveled.
                }
                else if (hitType == HitType.HIT_WALL)
                {
                    // Wall hit uses color yellow?
                    var incidence = normal.Dot(LightDirection);
                    var p = 6.283185f * RandomVal();
                    var c = RandomVal();
                    var s = (1 - c).Sqrt();
                    var g = normal.Z < 0 ? -1f : 1f;
                    var u = -1 / (g + normal.Z);
                    var v = normal.X * normal.Y * u;
                    direction = MakeVector(v, g + normal.Y.Sqr() * u, -normal.Y) * (MathF.Cos(p) * s) +
                                MakeVector(1 + g * normal.X * normal.X * u, g * v, -g * normal.X) * (MathF.Sin(p) * s) +
                                normal * c.Sqrt();
                    origin = sampledPosition + direction * .1f;
                    attenuation *= AttenuationFactor;

                    if (incidence > 0 &&
                        RayMarching(sampledPosition + normal * .1f,
                            LightDirection,
                            ref sampledPosition,
                            ref normal) == HitType.HIT_SUN)
                    {
                        color += attenuation * WallHitColor * incidence;
                    }
                }
                else if (hitType == HitType.HIT_SUN)
                {
                    color += attenuation * SunColor;
                    break;
                }
            }

            return color;
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Tone_mapping
        /// </summary>
        public static Vector3 ReinhardToneMapping(Vector3 v)
            => v / (v + One);


        public int Width { get; set; }
        public int Height { get; set; }
        public int SamplesCount { get; set; }
        public int BounceCount { get; set; }

        public (float, float, float) Eval(int x, int y)
        {
            var color = Zero;
        
            x = Width - 1 - x;
            y = Height - 1 - y;

            for (var p = 0; p < SamplesCount; ++p)
            {
                color += Trace(Position,
                    (Goal + Left *
                        (x - Width / 2 + RandomVal()) + Up *
                        (y - Height / 2 + RandomVal())).Normalize);
            }

            color = color * (1.0f / SamplesCount) + new Vector3(14.0f / 241);
            return ReinhardToneMapping(color);
        }
    }
}