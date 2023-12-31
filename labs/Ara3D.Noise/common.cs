using static Ara3D.Noise.math;

namespace Ara3D.Noise
{
    /// <summary>
    /// A static class containing noise functions.
    /// </summary>
    public static partial class Noise
    {
        // Modulo 289 without a division (only multiplications)
        private static float mod289(float x)
        {
            return x - floor(x * (1.0f / 289.0f)) * 289.0f;
        }

        private static float2 mod289(float2 x)
        {
            return x - floor(x * (1.0f / 289.0f)) * 289.0f;
        }

        private static float3 mod289(float3 x)
        {
            return x - floor(x * (1.0f / 289.0f)) * 289.0f;
        }

        private static float4 mod289(float4 x)
        {
            return x - floor(x * (1.0f / 289.0f)) * 289.0f;
        }

        // Modulo 7 without a division
        private static float3 mod7(float3 x)
        {
            return x - floor(x * (1.0f / 7.0f)) * 7.0f;
        }

        private static float4 mod7(float4 x)
        {
            return x - floor(x * (1.0f / 7.0f)) * 7.0f;
        }

        // Permutation polynomial: (34x^2 + x) math.mod 289
        private static float permute(float x)
        {
            return mod289((34.0f * x + 1.0f) * x);
        }

        private static float3 permute(float3 x)
        {
            return mod289((34.0f * x + 1.0f) * x);
        }

        private static float4 permute(float4 x)
        {
            return mod289((34.0f * x + 1.0f) * x);
        }

        private static float taylorInvSqrt(float r)
        {
            return 1.79284291400159f - 0.85373472095314f * r;
        }

        private static float4 taylorInvSqrt(float4 r)
        {
            return 1.79284291400159f - 0.85373472095314f * r;
        }

        private static float2 fade(float2 t)
        {
            return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
        }

        private static float3 fade(float3 t)
        {   
            return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
        }

        private static float4 fade(float4 t)
        {
            return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
        }

        private static float4 grad4(float j, float4 ip)
        {
            var ones = float4(1.0f, 1.0f, 1.0f, -1.0f);
            var pxyz = floor(frac(float3(j) * ip.xyz) * 7.0f) * ip.z - 1.0f;
            var pw = 1.5f - dot(abs(pxyz), ones.xyz);
            var p = float4(pxyz, pw);
            var s = float4(p.x < 0f ? 1f : 0f, p.y < 0f ? 1f : 0f, p.z < 0f ? 1f : 0f, p.w < 0f ? 1f : 0f);
            p.xyz += (s.xyz * 2.0f - 1.0f) * s.www;
            return p;
        }

        // Hashed 2-D gradients with an extra rotation.
        // (The constant 0.0243902439 is 1/41)
        private static float2 rgrad2(float2 p, float rot)
        {
            // For more isotropic gradients, math.sin/math.cos can be used instead.
            var u = permute(permute(p.x) + p.y) * 0.0243902439f + rot; // Rotate by shift
            u = frac(u) * 6.28318530718f; // 2*pi
            return float2(cos(u), sin(u));
        }
    }
}