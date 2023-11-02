//
// Description : Array and textureless GLSL 2D/3D/4D simplex
//               noise functions.
//      Author : Ian McEwan, Ashima Arts.
//  Maintainer : stegu
//     Lastmath.mod : 20110822 (ijm)
//     License : Copyright (C) 2011 Ashima Arts. All rights reserved.
//               Distributed under the MIT License. See LICENSE file.
//               https://github.com/ashima/webgl-noise
//               https://github.com/stegu/webgl-noise
//

using static Ara3D.Noise.math;

namespace Ara3D.Noise
{
    public static partial class Noise
    {
        /// <summary>
        /// Simplex noise.
        /// </summary>
        /// <param name="v">Input coordinate.</param>
        /// <returns>Noise value.</returns>
        public static float snoise(float3 v)
        {
            var C = float2(1.0f / 6.0f, 1.0f / 3.0f);
            var D = float4(0.0f, 0.5f, 1.0f, 2.0f);

            // First corner
            var i = floor(v + dot(v, C.yyy));
            var x0 = v - i + dot(i, C.xxx);

            // Other corners
            var g = step(x0.yzx, x0.xyz);
            var l = 1.0f - g;
            var i1 = min(g.xyz, l.zxy);
            var i2 = max(g.xyz, l.zxy);

            //   x0 = x0 - 0.0 + 0.0 * C.xxx;
            //   x1 = x0 - i1  + 1.0 * C.xxx;
            //   x2 = x0 - i2  + 2.0 * C.xxx;
            //   x3 = x0 - 1.0 + 3.0 * C.xxx;
            var x1 = x0 - i1 + C.xxx;
            var x2 = x0 - i2 + C.yyy; // 2.0*C.x = 1/3 = C.y
            var x3 = x0 - D.yyy; // -1.0+3.0*C.x = -0.5 = -D.y

            // Permutations
            i = mod289(i);
            var p = permute(permute(permute(
                                        i.z + float4(0.0f, i1.z, i2.z, 1.0f))
                                    + i.y + float4(0.0f, i1.y, i2.y, 1.0f))
                            + i.x + float4(0.0f, i1.x, i2.x, 1.0f));

            // Gradients: 7x7 points over a square, mapped onto an octahedron.
            // The ring size 17*17 = 289 is close to a multiple of 49 (49*6 = 294)
            var n_ = 0.142857142857f; // 1.0/7.0
            var ns = n_ * D.wyz - D.xzx;

            var j = p - 49.0f * floor(p * ns.z * ns.z); //  math.mod(p,7*7)

            var x_ = floor(j * ns.z);
            var y_ = floor(j - 7.0f * x_); // math.mod(j,N)

            var x = x_ * ns.x + ns.yyyy;
            var y = y_ * ns.x + ns.yyyy;
            var h = 1.0f - abs(x) - abs(y);

            var b0 = float4(x.xy, y.xy);
            var b1 = float4(x.zw, y.zw);

            //float4 s0 = float4(math.lessThan(b0,0.0))*2.0 - 1.0;
            //float4 s1 = float4(math.lessThan(b1,0.0))*2.0 - 1.0;
            var s0 = floor(b0) * 2.0f + 1.0f;
            var s1 = floor(b1) * 2.0f + 1.0f;
            var sh = -step(h, float4(0.0f));

            var a0 = b0.xzyw + s0.xzyw * sh.xxyy;
            var a1 = b1.xzyw + s1.xzyw * sh.zzww;

            var p0 = float3(a0.xy, h.x);
            var p1 = float3(a0.zw, h.y);
            var p2 = float3(a1.xy, h.z);
            var p3 = float3(a1.zw, h.w);

            //Normalise gradients
            var norm = taylorInvSqrt(float4(dot(p0, p0), dot(p1, p1), dot(p2, p2), dot(p3, p3)));
            p0 *= norm.x;
            p1 *= norm.y;
            p2 *= norm.z;
            p3 *= norm.w;

            // Mix final noise value
            var m = max(0.6f - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3)), 0.0f);
            m *= m;
            return 42.0f * dot(m * m, float4(dot(p0, x0), dot(p1, x1), dot(p2, x2), dot(p3, x3)));
        }
    }
}