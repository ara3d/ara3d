using System.Runtime.CompilerServices;
using Ara3D.Utils;
using Plato;

namespace PathTracer
{
    public class WavefrontPathTracer : IPathTracer
    {
        public Wavefront MainRays;
        public Wavefront DiffuseRays;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HitType RayMarching(Vector3 origin, Vector3 dir, out float d, out Vector3 hitPos)
        {
            var noHitCount = 0;
            d = 0f;
            hitPos = origin;

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

        public void UpdateTracedRay(Wavefront.Ray ray)
        {
            if (!ray.Active)
                return;

            if (ray.HitId == 0)
                ray.Active = false;

            if (ray.HitId == (int)HitType.HIT_LETTER)
            {
                // The letter is treated as a purely specular (reflective) surface.
                // Specular bounce on a letter. No color accumulation.
                var normal = GetNorm(ray.HitPosition, ray.HitDistance, (HitType)ray.HitId);
                ray.Direction += normal * (normal.Dot(ray.Direction) * -2);
                ray.Origin = ray.HitPosition + ray.Direction * 0.1f;
                ray.Attenuation *= AttenuationFactor; // Attenuation via distance traveled.
                return;
            }

            if (ray.HitId == (int)HitType.HIT_WALL)
            {
                // The wall is treated as diffuse.
                // After computing a random diffuse reflection direction for the next bounce, the code does a quick check to see if the sun is directly visible from that point (a “shadow ray” or “light test”).
                // If there’s a clear line of sight(i.e.RayMarching returns HIT_SUN), the code adds direct sunlight(color += attenuation * WallHitColor * incidence).
                // This is a standard “next -event estimation” technique: it explicitly checks whether the light (the sun) is visible from a diffuse surface.
                var normal = GetNorm(ray.HitPosition, ray.HitDistance, (HitType)ray.HitId);

                var incidence = normal.Dot(LightDirection);
                var p = 6.283185f * RandomVal();
                var c = RandomVal();
                var s = (1 - c).Sqrt();
                var g = normal.Z < 0 ? -1f : 1f;
                var u = -1 / (g + normal.Z);
                var v = normal.X * normal.Y * u;
                var (sinP, cosP) = MathF.SinCos(p);
                
                // Random bounced direction 
                ray.Direction = MakeVector(v, g + normal.Y.Sqr() * u, -normal.Y) * (cosP * s) +
                            MakeVector(1 + g * normal.X.Sqr() * u, g * v, -g * normal.X) * (sinP * s) +
                            normal * c.Sqrt();
                ray.Origin = ray.HitPosition + ray.Direction * .1f;
                ray.Attenuation *= AttenuationFactor;

                if (incidence > 0)
                {
                    var march = RayMarching(ray.HitPosition + normal * .1f, LightDirection, out _, out _);
                    if (march == HitType.HIT_SUN)
                    {
                        ray.Color += ray.Attenuation * WallHitColor * incidence;
                    }
                }
            }
            
            // If we hit the sun, set the color and terminate. 
            if (ray.HitId == (int)HitType.HIT_SUN)
            {
                ray.Color += ray.Attenuation * SunColor;
                ray.Active = false;
            }
        }

        public void RayMarching(Wavefront wavefront)
        {
            var ray = wavefront.GetRay();

            for (var i = 0; i < wavefront.Count; i++)
            {
                ray.Index = i;
                if (!ray.Active)
                    continue;
                ray.HitId = (int)RayMarching(ray.Origin, ray.Direction, out var d, out var pos);
                ray.HitPosition = pos;
                ray.HitDistance = d;
            }
        }

        public void UpdateRays(Wavefront wavefront)
        {
            var ray = wavefront.GetRay();
            for (var i = 0; i < wavefront.Count; i++)
            {
                ray.Index = i;
                UpdateTracedRay(ray);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Trace(Wavefront wavefront)
        {
            for (var bounceCount = BounceCount; bounceCount > 0; bounceCount--)
            {
                RayMarching(wavefront);
                UpdateRays(wavefront);
            }
        }

        /// <summary>
        /// https://en.wikipedia.org/wiki/Tone_mapping
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ReinhardToneMapping(Vector3 v)
            => v / (v + One);

        public int Width { get; set; }
        public int Height { get; set; }
        public int SamplesCount { get; set; }
        public int BounceCount { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (float, float, float) Eval(int x, int y)
        {
            var color = Zero;
            var halfWidth = Width / 2;
            var halfHeight = Height / 2;
            x = Width - 1 - x;
            y = Height - 1 - y;

            var wavefront = new Wavefront();
            
            // Add the rays 
            for (var p = 0; p < SamplesCount; ++p)
            {
                var dir = (Goal + Left *
                    (x - halfWidth + RandomVal()) + Up *
                    (y - halfHeight + RandomVal())).Normalize;
                wavefront.AddRay(new Ray3D(Position, dir));
            }

            // Trace the set of rays 
            Trace(wavefront);

            for (var i = 0; i < wavefront.Count; i++)
            {
                color += wavefront.GetColor(i);
            }

            color = color * (1.0f / SamplesCount) + new Vector3(14.0f / 241);
            return ReinhardToneMapping(color);
        }
    }
}