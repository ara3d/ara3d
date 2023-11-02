//
// GLSL textureless classic 3D noise "cnoise",
// with an RSL-style periodic variant "pnoise".
// Author:  Stefan Gustavson (stefan.gustavson@liu.se)
// Version: 2011-10-11
//
// Many thanks to Ian McEwan of Ashima Arts for the
// ideas for permutation and gradient selection.
//
// Copyright (c) 2011 Stefan Gustavson. All rights reserved.
// Distributed under the MIT license. See LICENSE file.
// https://github.com/stegu/webgl-noise
//

using static Unity.Mathematics.math;

namespace Unity.Mathematics
{
    public static partial class noise
    {
        /// <summary>
        /// Classic Perlin noise
        /// </summary>
        /// <param name="P">Point on a 3D grid of gradient vectors.</param>
        /// <returns>Noise value.</returns>
        public static float cnoise(float3 P)
        {
            var Pi0 = floor(P); // Integer part for indexing
            var Pi1 = Pi0 + float3(1.0f); // Integer part + 1
            Pi0 = mod289(Pi0);
            Pi1 = mod289(Pi1);
            var Pf0 = frac(P); // Fractional part for interpolation
            var Pf1 = Pf0 - float3(1.0f); // Fractional part - 1.0
            var ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
            var iy = float4(Pi0.yy, Pi1.yy);
            var iz0 = Pi0.zzzz;
            var iz1 = Pi1.zzzz;

            var ixy = permute(permute(ix) + iy);
            var ixy0 = permute(ixy + iz0);
            var ixy1 = permute(ixy + iz1);

            var gx0 = ixy0 * (1.0f / 7.0f);
            var gy0 = frac(floor(gx0) * (1.0f / 7.0f)) - 0.5f;
            gx0 = frac(gx0);
            var gz0 = float4(0.5f) - abs(gx0) - abs(gy0);
            var sz0 = step(gz0, float4(0.0f));
            gx0 -= sz0 * (step(0.0f, gx0) - 0.5f);
            gy0 -= sz0 * (step(0.0f, gy0) - 0.5f);

            var gx1 = ixy1 * (1.0f / 7.0f);
            var gy1 = frac(floor(gx1) * (1.0f / 7.0f)) - 0.5f;
            gx1 = frac(gx1);
            var gz1 = float4(0.5f) - abs(gx1) - abs(gy1);
            var sz1 = step(gz1, float4(0.0f));
            gx1 -= sz1 * (step(0.0f, gx1) - 0.5f);
            gy1 -= sz1 * (step(0.0f, gy1) - 0.5f);

            var g000 = float3(gx0.x, gy0.x, gz0.x);
            var g100 = float3(gx0.y, gy0.y, gz0.y);
            var g010 = float3(gx0.z, gy0.z, gz0.z);
            var g110 = float3(gx0.w, gy0.w, gz0.w);
            var g001 = float3(gx1.x, gy1.x, gz1.x);
            var g101 = float3(gx1.y, gy1.y, gz1.y);
            var g011 = float3(gx1.z, gy1.z, gz1.z);
            var g111 = float3(gx1.w, gy1.w, gz1.w);

            var norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
            g000 *= norm0.x;
            g010 *= norm0.y;
            g100 *= norm0.z;
            g110 *= norm0.w;
            var norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
            g001 *= norm1.x;
            g011 *= norm1.y;
            g101 *= norm1.z;
            g111 *= norm1.w;

            var n000 = dot(g000, Pf0);
            var n100 = dot(g100, float3(Pf1.x, Pf0.yz));
            var n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
            var n110 = dot(g110, float3(Pf1.xy, Pf0.z));
            var n001 = dot(g001, float3(Pf0.xy, Pf1.z));
            var n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
            var n011 = dot(g011, float3(Pf0.x, Pf1.yz));
            var n111 = dot(g111, Pf1);

            var fade_xyz = fade(Pf0);
            var n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
            var n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
            var n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
            return 2.2f * n_xyz;
        }

