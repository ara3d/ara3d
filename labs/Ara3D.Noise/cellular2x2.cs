// Cellular noise ("Worley noise") in 2D in GLSL.
// Copyright (c) Stefan Gustavson 2011-04-19. All rights reserved.
// This code is released under the conditions of the MIT license.
// See LICENSE file for details.
// https://github.com/stegu/webgl-noise

using static Ara3D.Noise.math;

namespace Ara3D.Noise
{
    public static partial class Noise
    {
        /// <summary>
        /// 2D Cellular noise ("Worley noise") with a 2x2 search window.
        /// </summary>
        /// <remarks>
        /// Faster than using 3x3, at the expense of some strong pattern artifacts. F2 is often wrong and has sharp discontinuities. If you need a smooth F2, use the slower 3x3 version. F1 is sometimes wrong, too, but OK for most purposes.
        /// </remarks>
        /// <param name="P">A point in 2D space.</param>
        /// <returns>Feature points. F1 is in the x component, F2 in the y component.</returns>
        public static float2 cellular2x2(float2 P)
        {
            const float K = 0.142857142857f; // 1/7
            const float K2 = 0.0714285714285f; // K/2
            const float jitter = 0.8f; // jitter 1.0 makes F1 wrong more often

            var Pi = mod289(floor(P));
            var Pf = frac(P);
            var Pfx = Pf.x + float4(-0.5f, -1.5f, -0.5f, -1.5f);
            var Pfy = Pf.y + float4(-0.5f, -0.5f, -1.5f, -1.5f);
            var p = permute(Pi.x + float4(0.0f, 1.0f, 0.0f, 1.0f));
            p = permute(p + Pi.y + float4(0.0f, 0.0f, 1.0f, 1.0f));
            var ox = mod7(p) * K + K2;
            var oy = mod7(floor(p * K)) * K + K2;
            var dx = Pfx + jitter * ox;
            var dy = Pfy + jitter * oy;
            var d = dx * dx + dy * dy; // d11, d12, d21 and d22, squared
            // Sort out the two smallest distances
            // Do it right and find both F1 and F2
            d.xy = d.x < d.y ? d.xy : d.yx; // Swap if smaller
            d.xz = d.x < d.z ? d.xz : d.zx;
            d.xw = d.x < d.w ? d.xw : d.wx;
            d.y = min(d.y, d.z);
            d.y = min(d.y, d.w);
            return sqrt(d.xy);
        }
    }
}