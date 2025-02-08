using System.Numerics;
using System.Runtime.CompilerServices;
using Plato;
using static System.Runtime.CompilerServices.MethodImplOptions;
using Line2D = Plato.Line2D;
using Vector2 = Plato.Vector2;
using Vector3 = Plato.Vector3;

namespace PathTracer
{
    public enum HitType
    {
        HIT_NONE = 0,
        HIT_LETTER = 1,
        HIT_WALL = 2,
        HIT_SUN = 3,
    };

    // Every voxel has a closest point in space. 

    public readonly struct HitResult
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly int Id;

        public HitResult(Vector3 position, Vector3 normal, float distance, int id)
        {
            Position = position;
            Normal = normal;
            Id = id;
        }
    }

    public static class SDFExtensions
    {
        public const float Epsilon = 0.01f;
        public static readonly Vector3 EpsX = Vector3.UnitX * Epsilon;
        public static readonly Vector3 EpsY = Vector3.UnitY * Epsilon;
        public static readonly Vector3 EpsZ = Vector3.UnitZ * Epsilon;

        [MethodImpl(AggressiveInlining)]
        public static Vector3 Normal(this ISignedDistanceField sdf, Vector3 point, float d)
        {
            var n = new Vector3(
                sdf.Distance(point + EpsX) - d,
                sdf.Distance(point + EpsY) - d,
                sdf.Distance(point + EpsZ) - d);
            return n.Normalize;
        }
    }

    public class PathTracerPlato_v2 : IPathTracer
    {
        // https://fabiensanglard.net/postcard_pathtracer/index.html
        // 2 minutes, 58 seconds in C++.
        // divisor = 1, samplesCount = 16;

        public const int MinNoHitCount = 99;
        public const float AttenuationFactor = 0.2f;
        public const float MinMarchingDistance = 0.01f;
        public const float MaxDistance = 100; // 50 makes an interesting effect. Can't see the walls.

        public static readonly Vector3 SunColor = new(50, 80, 100);
        public static readonly Vector3 Position = new(-22, 5, 25);
        public static readonly Vector3 Goal = (new Vector3(-3, 4, 0) + Position * -1).Normalize;
        
        public Vector3 Left => new Vector3(Goal.Z, 0, -Goal.X).Normalize * (1.0f / Width);
        public Vector3 Up => Goal.Cross(Left);

        public static readonly Vector3 WallHitColor = new(500, 400, 100);
        public static readonly Vector3 LightDirection = MakeVector(.6f, .6f, 1f).Normalize;
        private static readonly ThreadLocal<Random> _random = new(() => new Random(99));
        public static Vector3 MakeVector(float a, float b, float c = 0) => new Vector3(a, b, c);
        
        public static float RandomVal() => _random.Value.NextSingle();

        public static readonly SunDistanceField SunDistanceField = new SunDistanceField();
        public static readonly RoomDistanceField RoomDistanceField = new RoomDistanceField();
        public static readonly LettersDistanceField LettersDistanceField = new LettersDistanceField();

        // Sample the world using Signed Distance Fields.
        [MethodImpl(AggressiveInlining)]
        public static float QuerySDF(Vector3 p, out HitType hitType)
        {
            var d = LettersDistanceField.Distance(p);
            hitType = HitType.HIT_LETTER;

            var d2 = RoomDistanceField.Distance(p);
            if (d2 < d)
            {
                d = d2;
                hitType = HitType.HIT_WALL;
            }

            var d3 = SunDistanceField.Distance(p);
            if (d3 < d)
            {
                d = d3;
                hitType = HitType.HIT_SUN;
            }

            return d;
        }

        [MethodImpl(AggressiveInlining)]
        public static Vector3 GetNorm(Vector3 p, float d, HitType hitType)
        {
            if (hitType == HitType.HIT_LETTER)
                return LettersDistanceField.Normal(p, d);

            if (hitType == HitType.HIT_WALL)
                return RoomDistanceField.Normal(p, d);

            return SunDistanceField.Normal(p, d);
        }

        // Perform signed sphere marching
        // Returns hitType 0, 1, 2, or 3 and update hit position/normal
        [MethodImpl(AggressiveInlining)]
        public HitType RayMarching(Vector3 origin, Vector3 dir, ref Vector3 hitPos, ref float d)
        {
            var noHitCount = 0;

            // Signed distance marching
            for (float totalDistance = 0; totalDistance < MaxDistance; totalDistance += d)
            {
                hitPos = origin + dir * totalDistance;
                d = QuerySDF(hitPos, out var hitType);
                if (d < MinMarchingDistance || ++noHitCount > MinNoHitCount)
                {
                    return hitType;
                }
            }

            return HitType.HIT_NONE;
        }

        public static readonly Vector3 Zero = new(0);
        public static readonly Vector3 One = new(1);

        [MethodImpl(AggressiveInlining)]
        public Vector3 Trace(Vector3 origin, Vector3 direction)
        {
            var color = Zero;
            var position = Zero;
            var attenuation = One;

            for (var bounceCount = BounceCount; bounceCount > 0; bounceCount--)
            {
                var d = 0f;
                var hitResult = RayMarching(origin, direction, ref position, ref d);

                if (hitResult == HitType.HIT_NONE)
                    break;

                if (hitResult == HitType.HIT_LETTER)
                {
                    var normal = GetNorm(position, d, hitResult);

                    // Specular bounce on a letter. No color acc.
                    direction += normal * (normal.Dot(direction) * -2);
                    origin = position + direction * 0.1f;
                    attenuation *= AttenuationFactor; // Attenuation via distance traveled.
                }
                else if (hitResult == HitType.HIT_WALL)
                {
                    var normal = GetNorm(position, d, hitResult);

                    // Wall hit uses color yellow?
                    var incidence = normal.Dot(LightDirection);
                    var p = 6.283185f * RandomVal();
                    var c = RandomVal();
                    var s = (1 - c).Sqrt();
                    var g = normal.Z < 0 ? -1f : 1f;
                    var u = -1 / (g + normal.Z);
                    var v = normal.X * normal.Y * u;
                    var (sinP, cosP) = MathF.SinCos(p);
                    direction = MakeVector(v, g + normal.Y.Sqr() * u, -normal.Y) * (cosP * s) +
                                MakeVector(1 + g * normal.X.Sqr() * u, g * v, -g * normal.X) * (sinP * s) +
                                normal * c.Sqrt();
                    origin = position + direction * .1f;
                    attenuation *= AttenuationFactor;

                    if (incidence > 0)
                    {
                        var march = RayMarching(position + normal * .1f, LightDirection, ref position, ref d);
                        if (march == HitType.HIT_SUN)
                        {
                            color += attenuation * WallHitColor * incidence;
                        }
                    }
                }
                else if (hitResult == HitType.HIT_SUN)
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
        [MethodImpl(AggressiveInlining)]
        public static Vector3 ReinhardToneMapping(Vector3 v)
            => v / (v + One);

        public int Width { get; set; }
        public int Height { get; set; }
        public int SamplesCount { get; set; }
        public int BounceCount { get; set; }

        [MethodImpl(AggressiveInlining)]
        public (float, float, float) Eval(int x, int y)
        {
            var color = Zero;
            var halfWidth = Width / 2;
            var halfHeight = Height / 2;
            x = Width - 1 - x;
            y = Height - 1 - y;

            for (var p = 0; p < SamplesCount; ++p)
            {
                color += Trace(Position,
                    (Goal + Left *
                        (x - halfWidth + RandomVal()) + Up *
                        (y - halfHeight + RandomVal())).Normalize);
            }

            color = color * (1.0f / SamplesCount) + new Vector3(14.0f / 241);
            return ReinhardToneMapping(color);
        }
    }
}

