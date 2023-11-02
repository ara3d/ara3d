using System;
using System.Runtime.CompilerServices;
// ReSharper disable InconsistentNaming

namespace Ara3D.Noise
{
  public static partial class math
    {
        /// <summary>The mathematical constant e also known as Euler's number. Approximately 2.72. This is a f64/double precision constant.</summary>
        public const double E_DBL = 2.71828182845904523536;

        /// <summary>The mathematical constant pi. Approximately 3.14. This is a f64/double precision constant.</summary>
        public const double PI_DBL = 3.14159265358979323846;

        /// <summary>
        /// The mathematical constant (2 * pi). Approximately 6.28. This is a f64/double precision constant. Also known as <see cref="TAU_DBL"/>.
        /// </summary>
        public const double PI2_DBL = PI_DBL * 2.0;

        /// <summary>
        /// The conversion constant used to convert radians to degrees. Multiply the radian value by this constant to get degrees.
        /// </summary>
        /// <remarks>Multiplying by this constant is equivalent to using <see cref="math.degrees(double)"/>.</remarks>
        public const double TODEGREES_DBL = 57.29577951308232;

        /// <summary>
        /// The conversion constant used to convert degrees to radians. Multiply the degree value by this constant to get radians.
        /// </summary>
        /// <remarks>Multiplying by this constant is equivalent to using <see cref="math.radians(double)"/>.</remarks>
        public const double TORADIANS_DBL = 0.017453292519943296;

        /// <summary>The mathematical constant e also known as Euler's number. Approximately 2.72.</summary>
        public const float E = (float)E_DBL;

   /// <summary>The mathematical constant pi. Approximately 3.14.</summary>
        public const float PI = (float)PI_DBL;

        /// <summary>
        /// The mathematical constant (2 * pi). Approximately 6.28. Also known as <see cref="TAU"/>.
        /// </summary>
        public const float PI2 = (float)PI2_DBL;
        

        /// <summary>
        /// The mathematical constant tau. Approximately 6.28. Also known as <see cref="PI2"/>.
        /// </summary>
        public const float TAU = (float)PI2_DBL;

        /// <summary>
        /// The conversion constant used to convert radians to degrees. Multiply the radian value by this constant to get degrees.
        /// </summary>
        /// <remarks>Multiplying by this constant is equivalent to using <see cref="math.degrees(float)"/>.</remarks>
        public const float TODEGREES = (float)TODEGREES_DBL;

        /// <summary>
        /// The conversion constant used to convert degrees to radians. Multiply the degree value by this constant to get radians.
        /// </summary>
        /// <remarks>Multiplying by this constant is equivalent to using <see cref="math.radians(float)"/>.</remarks>
        public const float TORADIANS = (float)TORADIANS_DBL;

    
        /// <summary>
        /// Single precision constant for Not a Number.
        ///
        /// NAN is considered unordered, which means all comparisons involving it are false except for not equal (operator !=).
        /// As a consequence, NAN == NAN is false but NAN != NAN is true.
        ///
        /// Additionally, there are multiple bit representations for Not a Number, so if you must test if your value
        /// is NAN, use isnan().
        /// </summary>
        public const float NAN = float.NaN;

        
        /// <summary>Returns the bit pattern of a float as a uint.</summary>
        /// <param name="x">The float bits to copy.</param>
        /// <returns>The uint with the same bit pattern as the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint asuint(float x)
        {
            unsafe
            {
                return *(uint*)&x;
            }
        }



        /// <summary>Returns the bit pattern of a double as a ulong.</summary>
        /// <param name="x">The double bits to copy.</param>
        /// <returns>The ulong with the same bit pattern as the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong asulong(double x)
        {
            unsafe
            {
                return *(ulong*)&x;
            }
        }

        /// <summary>Returns the bit pattern of a uint as a float.</summary>
        /// <param name="x">The uint bits to copy.</param>
        /// <returns>The float with the same bit pattern as the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float asfloat(uint x)
        {
            unsafe
            {
                return *(float*)&x;
            }
        }
        

        /// <summary>Returns the bit pattern of a ulong as a double.</summary>
        /// <param name="x">The ulong bits to copy.</param>
        /// <returns>The double with the same bit pattern as the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double asdouble(ulong x)
        {
            unsafe
            {
                return *(double*)&x;
            }
        }
        

        /// <summary>Returns the minimum of two int values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int min(int x, int y)
        {
            return x < y ? x : y;
        }


        /// <summary>Returns the minimum of two uint values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint min(uint x, uint y)
        {
            return x < y ? x : y;
        }


        /// <summary>Returns the minimum of two long values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long min(long x, long y)
        {
            return x < y ? x : y;
        }


        /// <summary>Returns the minimum of two ulong values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong min(ulong x, ulong y)
        {
            return x < y ? x : y;
        }


        /// <summary>Returns the minimum of two float values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float min(float x, float y)
        {
            return float.IsNaN(y) || x < y ? x : y;
        }

        /// <summary>Returns the componentwise minimum of two float2 vectors.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The componentwise minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 min(float2 x, float2 y)
        {
            return new float2(min(x.x, y.x), min(x.y, y.y));
        }

        /// <summary>Returns the componentwise minimum of two float3 vectors.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The componentwise minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 min(float3 x, float3 y)
        {
            return new float3(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z));
        }