        /// <summary>
        /// Classic Perlin noise, periodic variant
        /// </summary>
        /// <param name="P">Point on a 3D grid of gradient vectors.</param>
        /// <param name="rep">Period of repetition.</param>
        /// <returns>Noise value.</returns>
        public static float pnoise(float3 P, float3 rep)
        {
            var Pi0 = fmod(floor(P), rep); // Integer part, math.modulo period
            var Pi1 = fmod(Pi0 + float3(1.0f), rep); // Integer part + 1, math.mod period
            Pi0 = mod289(Pi0);
            Pi1 = mod289(Pi1);
            var Pf0 = frac(P); // Fractional part for interpolation
            var Pf1 = Pf0 - float3(1.0f); // Fractional part - 1.0
            var ix = float4(Pi0.x, Pi1.x, Pi0.x, Pi1.x);
            var iy = float4(Pi0.yy, Pi1.yy);
            var iz0 = Pi0.zzzz;
            var iz1 = Pi1.zzzz;

            var ixy = permute(permute(ix) + iy);
            var ixy0 = permute(ixy + iz0);
            var ixy1 = permute(ixy + iz1);

            var gx0 = ixy0 * (1.0f / 7.0f);
            var gy0 = frac(floor(gx0) * (1.0f / 7.0f)) - 0.5f;
            gx0 = frac(gx0);
            var gz0 = float4(0.5f) - abs(gx0) - abs(gy0);
            var sz0 = step(gz0, float4(0.0f));
            gx0 -= sz0 * (step(0.0f, gx0) - 0.5f);
            gy0 -= sz0 * (step(0.0f, gy0) - 0.5f);

            var gx1 = ixy1 * (1.0f / 7.0f);
            var gy1 = frac(floor(gx1) * (1.0f / 7.0f)) - 0.5f;
            gx1 = frac(gx1);
            var gz1 = float4(0.5f) - abs(gx1) - abs(gy1);
            var sz1 = step(gz1, float4(0.0f));
            gx1 -= sz1 * (step(0.0f, gx1) - 0.5f);
            gy1 -= sz1 * (step(0.0f, gy1) - 0.5f);

            var g000 = float3(gx0.x, gy0.x, gz0.x);
            var g100 = float3(gx0.y, gy0.y, gz0.y);
            var g010 = float3(gx0.z, gy0.z, gz0.z);
            var g110 = float3(gx0.w, gy0.w, gz0.w);
            var g001 = float3(gx1.x, gy1.x, gz1.x);
            var g101 = float3(gx1.y, gy1.y, gz1.y);
            var g011 = float3(gx1.z, gy1.z, gz1.z);
            var g111 = float3(gx1.w, gy1.w, gz1.w);

            var norm0 = taylorInvSqrt(float4(dot(g000, g000), dot(g010, g010), dot(g100, g100), dot(g110, g110)));
            g000 *= norm0.x;
            g010 *= norm0.y;
            g100 *= norm0.z;
            g110 *= norm0.w;
            var norm1 = taylorInvSqrt(float4(dot(g001, g001), dot(g011, g011), dot(g101, g101), dot(g111, g111)));
            g001 *= norm1.x;
            g011 *= norm1.y;
            g101 *= norm1.z;
            g111 *= norm1.w;

            var n000 = dot(g000, Pf0);
            var n100 = dot(g100, float3(Pf1.x, Pf0.yz));
            var n010 = dot(g010, float3(Pf0.x, Pf1.y, Pf0.z));
            var n110 = dot(g110, float3(Pf1.xy, Pf0.z));
            var n001 = dot(g001, float3(Pf0.xy, Pf1.z));
            var n101 = dot(g101, float3(Pf1.x, Pf0.y, Pf1.z));
            var n011 = dot(g011, float3(Pf0.x, Pf1.yz));
            var n111 = dot(g111, Pf1);

            var fade_xyz = fade(Pf0);
            var n_z = lerp(float4(n000, n100, n010, n110), float4(n001, n101, n011, n111), fade_xyz.z);
            var n_yz = lerp(n_z.xy, n_z.zw, fade_xyz.y);
            var n_xyz = lerp(n_yz.x, n_yz.y, fade_xyz.x);
            return 2.2f * n_xyz;
        }
    }
}
