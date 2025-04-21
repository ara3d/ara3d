using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Xml;
using Ara3D.Mathematics;
using Ara3D.Utils;
using Plato;
using static System.Runtime.CompilerServices.MethodImplOptions;
using Line2D = Plato.Line2D;
using Vector2 = Plato.Vector2;
using Vector3 = Plato.Vector3;

namespace PathTracer
{
    public static class SDFExtensionsSIMD
    {
        public const float Epsilon = 0.01f;
        public static readonly Vector3x8 EpsX = new(Vector3.UnitX * Epsilon);
        public static readonly Vector3x8 EpsY = new(Vector3.UnitY * Epsilon);
        public static readonly Vector3x8 EpsZ = new(Vector3.UnitZ * Epsilon);

        [MethodImpl(AggressiveInlining)]
        public static Vector3x8 Normal(this ISignedDistanceField sdf, Vector3x8 point, Vector8 d)
        {
            var n = new Vector3x8(
                sdf.Distance(point + EpsX) - d,
                sdf.Distance(point + EpsY) - d,
                sdf.Distance(point + EpsZ) - d);
            return n.Normalize;
        }

        [MethodImpl(AggressiveInlining)]
        public static Vector8 Pow(this Vector8 self, float f)
            => Vector256.Exp(Vector256.Log(self.Value) * f);
    }


    // A packet of 8 rays. 
    public class TracedRayPacketSIMD
    {
        public Vector3x8 Origin;
        public Vector3x8 Position;
        public Vector3x8 Direction;
        public float[] Distance = new float[8];
        public HitType[] HitType = new HitType[8];
        public Vector3[] Color = new Vector3[8];
        public float[] Attenuation = new float[8];

        public unsafe void SetVector(Vector3x8* p, int i, Vector3 v)
        {
            var px = &p->X;
            var py = &p->Y;
            var pz = &p->Z;
            px[i] = v.X;
            py[i] = v.Y;
            pz[i] = v.Z;
        }

        public unsafe void SetOrigin(int i, Vector3 v)
        {
            fixed (Vector3x8* p = &Origin)
                SetVector(p, i, v);
        }

        public unsafe void SetDirection(int i, Vector3 v)
        {
            fixed (Vector3x8* p = &Origin)
                SetVector(p, i, v);
        }

        public unsafe void SetPosition(int i, Vector3 v)
        {
            fixed (Vector3x8* p = &Origin)
                SetVector(p, i, v);
        }
    }

    public class PathTracerPlatoSIMD : IPathTracer
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

        public static readonly SunDistanceField SunDistanceField = new ();
        public static readonly RoomDistanceField RoomDistanceField = new ();
        public static readonly LettersDistanceField LettersDistanceField = new ();

        // Sample the world using Signed Distance Fields.
        [MethodImpl(AggressiveInlining)]
        public static unsafe Vector8 QuerySDF(in Vector3x8 positions, int[] h)
        {
            var r = new Vector8(1e9f);
            var d = (float*)&r;
            var d1 = LettersDistanceField.Distance(positions);
            var d2 = RoomDistanceField.Distance(positions);
            var d3 = SunDistanceField.Distance(positions);

            for (var i = 0; i < 8; i++)
            {
                d[i] = d1.Value.GetElement(i);
                var d2i = d2.Value.GetElement(i);
                var d3i = d3.Value.GetElement(i);
                h[i] = (int)HitType.HIT_LETTER;

                if (d2i < d[i])
                {
                    d[i] = d2i;
                    h[i] = (int)HitType.HIT_WALL;
                }

                if (d3i < d[i])
                {
                    d[i] = d3i;
                    h[i] = (int)HitType.HIT_SUN;
                }
            }

            return r;
        }

        [MethodImpl(AggressiveInlining)]
        public static unsafe Vector3x8 GetNorm(Vector3x8 p, Vector8 d)
        {
            var xs = p + SDFExtensionsSIMD.EpsX;
            var ys = p + SDFExtensionsSIMD.EpsY;
            var zs = p + SDFExtensionsSIMD.EpsZ;
            var ints = new int[8];
            var dxs = QuerySDF(xs, ints) - d;
            var dys = QuerySDF(ys, ints) - d;
            var dzs = QuerySDF(zs, ints) - d;
            var r = new Vector3x8(dxs, dys, dzs);
            return r.Normalize;
        }