        /// <summary>Returns the componentwise minimum of two float4 vectors.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The componentwise minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 min(float4 x, float4 y)
        {
            return new float4(min(x.x, y.x), min(x.y, y.y), min(x.z, y.z), min(x.w, y.w));
        }


        /// <summary>Returns the minimum of two double values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The minimum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double min(double x, double y)
        {
            return double.IsNaN(y) || x < y ? x : y;
        }


        /// <summary>Returns the maximum of two int values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int max(int x, int y)
        {
            return x > y ? x : y;
        }


        /// <summary>Returns the maximum of two uint values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint max(uint x, uint y)
        {
            return x > y ? x : y;
        }


        /// <summary>Returns the maximum of two long values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long max(long x, long y)
        {
            return x > y ? x : y;
        }


        /// <summary>Returns the maximum of two ulong values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong max(ulong x, ulong y)
        {
            return x > y ? x : y;
        }


        /// <summary>Returns the maximum of two float values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float max(float x, float y)
        {
            return float.IsNaN(y) || x > y ? x : y;
        }

        /// <summary>Returns the componentwise maximum of two float2 vectors.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The componentwise maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 max(float2 x, float2 y)
        {
            return new float2(max(x.x, y.x), max(x.y, y.y));
        }

        /// <summary>Returns the componentwise maximum of two float3 vectors.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The componentwise maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 max(float3 x, float3 y)
        {
            return new float3(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z));
        }

        /// <summary>Returns the componentwise maximum of two float4 vectors.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The componentwise maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 max(float4 x, float4 y)
        {
            return new float4(max(x.x, y.x), max(x.y, y.y), max(x.z, y.z), max(x.w, y.w));
        }


        /// <summary>Returns the maximum of two double values.</summary>
        /// <param name="x">The first input value.</param>
        /// <param name="y">The second input value.</param>
        /// <returns>The maximum of the two input values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double max(double x, double y)
        {
            return double.IsNaN(y) || x > y ? x : y;
        }

        /// <summary>Returns the result of linearly interpolating from start to end using the interpolation parameter t.</summary>
        /// <remarks>
        /// If the interpolation parameter is not in the range [0, 1], then this function extrapolates.
        /// </remarks>
        /// <param name="start">The start point, corresponding to the interpolation parameter value of 0.</param>
        /// <param name="end">The end point, corresponding to the interpolation parameter value of 1.</param>
        /// <param name="t">The interpolation parameter. May be a value outside the interval [0, 1].</param>
        /// <returns>The interpolation from start to end.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float lerp(float start, float end, float t)
        {
            return start + t * (end - start);
        }

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the interpolation parameter t.</summary>
        /// <remarks>
        /// If the interpolation parameter is not in the range [0, 1], then this function extrapolates.
        /// </remarks>
        /// <param name="start">The start point, corresponding to the interpolation parameter value of 0.</param>
        /// <param name="end">The end point, corresponding to the interpolation parameter value of 1.</param>
        /// <param name="t">The interpolation parameter. May be a value outside the interval [0, 1].</param>
        /// <returns>The componentwise interpolation from x to y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 lerp(float2 start, float2 end, float t)
        {
            return start + t * (end - start);
        }
        

        /// <summary>Returns the result of a componentwise linear interpolating from x to y using the interpolation parameter t.</summary>
        /// <remarks>
        /// If the interpolation parameter is not in the range [0, 1], then this function extrapolates.
        /// </remarks>
        /// <param name="start">The start point, corresponding to the interpolation parameter value of 0.</param>
        /// <param name="end">The end point, corresponding to the interpolation parameter value of 1.</param>
        /// <param name="t">The interpolation parameter. May be a value outside the interval [0, 1].</param>
        /// <returns>The componentwise interpolation from x to y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 lerp(float4 start, float4 end, float t)
        {
            return start + t * (end - start);
        }


      
        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 int values.</summary>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int mad(int mulA, int mulB, int addC)
        {
            return mulA * mulB + addC;
        }


        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 uint values.</summary>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint mad(uint mulA, uint mulB, uint addC)
        {
            return mulA * mulB + addC;
        }

        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 long values.</summary>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long mad(long mulA, long mulB, long addC)
        {
            return mulA * mulB + addC;
        }


        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 ulong values.</summary>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong mad(ulong mulA, ulong mulB, ulong addC)
        {
            return mulA * mulB + addC;
        }


        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 float values.</summary>
        /// <remarks>
        /// When Burst compiled with fast math enabled on some architectures, this could be converted to a fused multiply add (FMA).
        /// FMA is more accurate due to rounding once at the end of the computation rather than twice that is required when
        /// this computation is not fused.
        /// </remarks>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float mad(float mulA, float mulB, float addC)
        {
            return mulA * mulB + addC;
        }

        /// <summary>Returns the result of a componentwise multiply-add operation (a * b + c) on 3 float2 vectors.</summary>
        /// <remarks>
        /// When Burst compiled with fast math enabled on some architectures, this could be converted to a fused multiply add (FMA).
        /// FMA is more accurate due to rounding once at the end of the computation rather than twice that is required when
        /// this computation is not fused.
        /// </remarks>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The componentwise multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 mad(float2 mulA, float2 mulB, float2 addC)
        {
            return mulA * mulB + addC;
        }

        /// <summary>Returns the result of a componentwise multiply-add operation (a * b + c) on 3 float3 vectors.</summary>
        /// <remarks>
        /// When Burst compiled with fast math enabled on some architectures, this could be converted to a fused multiply add (FMA).
        /// FMA is more accurate due to rounding once at the end of the computation rather than twice that is required when
        /// this computation is not fused.
        /// </remarks>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The componentwise multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 mad(float3 mulA, float3 mulB, float3 addC)
        {
            return mulA * mulB + addC;
        }

        /// <summary>Returns the result of a componentwise multiply-add operation (a * b + c) on 3 float4 vectors.</summary>
        /// <remarks>
        /// When Burst compiled with fast math enabled on some architectures, this could be converted to a fused multiply add (FMA).
        /// FMA is more accurate due to rounding once at the end of the computation rather than twice that is required when
        /// this computation is not fused.
        /// </remarks>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The componentwise multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 mad(float4 mulA, float4 mulB, float4 addC)
        {
            return mulA * mulB + addC;
        }


        /// <summary>Returns the result of a multiply-add operation (a * b + c) on 3 double values.</summary>
        /// <remarks>
        /// When Burst compiled with fast math enabled on some architectures, this could be converted to a fused multiply add (FMA).
        /// FMA is more accurate due to rounding once at the end of the computation rather than twice that is required when
        /// this computation is not fused.
        /// </remarks>
        /// <param name="mulA">First value to multiply.</param>
        /// <param name="mulB">Second value to multiply.</param>
        /// <param name="addC">Third value to add to the product of a and b.</param>
        /// <returns>The multiply-add of the inputs.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double mad(double mulA, double mulB, double addC)
        {
            return mulA * mulB + addC;
        }


        /// <summary>Returns the result of a componentwise clamping of the value valueToClamp into the interval (inclusive) [lowerBound, upperBound], where valueToClamp, lowerBound and upperBound are float4 vectors.</summary>
        /// <param name="valueToClamp">Input value to be clamped.</param>
        /// <param name="lowerBound">Lower bound of the interval.</param>
        /// <param name="upperBound">Upper bound of the interval.</param>
        /// <returns>The componentwise clamping of the input valueToClamp into the interval (inclusive) [lowerBound, upperBound].</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 clamp(float4 valueToClamp, float4 lowerBound, float4 upperBound)
        {
            return max(lowerBound, min(upperBound, valueToClamp));
        }
        

        /// <summary>Returns the absolute value of a int value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int abs(int x)
        {
            return max(-x, x);
        }


        /// <summary>Returns the absolute value of a long value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long abs(long x)
        {
            return max(-x, x);
        }


        /// <summary>Returns the absolute value of a float value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float abs(float x)
        {
            return asfloat(asuint(x) & 0x7FFFFFFF);
        }

        /// <summary>Returns the componentwise absolute value of a float2 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 abs(float2 x)
        {
            return new float2(abs(x.x), abs(x.y));
        }

        /// <summary>Returns the componentwise absolute value of a float3 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 abs(float3 x)
        {
            return new float3(abs(x.x), abs(x.y), abs(x.z)); 
        }

        /// <summary>Returns the componentwise absolute value of a float4 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 abs(float4 x)
        {
            return new float4(abs(x.x), abs(x.y), abs(x.z), abs(x.w));
        }


        /// <summary>Returns the absolute value of a double value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The absolute value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double abs(double x)
        {
            return asdouble(asulong(x) & 0x7FFFFFFFFFFFFFFF);
        }

        /// <summary>Returns the dot product of two int values. Equivalent to multiplication.</summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The dot product of two values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int dot(int x, int y)
        {
            return x * y;
        }


        /// <summary>Returns the dot product of two uint values. Equivalent to multiplication.</summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The dot product of two values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint dot(uint x, uint y)
        {
            return x * y;
        }

        /// <summary>Returns the dot product of two float values. Equivalent to multiplication.</summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The dot product of two values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float dot(float x, float y)
        {
            return x * y;
        }

        /// <summary>Returns the dot product of two float2 vectors.</summary>
        /// <param name="x">The first vector.</param>
        /// <param name="y">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float dot(float2 x, float2 y)
        {
            return x.x * y.x + x.y * y.y;
        }

        /// <summary>Returns the dot product of two float3 vectors.</summary>
        /// <param name="x">The first vector.</param>
        /// <param name="y">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float dot(float3 x, float3 y)
        {
            return x.x * y.x + x.y * y.y + x.z * y.z;
        }

        /// <summary>Returns the dot product of two float4 vectors.</summary>
        /// <param name="x">The first vector.</param>
        /// <param name="y">The second vector.</param>
        /// <returns>The dot product of two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float dot(float4 x, float4 y)
        {
            return x.x * y.x + x.y * y.y + x.z * y.z + x.w * y.w;
        }


        /// <summary>Returns the dot product of two double values. Equivalent to multiplication.</summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The dot product of two values.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double dot(double x, double y)
        {
            return x * y;
        }

        /// <summary>Returns the tangent of a float value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The tangent of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float tan(float x)
        {
            return (float)Math.Tan(x);
        }

        /// <summary>Returns the componentwise tangent of a float2 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise tangent of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 tan(float2 x)
        {
            return new float2(tan(x.x), tan(x.y));
        }

        /// <summary>Returns the componentwise tangent of a float3 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise tangent of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 tan(float3 x)
        {
            return new float3(tan(x.x), tan(x.y), tan(x.z));
        }

        /// <summary>Returns the componentwise tangent of a float4 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise tangent of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 tan(float4 x)
        {
            return new float4(tan(x.x), tan(x.y), tan(x.z), tan(x.w));
        }


        /// <summary>Returns the tangent of a double value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The tangent of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double tan(double x)
        {
            return Math.Tan(x);
        }

        /// <summary>Returns the arctangent of a float value.</summary>
        /// <param name="x">A tangent value, usually the ratio y/x on the unit circle.</param>
        /// <returns>The arctangent of the input, in radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float atan(float x)
        {
            return (float)Math.Atan(x);
        }

        /// <summary>Returns the componentwise arctangent of a float2 vector.</summary>
        /// <param name="x">A tangent value, usually the ratio y/x on the unit circle.</param>
        /// <returns>The componentwise arctangent of the input, in radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 atan(float2 x)
        {
            return new float2(atan(x.x), atan(x.y));
        }

        /// <summary>Returns the componentwise arctangent of a float3 vector.</summary>
        /// <param name="x">A tangent value, usually the ratio y/x on the unit circle.</param>
        /// <returns>The componentwise arctangent of the input, in radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 atan(float3 x)
        {
            return new float3(atan(x.x), atan(x.y), atan(x.z));
        }

        /// <summary>Returns the componentwise arctangent of a float4 vector.</summary>
        /// <param name="x">A tangent value, usually the ratio y/x on the unit circle.</param>
        /// <returns>The componentwise arctangent of the input, in radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 atan(float4 x)
        {
            return new float4(atan(x.x), atan(x.y), atan(x.z), atan(x.w));
        }


        /// <summary>Returns the arctangent of a double value.</summary>
        /// <param name="x">A tangent value, usually the ratio y/x on the unit circle.</param>
        /// <returns>The arctangent of the input, in radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double atan(double x)
        {
            return Math.Atan(x);
        }

        /// <summary>Returns the cosine of a float value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The cosine cosine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float cos(float x)
        {
            return (float)Math.Cos(x);
        }

        /// <summary>Returns the componentwise cosine of a float2 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise cosine cosine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 cos(float2 x)
        {
            return new float2(cos(x.x), cos(x.y));
        }

        /// <summary>Returns the componentwise cosine of a float3 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise cosine cosine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 cos(float3 x)
        {
            return new float3(cos(x.x), cos(x.y), cos(x.z));
        }

        /// <summary>Returns the componentwise cosine of a float4 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise cosine cosine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 cos(float4 x)
        {
            return new float4(cos(x.x), cos(x.y), cos(x.z), cos(x.w));
        }


        /// <summary>Returns the cosine of a double value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The cosine cosine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double cos(double x)
        {
            return Math.Cos(x);
        }

      

        /// <summary>Returns the sine of a float value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The sine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float sin(float x)
        {
            return (float)Math.Sin(x);
        }

        /// <summary>Returns the componentwise sine of a float2 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise sine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 sin(float2 x)
        {
            return new float2(sin(x.x), sin(x.y));
        }

        /// <summary>Returns the componentwise sine of a float3 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise sine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 sin(float3 x)
        {
            return new float3(sin(x.x), sin(x.y), sin(x.z));
        }

        /// <summary>Returns the componentwise sine of a float4 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise sine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 sin(float4 x)
        {
            return new float4(sin(x.x), sin(x.y), sin(x.z), sin(x.w));
        }


        /// <summary>Returns the sine of a double value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The sine of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double sin(double x)
        {
            return Math.Sin(x);
        }


        
        /// <summary>Returns the result of rounding a float value up to the nearest integral value less or equal to the original value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The round down to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float floor(float x)
        {
            return (float)Math.Floor(x);
        }

        /// <summary>Returns the result of rounding each component of a float2 vector value down to the nearest value less or equal to the original value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise round down to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 floor(float2 x)
        {
            return new float2(floor(x.x), floor(x.y));
        }

        /// <summary>Returns the result of rounding each component of a float3 vector value down to the nearest value less or equal to the original value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise round down to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 floor(float3 x)
        {
            return new float3(floor(x.x), floor(x.y), floor(x.z));
        }

        /// <summary>Returns the result of rounding each component of a float4 vector value down to the nearest value less or equal to the original value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise round down to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 floor(float4 x)
        {
            return new float4(floor(x.x), floor(x.y), floor(x.z), floor(x.w));
        }

        /// <summary>Returns the result of rounding a float value to the nearest integral value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The round to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float round(float x)
        {
            return (float)Math.Round(x);
        }

        /// <summary>Returns the result of rounding each component of a float2 vector value to the nearest integral value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise round to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 round(float2 x)
        {
            return new float2(round(x.x), round(x.y));
        }

        /// <summary>Returns the result of rounding each component of a float3 vector value to the nearest integral value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise round to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 round(float3 x)
        {
            return new float3(round(x.x), round(x.y), round(x.z));
        }

        /// <summary>Returns the result of rounding each component of a float4 vector value to the nearest integral value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise round to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 round(float4 x)
        {
            return new float4(round(x.x), round(x.y), round(x.z), round(x.w));
        }


        /// <summary>Returns the result of rounding a double value to the nearest integral value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The round to nearest integral value of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double round(double x)
        {
            return Math.Round(x);
        }

        /// <summary>Returns the fractional part of a float value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The fractional part of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float frac(float x)
        {
            return x - floor(x);
        }

        /// <summary>Returns the componentwise fractional parts of a float2 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise fractional part of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 frac(float2 x)
        {
            return x - floor(x);
        }

        /// <summary>Returns the componentwise fractional parts of a float3 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise fractional part of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 frac(float3 x)
        {
            return x - floor(x);
        }

        /// <summary>Returns the componentwise fractional parts of a float4 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise fractional part of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 frac(float4 x)
        {
            return x - floor(x);
        }
        

        /// <summary>Returns the sign of a int value. -1 if it is less than zero, 0 if it is zero and 1 if it greater than zero.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The sign of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int sign(int x)
        {
            return (x > 0 ? 1 : 0) - (x < 0 ? 1 : 0);
        }

        /// <summary>Returns the sign of a float value. -1.0f if it is less than zero, 0.0f if it is zero and 1.0f if it greater than zero.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The sign of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float sign(float x)
        {
            return (x > 0.0f ? 1.0f : 0.0f) - (x < 0.0f ? 1.0f : 0.0f);
        }

        /// <summary>Returns the componentwise sign of a float2 value. 1.0f for positive components, 0.0f for zero components and -1.0f for negative components.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise sign of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 sign(float2 x)
        {
            return new float2(sign(x.x), sign(x.y));
        }

        /// <summary>Returns the componentwise sign of a float3 value. 1.0f for positive components, 0.0f for zero components and -1.0f for negative components.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise sign of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 sign(float3 x)
        {
            return new float3(sign(x.x), sign(x.y), sign(x.z));
        }

        /// <summary>Returns the componentwise sign of a float4 value. 1.0f for positive components, 0.0f for zero components and -1.0f for negative components.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise sign of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 sign(float4 x)
        {
            return new float4(sign(x.x), sign(x.y), sign(x.z), sign(x.w));
        }


        /// <summary>Returns the sign of a double value. -1.0 if it is less than zero, 0.0 if it is zero and 1.0 if it greater than zero.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The sign of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double sign(double x)
        {
            return x == 0 ? 0 : (x > 0.0 ? 1.0 : 0.0) - (x < 0.0 ? 1.0 : 0.0);
        }


        /// <summary>Returns the base-e exponential of x.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The base-e exponential of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float exp(float x)
        {
            return (float)Math.Exp(x);
        }

        /// <summary>Returns the componentwise base-e exponential of x.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise base-e exponential of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 exp(float2 x)
        {
            return new float2(exp(x.x), exp(x.y));
        }

        /// <summary>Returns the componentwise base-e exponential of x.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise base-e exponential of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 exp(float3 x)
        {
            return new float3(exp(x.x), exp(x.y), exp(x.z));
        }

        /// <summary>Returns the componentwise base-e exponential of x.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise base-e exponential of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 exp(float4 x)
        {
            return new float4(exp(x.x), exp(x.y), exp(x.z), exp(x.w));
        }

        /// <summary>Returns the base-e exponential of x.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The base-e exponential of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double exp(double x)
        {
            return Math.Exp(x);
        }

        /// <summary>Returns the natural logarithm of a float value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The natural logarithm of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float log(float x)
        {
            return (float)Math.Log(x);
        }

        /// <summary>Returns the componentwise natural logarithm of a float2 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise natural logarithm of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 log(float2 x)
        {
            return new float2(log(x.x), log(x.y));
        }

        /// <summary>Returns the componentwise natural logarithm of a float3 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise natural logarithm of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 log(float3 x)
        {
            return new float3(log(x.x), log(x.y), log(x.z));
        }

        /// <summary>Returns the componentwise natural logarithm of a float4 vector.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The componentwise natural logarithm of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 log(float4 x)
        {
            return new float4(log(x.x), log(x.y), log(x.z), log(x.w));
        }


        /// <summary>Returns the natural logarithm of a double value.</summary>
        /// <param name="x">Input value.</param>
        /// <returns>The natural logarithm of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double log(double x)
        {
            return Math.Log(x);
        }


     
        /// <summary>Returns the componentwise floating point remainder of x/y.</summary>
        /// <param name="x">The dividend in x/y.</param>
        /// <param name="y">The divisor in x/y.</param>
        /// <returns>The componentwise remainder of x/y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 fmod(float3 x, float3 y)
        {
            return new float3(x.x % y.x, x.y % y.y, x.z % y.z);
        }

        /// <summary>Returns the componentwise floating point remainder of x/y.</summary>
        /// <param name="x">The dividend in x/y.</param>
        /// <param name="y">The divisor in x/y.</param>
        /// <returns>The componentwise remainder of x/y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 fmod(float4 x, float4 y)
        {
            return new float4(x.x % y.x, x.y % y.y, x.z % y.z, x.w % y.w);
        }

        

        /// <summary>Returns the square root of a float value.</summary>
        /// <param name="x">Value to use when computing square root.</param>
        /// <returns>The square root.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

        /// <summary>Returns the componentwise square root of a float2 vector.</summary>
        /// <param name="x">Value to use when computing square root.</param>
        /// <returns>The componentwise square root.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 sqrt(float2 x)
        {
            return new float2(sqrt(x.x), sqrt(x.y));
        }

    /// <summary>Returns the length of a float value. Equivalent to the absolute value.</summary>
        /// <param name="x">Value to use when computing length.</param>
        /// <returns>Length of x.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float length(float x)
        {
            return abs(x);
        }

        /// <summary>Returns the length of a float2 vector.</summary>
        /// <param name="x">Vector to use when computing length.</param>
        /// <returns>Length of vector x.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float length(float2 x)
        {
            return sqrt(dot(x, x));
        }

        /// <summary>Returns the length of a float3 vector.</summary>
        /// <param name="x">Vector to use when computing length.</param>
        /// <returns>Length of vector x.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float length(float3 x)
        {
            return sqrt(dot(x, x));
        }

        /// <summary>Returns the length of a float4 vector.</summary>
        /// <param name="x">Vector to use when computing length.</param>
        /// <returns>Length of vector x.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float length(float4 x)
        {
            return sqrt(dot(x, x));
        }

        /// <summary>Returns the length of a double value. Equivalent to the absolute value.</summary>
        /// <param name="x">Value to use when computing squared length.</param>
        /// <returns>Squared length of x.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double length(double x)
        {
            return abs(x);
        }

        /// <summary>Returns the distance between two float values.</summary>
        /// <param name="x">First value to use in distance computation.</param>
        /// <param name="y">Second value to use in distance computation.</param>
        /// <returns>The distance between x and y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float distance(float x, float y)
        {
            return abs(y - x);
        }

        /// <summary>Returns the distance between two float2 vectors.</summary>
        /// <param name="x">First vector to use in distance computation.</param>
        /// <param name="y">Second vector to use in distance computation.</param>
        /// <returns>The distance between x and y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float distance(float2 x, float2 y)
        {
            return length(y - x);
        }

        /// <summary>Returns the distance between two float3 vectors.</summary>
        /// <param name="x">First vector to use in distance computation.</param>
        /// <param name="y">Second vector to use in distance computation.</param>
        /// <returns>The distance between x and y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float distance(float3 x, float3 y)
        {
            return length(y - x);
        }

        /// <summary>Returns the distance between two float4 vectors.</summary>
        /// <param name="x">First vector to use in distance computation.</param>
        /// <param name="y">Second vector to use in distance computation.</param>
        /// <returns>The distance between x and y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float distance(float4 x, float4 y)
        {
            return length(y - x);
        }


        /// <summary>Returns the distance between two double values.</summary>
        /// <param name="x">First value to use in distance computation.</param>
        /// <param name="y">Second value to use in distance computation.</param>
        /// <returns>The distance between x and y.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double distance(double x, double y)
        {
            return abs(y - x);
        }
        
        /// <summary>Returns true if any component of the input float2 vector is non-zero, false otherwise.</summary>
        /// <param name="x">Vector of values to compare.</param>
        /// <returns>True if any the components of x are non-zero, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(float2 x)
        {
            return x.x != 0.0f || x.y != 0.0f;
        }

        /// <summary>Returns true if any component of the input float3 vector is non-zero, false otherwise.</summary>
        /// <param name="x">Vector of values to compare.</param>
        /// <returns>True if any the components of x are non-zero, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(float3 x)
        {
            return x.x != 0.0f || x.y != 0.0f || x.z != 0.0f;
        }

        /// <summary>Returns true if any component of the input float4 vector is non-zero, false otherwise.</summary>
        /// <param name="x">Vector of values to compare.</param>
        /// <returns>True if any the components of x are non-zero, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool any(float4 x)
        {
            return x.x != 0.0f || x.y != 0.0f || x.z != 0.0f || x.w != 0.0f;
        }


        /// <summary>Returns true if all components of the input float2 vector are non-zero, false otherwise.</summary>
        /// <param name="x">Vector of values to compare.</param>
        /// <returns>True if all the components of x are non-zero, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(float2 x)
        {
            return x.x != 0.0f && x.y != 0.0f;
        }

        /// <summary>Returns true if all components of the input float3 vector are non-zero, false otherwise.</summary>
        /// <param name="x">Vector of values to compare.</param>
        /// <returns>True if all the components of x are non-zero, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(float3 x)
        {
            return x.x != 0.0f && x.y != 0.0f && x.z != 0.0f;
        }

        /// <summary>Returns true if all components of the input float4 vector are non-zero, false otherwise.</summary>
        /// <param name="x">Vector of values to compare.</param>
        /// <returns>True if all the components of x are non-zero, false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool all(float4 x)
        {
            return x.x != 0.0f && x.y != 0.0f && x.z != 0.0f && x.w != 0.0f;
        }


        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int select(int falseValue, int trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }


        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint select(uint falseValue, uint trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }


        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long select(long falseValue, long trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }

        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong select(ulong falseValue, ulong trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }


        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float select(float falseValue, float trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }

        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 select(float2 falseValue, float2 trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }

        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 select(float3 falseValue, float3 trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }

        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 select(float4 falseValue, float4 trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }


        /// <summary>Returns trueValue if test is true, falseValue otherwise.</summary>
        /// <param name="falseValue">Value to use if test is false.</param>
        /// <param name="trueValue">Value to use if test is true.</param>
        /// <param name="test">Bool value to choose between falseValue and trueValue.</param>
        /// <returns>The selection between falseValue and trueValue according to bool test.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double select(double falseValue, double trueValue, bool test)
        {
            return test ? trueValue : falseValue;
        }

        /// <summary>Returns the result of a step function where the result is 1.0f when x &gt;= threshold and 0.0f otherwise.</summary>
        /// <param name="threshold">Value to be used as a threshold for returning 1.</param>
        /// <param name="x">Value to compare against threshold.</param>
        /// <returns>1 if the comparison x &gt;= threshold is true, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float step(float threshold, float x)
        {
            return select(0.0f, 1.0f, x >= threshold);
        }


        /// <summary>Returns the result of a step function where the result is 1.0f when x &gt;= threshold and 0.0f otherwise.</summary>
        /// <param name="threshold">Value to be used as a threshold for returning 1.</param>
        /// <param name="x">Value to compare against threshold.</param>
        /// <returns>1 if the comparison x &gt;= threshold is true, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 step(float2 threshold, float2 x)
        {
            return new float2(step(threshold.x, x.x), step(threshold.y, x.y));
        }

        /// <summary>Returns the result of a step function where the result is 1.0f when x &gt;= threshold and 0.0f otherwise.</summary>
        /// <param name="threshold">Value to be used as a threshold for returning 1.</param>
        /// <param name="x">Value to compare against threshold.</param>
        /// <returns>1 if the comparison x &gt;= threshold is true, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 step(float3 threshold, float3 x)
        {
            return new float3(step(threshold.xy, x.xy), step(threshold.z, x.z));
        }

        /// <summary>Returns the result of a step function where the result is 1.0f when x &gt;= threshold and 0.0f otherwise.</summary>
        /// <param name="threshold">Value to be used as a threshold for returning 1.</param>
        /// <param name="x">Value to compare against threshold.</param>
        /// <returns>1 if the comparison x &gt;= threshold is true, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 step(float4 threshold, float4 x)
        {
            return new float4(step(threshold.xyz, x.xyz), step(threshold.w, x.w));
        }


        /// <summary>Given an incident vector i and a normal vector n, returns the reflection vector r = i - 2.0f * dot(i, n) * n.</summary>
        /// <param name="i">Incident vector.</param>
        /// <param name="n">Normal vector.</param>
        /// <returns>Reflection vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 reflect(float2 i, float2 n)
        {
            return i - 2f * n * dot(i, n);
        }

        /// <summary>Given an incident vector i and a normal vector n, returns the reflection vector r = i - 2.0f * dot(i, n) * n.</summary>
        /// <param name="i">Incident vector.</param>
        /// <param name="n">Normal vector.</param>
        /// <returns>Reflection vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 reflect(float3 i, float3 n)
        {
            return i - 2f * n * dot(i, n);
        }

        /// <summary>Given an incident vector i and a normal vector n, returns the reflection vector r = i - 2.0f * dot(i, n) * n.</summary>
        /// <param name="i">Incident vector.</param>
        /// <param name="n">Normal vector.</param>
        /// <returns>Reflection vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 reflect(float4 i, float4 n)
        {
            return i - 2f * n * dot(i, n);
        }
        

        /// <summary>
        /// Compute vector projection of a onto b.
        /// </summary>
        /// <remarks>
        /// Some finite vectors a and b could generate a non-finite result. This is most likely when a's components
        /// are very large (close to Single.MaxValue) or when b's components are very small (close to FLT_MIN_NORMAL).
        /// which will use a given default value if the result is not finite.
        /// </remarks>
        /// <param name="a">Vector to project.</param>
        /// <param name="ontoB">Non-zero vector to project onto.</param>
        /// <returns>Vector projection of a onto b.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 project(float2 a, float2 ontoB)
        {
            return dot(a, ontoB) / dot(ontoB, ontoB) * ontoB;
        }

        /// <summary>
        /// Compute vector projection of a onto b.
        /// </summary>
        /// <remarks>
        /// Some finite vectors a and b could generate a non-finite result. This is most likely when a's components
        /// are very large (close to Single.MaxValue) or when b's components are very small (close to FLT_MIN_NORMAL).
      /// </remarks>
        /// <param name="a">Vector to project.</param>
        /// <param name="ontoB">Non-zero vector to project onto.</param>
        /// <returns>Vector projection of a onto b.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 project(float3 a, float3 ontoB)
        {
            return dot(a, ontoB) / dot(ontoB, ontoB) * ontoB;
        }

        /// <summary>
        /// Compute vector projection of a onto b.
        /// </summary>
        /// <remarks>
        /// Some finite vectors a and b could generate a non-finite result. This is most likely when a's components
        /// are very large (close to Single.MaxValue) or when b's components are very small (close to FLT_MIN_NORMAL).
        /// </remarks>
        /// <param name="a">Vector to project.</param>
        /// <param name="ontoB">Non-zero vector to project onto.</param>
        /// <returns>Vector projection of a onto b.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 project(float4 a, float4 ontoB)
        {
            return dot(a, ontoB) / dot(ontoB, ontoB) * ontoB;
        }
        

        /// <summary>Returns the sine and cosine of the input float value x through the out parameters s and c.</summary>
        /// <remarks>When Burst compiled, his method is faster than calling sin() and cos() separately.</remarks>
        /// <param name="x">Input angle in radians.</param>
        /// <param name="s">Output sine of the input.</param>
        /// <param name="c">Output cosine of the input.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(float x, out float s, out float c)
        {
            s = sin(x);
            c = cos(x);
        }

        /// <summary>Returns the componentwise sine and cosine of the input float2 vector x through the out parameters s and c.</summary>
        /// <remarks>When Burst compiled, his method is faster than calling sin() and cos() separately.</remarks>
        /// <param name="x">Input vector containing angles in radians.</param>
        /// <param name="s">Output vector containing the componentwise sine of the input.</param>
        /// <param name="c">Output vector containing the componentwise cosine of the input.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(float2 x, out float2 s, out float2 c)
        {
            s = sin(x);
            c = cos(x);
        }

        /// <summary>Returns the componentwise sine and cosine of the input float3 vector x through the out parameters s and c.</summary>
        /// <remarks>When Burst compiled, his method is faster than calling sin() and cos() separately.</remarks>
        /// <param name="x">Input vector containing angles in radians.</param>
        /// <param name="s">Output vector containing the componentwise sine of the input.</param>
        /// <param name="c">Output vector containing the componentwise cosine of the input.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(float3 x, out float3 s, out float3 c)
        {
            s = sin(x);
            c = cos(x);
        }

        /// <summary>Returns the componentwise sine and cosine of the input float4 vector x through the out parameters s and c.</summary>
        /// <remarks>When Burst compiled, his method is faster than calling sin() and cos() separately.</remarks>
        /// <param name="x">Input vector containing angles in radians.</param>
        /// <param name="s">Output vector containing the componentwise sine of the input.</param>
        /// <param name="c">Output vector containing the componentwise cosine of the input.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(float4 x, out float4 s, out float4 c)
        {
            s = sin(x);
            c = cos(x);
        }


        /// <summary>Returns the sine and cosine of the input double value x through the out parameters s and c.</summary>
        /// <remarks>When Burst compiled, his method is faster than calling sin() and cos() separately.</remarks>
        /// <param name="x">Input angle in radians.</param>
        /// <param name="s">Output sine of the input.</param>
        /// <param name="c">Output cosine of the input.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void sincos(double x, out double s, out double c)
        {
            s = sin(x);
            c = cos(x);
        }

        /// <summary>Returns the result of rotating the bits of an int left by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int rol(int x, int n)
        {
            return (int)rol((uint)x, n);
        }

        /// <summary>Returns the result of rotating the bits of a uint left by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint rol(uint x, int n)
        {
            return (x << n) | (x >> (32 - n));
        }

        /// <summary>Returns the result of rotating the bits of a long left by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long rol(long x, int n)
        {
            return (long)rol((ulong)x, n);
        }


        /// <summary>Returns the result of rotating the bits of a ulong left by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong rol(ulong x, int n)
        {
            return (x << n) | (x >> (64 - n));
        }


        /// <summary>Returns the result of rotating the bits of an int right by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ror(int x, int n)
        {
            return (int)ror((uint)x, n);
        }

        /// <summary>Returns the result of rotating the bits of a uint right by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ror(uint x, int n)
        {
            return (x >> n) | (x << (32 - n));
        }

        /// <summary>Returns the result of rotating the bits of a long right by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ror(long x, int n)
        {
            return (long)ror((ulong)x, n);
        }
        
        /// <summary>Returns the result of rotating the bits of a ulong right by bits n.</summary>
        /// <param name="x">Value to rotate.</param>
        /// <param name="n">Number of bits to rotate.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ror(ulong x, int n)
        {
            return (x >> n) | (x << (64 - n));
        }


        
        /// <summary>Returns the result of converting a float value from degrees to radians.</summary>
        /// <param name="x">Angle in degrees.</param>
        /// <returns>Angle converted to radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float radians(float x)
        {
            return x * TORADIANS;
        }
        

        /// <summary>Returns the result of converting a float value from degrees to radians.</summary>
        /// <param name="x">Angle in degrees.</param>
        /// <returns>Angle converted to radians.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double radians(double x)
        {
            return x * TORADIANS_DBL;
        }


        /// <summary>Returns the result of converting a double value from radians to degrees.</summary>
        /// <param name="x">Angle in radians.</param>
        /// <returns>Angle converted to degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float degrees(float x)
        {
            return x * TODEGREES;
        }


        /// <summary>Returns the result of converting a double value from radians to degrees.</summary>
        /// <param name="x">Angle in radians.</param>
        /// <returns>Angle converted to degrees.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double degrees(double x)
        {
            return x * TODEGREES_DBL;
        }



        /// <summary>
        /// Unity's up axis (0, 1, 0).
        /// </summary>
        /// <remarks>Matches [https://docs.unity3d.com/ScriptReference/Vector3-up.html](https://docs.unity3d.com/ScriptReference/Vector3-up.html)</remarks>
        /// <returns>The up axis.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 up()
        {
            return new float3(0.0f, 1.0f, 0.0f);
        } // for compatibility

        /// <summary>
        /// Unity's back axis (0, 0, -1).
        /// </summary>
        /// <remarks>Matches [https://docs.unity3d.com/ScriptReference/Vector3-back.html](https://docs.unity3d.com/ScriptReference/Vector3-back.html)</remarks>
        /// <returns>The back axis.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 back()
        {
            return new float3(0.0f, 0.0f, -1.0f);
        }

        /// <summary>
        /// Unity's left axis (-1, 0, 0).
        /// </summary>
        /// <remarks>Matches [https://docs.unity3d.com/ScriptReference/Vector3-left.html](https://docs.unity3d.com/ScriptReference/Vector3-left.html)</remarks>
        /// <returns>The left axis.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 left()
        {
            return new float3(-1.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Unity's right axis (1, 0, 0).
        /// </summary>
        /// <remarks>Matches [https://docs.unity3d.com/ScriptReference/Vector3-right.html](https://docs.unity3d.com/ScriptReference/Vector3-right.html)</remarks>
        /// <returns>The right axis.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 right()
        {
            return new float3(1.0f, 0.0f, 0.0f);
        }
    }
}