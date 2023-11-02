//
// GLSL textureless classic 2D noise "cnoise",
// with an RSL-style periodic variant "pnoise".
// Author:  Stefan Gustavson (stefan.gustavson@liu.se)
// Version: 2011-08-22
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
        /// <param name="P">Point on a 2D grid of gradient vectors.</param>
        /// <returns>Noise value.</returns>
        public static float cnoise(float2 P)
        {
            var Pi = floor(P.xyxy) + float4(0.0f, 0.0f, 1.0f, 1.0f);
            var Pf = frac(P.xyxy) - float4(0.0f, 0.0f, 1.0f, 1.0f);
            Pi = mod289(Pi); // To avoid truncation effects in permutation
            var ix = Pi.xzxz;
            var iy = Pi.yyww;
            var fx = Pf.xzxz;
            var fy = Pf.yyww;

            var i = permute(permute(ix) + iy);

            var gx = frac(i * (1.0f / 41.0f)) * 2.0f - 1.0f;
            var gy = abs(gx) - 0.5f;
            var tx = floor(gx + 0.5f);
            gx = gx - tx;

            var g00 = float2(gx.x, gy.x);
            var g10 = float2(gx.y, gy.y);
            var g01 = float2(gx.z, gy.z);
            var g11 = float2(gx.w, gy.w);

            var norm = taylorInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
            g00 *= norm.x;
            g01 *= norm.y;
            g10 *= norm.z;
            g11 *= norm.w;

            var n00 = dot(g00, float2(fx.x, fy.x));
            var n10 = dot(g10, float2(fx.y, fy.y));
            var n01 = dot(g01, float2(fx.z, fy.z));
            var n11 = dot(g11, float2(fx.w, fy.w));

            var fade_xy = fade(Pf.xy);
            var n_x = lerp(float2(n00, n01), float2(n10, n11), fade_xy.x);
            var n_xy = lerp(n_x.x, n_x.y, fade_xy.y);
            return 2.3f * n_xy;
        }

        /// <summary>
        /// Classic Perlin noise, periodic variant
        /// </summary>
        /// <param name="P">Point on a 2D grid of gradient vectors.</param>
        /// <param name="rep">Period of repetition.</param>
        /// <returns>Noise value.</returns>
        public static float pnoise(float2 P, float2 rep)
        {
            var Pi = floor(P.xyxy) + float4(0.0f, 0.0f, 1.0f, 1.0f);
            var Pf = frac(P.xyxy) - float4(0.0f, 0.0f, 1.0f, 1.0f);
            Pi = fmod(Pi, rep.xyxy); // To create noise with explicit period
            Pi = mod289(Pi); // To avoid truncation effects in permutation
            var ix = Pi.xzxz;
            var iy = Pi.yyww;
            var fx = Pf.xzxz;
            var fy = Pf.yyww;

            var i = permute(permute(ix) + iy);

            var gx = frac(i * (1.0f / 41.0f)) * 2.0f - 1.0f;
            var gy = abs(gx) - 0.5f;
            var tx = floor(gx + 0.5f);
            gx = gx - tx;

            var g00 = float2(gx.x, gy.x);
            var g10 = float2(gx.y, gy.y);
            var g01 = float2(gx.z, gy.z);
            var g11 = float2(gx.w, gy.w);

            var norm = taylorInvSqrt(float4(dot(g00, g00), dot(g01, g01), dot(g10, g10), dot(g11, g11)));
            g00 *= norm.x;
            g01 *= norm.y;
            g10 *= norm.z;
            g11 *= norm.w;

            var n00 = dot(g00, float2(fx.x, fy.x));
            var n10 = dot(g10, float2(fx.y, fy.y));
            var n01 = dot(g01, float2(fx.z, fy.z));
            var n11 = dot(g11, float2(fx.w, fy.w));

            var fade_xy = fade(Pf.xy);
            var n_x = lerp(float2(n00, n01), float2(n10, n11), fade_xy.x);
            var n_xy = lerp(n_x.x, n_x.y, fade_xy.y);
            return 2.3f * n_xy;
        }
    }
}