        public static readonly Vector3 Zero = new(0);
        public static readonly Vector3 One = new(1);

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
        public unsafe HitType[] RayMarching(Vector3x8 origin, Vector3x8 dir, ref Vector3x8 hitPos, float[] dValues)
        {
            var r = new int[8];
            var r2 = new HitType[8];

            var terminated = new bool[8];
            var noHitCount = new int[8];

            fixed (Vector3x8* pHitPos = &hitPos)
            {
                Vector8 totalDistance = new Vector8(0);
                Vector8* pTotalDistance = &totalDistance;
                int terminatedCount = 0;
                float* pTotalDistanceFloats = (float*)&pTotalDistance;
                {
                    while (terminatedCount < 8)
                    {
                        hitPos = origin + dir * totalDistance;
                        var d2 = QuerySDF(hitPos, r);

                        for (var i = 0; i < 8; i++)
                        {
                            if (terminated[i])
                                continue;

                            var d = dValues[i] = d2[i];
                            if (d < MinMarchingDistance || ++(noHitCount[i]) > MinNoHitCount)
                            {
                                terminated[i] = true;
                                terminatedCount++;
                                r2[i] = (HitType)r[i];
                            }
                            else
                            {
                                // Have we traveled too far ... no hit then. 
                                if ((pTotalDistanceFloats[i] += d) >= MaxDistance)
                                {
                                    terminatedCount++;
                                    terminated[i] = true;
                                    r2[i] = 0;
                                }
                            }
                        }

                        totalDistance += d2;
                    }
                }
            }

            return r2;
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

        [MethodImpl(AggressiveInlining)]
        public void Trace(ref TracedRayPacketSIMD ray)
        {
            for (var bounceCount = BounceCount; bounceCount > 0; bounceCount--)
            {
                ray.HitType = RayMarching(ray.Origin, ray.Direction, ref ray.Position, ray.Distance);

                for (var i = 0; i < 8; i++)
                {
                    var hitResult = ray.HitType[i];
                    if (hitResult == HitType.HIT_NONE)
                        break;

                    if (hitResult == HitType.HIT_LETTER)
                    {
                        var normal = GetNorm(ray.Position[i], ray.Distance[i] , hitResult);

                        // Specular bounce on a letter. No color acc.
                        var direction = ray.Direction[i];
                        direction += normal * (normal.Dot(direction) * -2);
                        ray.SetDirection(i, direction);
                        var position = ray.Position[i];
                        ray.SetOrigin(i, position + direction * 0.1f);
                        ray.Attenuation[i] *= AttenuationFactor; // Attenuation via distance traveled.
                    }
                    else if (hitResult == HitType.HIT_WALL)
                    {
                        var normal = GetNorm(ray.Position[i], ray.Distance[i], hitResult);

                        // Wall hit uses color yellow?
                        var incidence = normal.Dot(LightDirection);
                        var p = 6.283185f * RandomVal();
                        var c = RandomVal();
                        var s = (1 - c).Sqrt();
                        var g = normal.Z < 0 ? -1f : 1f;
                        var u = -1 / (g + normal.Z);
                        var v = normal.X * normal.Y * u;
                        var (sinP, cosP) = MathF.SinCos(p);
                        var direction = MakeVector(v, g + normal.Y.Sqr() * u, -normal.Y) * (cosP * s) +
                                    MakeVector(1 + g * normal.X.Sqr() * u, g * v, -g * normal.X) * (sinP * s) +
                                    normal * c.Sqrt();

                        var position = ray.Position[i];
                        ray.SetDirection(i, direction);
                        ray.SetOrigin(i, position + direction * .1f);
                        ray.Attenuation[i] *= AttenuationFactor;

                        if (incidence > 0)
                        {
                            var d = ray.Distance[i];
                            var march = RayMarching(position + normal * .1f, LightDirection, ref position, ref d);
                            if (march == HitType.HIT_SUN)
                            {
                                ray.Color[i] += ray.Attenuation[i] * WallHitColor * incidence;
                            }
                            ray.SetPosition(i, position);
                            ray.Distance[i] = d;
                        }
                    }
                    else if (hitResult == HitType.HIT_SUN)
                    {
                        ray.Color[i] += ray.Attenuation[i] * SunColor;
                        break;
                    }
                }
            }
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
                var ray = new TracedRayPacketSIMD();
                ray.Origin = new Vector3x8(Position);
                var directions = new Vector3[8];
                
                for (var i=0; i < directions.Length; i++)
                {
                    directions[i] = (Goal + Left *
                        (x - halfWidth + RandomVal()) + Up *
                        (y - halfHeight + RandomVal())).Normalize;
                }

                Trace(ref ray);

                for (var i=0; i < 8; i++)
                {
                    color += ray.Color[i];
                }
            }

            color = color * (1.0f / (SamplesCount * 8)) + new Vector3(14.0f / 241);
            return ReinhardToneMapping(color);
        }
    }
}