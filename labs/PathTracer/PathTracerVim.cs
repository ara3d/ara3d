using Vim.Math3d;

namespace PathTracer
{
    public class PathTracerVim : IPathTracer
    {
        // https://fabiensanglard.net/postcard_pathtracer/index.html
        // 2 minutes, 58 seconds in C++.
        // divisor = 1, samplesCount = 16;

        public int Width { get; set; }
        public int Height { get; set; }
        public int SamplesCount { get; set; }
        public int BounceCount { get; set; }

        public const int MinNoHitCount = 99;
        public const float SkyHeight = 19.9f;
        public const float AttenuationFactor = 0.2f;
        public const float MinMarchingDistance = 0.01f;
        public const float PlankDistance = 8;
        public const float MaxDistance = 100; // 50 makes an interesting effect. Can't see the walls.

        public static readonly Vector3 SunColor = (50, 80, 100);
        public static readonly Vector3 Position = (-22, 5, 25);
        public static readonly Vector3 Goal = (new Vector3(-3, 4, 0) + Position * -1).Normalize();
        public Vector3 Left => new Vector3(Goal.Z, 0, -Goal.X).Normalize() * (1.0f / Width);
        public Vector3 Up => Goal.Cross(Left);
        public static readonly Vector3 WallHitColor = (500, 400, 100);

        public static readonly AABox LowerRoomBounds = (MakeVector(-30, -0.5f, -30), MakeVector(30, 18, 30));
        public static readonly AABox UpperRoomBounds = (MakeVector(-25, 17, -25), MakeVector(25, 20, 25));
        public static readonly AABox CeilingBounds = (MakeVector(1.5f, 18.5f, -25), MakeVector(6.5f, 20, 25));

        public static readonly Vector3 LightDirection = MakeVector(.6f, .6f, 1f).Normalize();

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
            ((-26, 0), (-26, 16)),
            ((-26, 8), (-22, 8)),
            ((-26, 16), (-22, 16)),
            // I 
            ((-14, 0), (-10, 0)),
            ((-12, 0), (-12, 16)),
            ((-14, 16), (-10, 16)),
            // X 
            ((-6, 0), (2, 16)),
            ((-6, 16), (2, 0)),
            // A 
            ((6, 0), (10, 16)),
            ((10, 16), (14, 0)),
            ((8, 8), (12, 8)),
            // R without curve
            ((18, 0), (18, 16)),
            ((18, 8), (22, 8)),
            ((18, 16), (22, 16)),
            ((20, 8), (26, 0)),
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
                var o = f - (a + b * Min(-Min((a - f).Dot(b) / b.Dot(b), 0), 1));
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
                    distance = Min(distance, (o.Dot(o).Sqrt() - 2).Abs());
                }
                else
                {
                    o = o.SetY(o.Y > 0 ? o.Y - 2 : o.Y + 2);
                    distance = Min(distance, o.Dot(o).Sqrt());
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
                        MakeVector(position.X.Abs() % PlankDistance,
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
                            .Normalize();
                    return hitType;
                }

            return 0;
        }

        public Vector3 Trace(Vector3 origin, Vector3 direction)
        {
            var sampledPosition = Vector3.Zero;
            var normal = Vector3.Zero;
            var color = Vector3.Zero;
            var attenuation = Vector3.One;

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
                    direction = MakeVector(v, g + normal.Y.Sqr() * u, -normal.Y) * (p.Cos() * s) +
                                MakeVector(1 + g * normal.X * normal.X * u, g * v, -g * normal.X) * (p.Sin() * s) +
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
            => v / (v + new Vector3(1));

        public (float, float, float) Eval(int x, int y)
        {
            var color = Vector3.Zero;
        
            x = Width - 1 - x;
            y = Height - 1 - y;

            for (var p = 0; p < SamplesCount; ++p)
            {
                color += Trace(Position,
                    (Goal + Left *
                        (x - Width / 2 + RandomVal()) + Up *
                        (y - Height / 2 + RandomVal())).Normalize());
            }

            color = color * (1.0f / SamplesCount) + 14.0f / 241;
            return ReinhardToneMapping(color);
        }
    }
}