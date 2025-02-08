using System.Runtime.CompilerServices;
using Plato;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace PathTracer
{
    public interface ISignedDistanceField
    {
        float Distance(Vector3 p);
        Vector8 Distance(in Vector3x8 p);
    }

    public readonly struct LineDistanceField : ISignedDistanceField
    {
        public readonly Line2D Line;

        public readonly Vector2 A;
        public readonly Vector2 B;
        public readonly float BdotB;

        public readonly Vector3x8 A8;
        public readonly Vector3x8 B8;
        public readonly Vector8 BdotB8;

        [MethodImpl(AggressiveInlining)]
        public LineDistanceField(Line2D line)
        {
            Line = line;
            A = line.A * 0.5f;
            B = line.B * 0.5f - A;
            A8 = new Vector3x8(A.X, A.Y, Zero);
            B8 = new Vector3x8(B.X, B.Y, Zero);
            BdotB8 = B8.Dot(B8);
            BdotB = B.Dot(B);
        }

        [MethodImpl(AggressiveInlining)]
        public float Distance(Vector3 p)
        {
            var xy = p.XY();
            var o = xy - (A + B * MathF.Min(-MathF.Min((A - xy).Dot(B) / BdotB, 0), 1));
            return o.Dot(o).SquareRoot;
        }

        public static Vector8 Zero = new Vector8(0.0f);
        public static Vector8 One = new Vector8(1.0f);

        [MethodImpl(AggressiveInlining)]
        public Vector8 Distance(in Vector3x8 p)
        {
            var xy = new Vector3x8(p.X, p.Y, Zero);
            var min1 = -((A8 - xy).Dot(B8) / BdotB8).Min(Zero);
            var min2 = min1.Min(One);
            var o = xy - (A8 + B8 * min2);
            return o.Dot(o).Sqrt;
        }
    }

    public readonly struct CurveDistanceField : ISignedDistanceField
    {
        public readonly Vector3 Center;

        [MethodImpl(AggressiveInlining)]
        public CurveDistanceField(Vector3 center)
            => Center = center;

        [MethodImpl(AggressiveInlining)]
        public float Distance(Vector3 p)
        {
            var o = p.WithZ(0) - Center;
            if (o.X > 0)
            {
                return (o.Dot(o).SquareRoot - 2).Abs;
            }

            o = o.WithY(o.Y > 0 ? o.Y - 2 : o.Y + 2);
            return o.Dot(o).SquareRoot;
        }

        [MethodImpl(AggressiveInlining)]
        public Vector8 Distance(in Vector3x8 ps)
        {
            var r = new float[8];
            for (var i = 0; i < 8; i++)
            {
                var p = ps[i];
                var o = p.WithZ(0) - Center;
                if (o.X > 0)
                {
                    r[i] = (o.Dot(o).SquareRoot - 2).Abs;
                }
                else
                {
                    o = o.WithY(o.Y > 0 ? o.Y - 2 : o.Y + 2);
                    r[i] = o.Dot(o).SquareRoot;
                }
            }

            return new(r[0], r[1], r[2], r[3], r[4], r[5], r[6], r[7]);
        }
    }

    public readonly struct BoxDistanceField : ISignedDistanceField
    {
        public readonly Bounds3D Box;
        public readonly Vector3x8 BoxMin;
        public readonly Vector3x8 BoxMax;

        [MethodImpl(AggressiveInlining)]
        public BoxDistanceField(Bounds3D box)
        {
            Box = box;
            BoxMin = new(box.Min);
            BoxMax = new(box.Max);
        }

        [MethodImpl(AggressiveInlining)]
        public float Distance(Vector3 p)
        {
            var lowerLeft = p + (Box.Min * -1.0f);
            var upperRight = Box.Max + (p * -1.0f);
            return -MathF.Min(MathF.Min(
                    MathF.Min(lowerLeft.X, upperRight.X),
                    MathF.Min(lowerLeft.Y, upperRight.Y)),
                MathF.Min(lowerLeft.Z, upperRight.Z));
        }

        [MethodImpl(AggressiveInlining)]
        public Vector8 Distance(in Vector3x8 p)
        {
            var lowerLeft = p + (BoxMin * -1.0f);
            var upperRight = BoxMax + (p * -1.0f);
            var minX = lowerLeft.X.Min(upperRight.X);
            var minY = lowerLeft.Y.Min(upperRight.Y);
            var minZ = lowerLeft.Z.Min(upperRight.Z);
            var minXY = minX.Min(minY);
            return -minXY.Min(minZ);
        }
    }

    public readonly struct RoomDistanceField : ISignedDistanceField
    {
        public static readonly Bounds3D LowerRoomBounds = ((-30, -0.5f, -30), (30, 18, 30));
        public static readonly Bounds3D UpperRoomBounds = ((-25, 17, -25), (25, 20, 25));
        public static readonly Bounds3D CeilingBounds = ((1.5f, 18.5f, -25), (6.5f, 20, 25));

        public readonly BoxDistanceField LowerRoom;
        public readonly BoxDistanceField UpperRoom;
        public readonly BoxDistanceField Ceiling;

        public const float PlankDistance = 8;

        [MethodImpl(AggressiveInlining)]
        public RoomDistanceField()
        {
            LowerRoom = new BoxDistanceField(LowerRoomBounds);
            UpperRoom = new BoxDistanceField(UpperRoomBounds);
            Ceiling = new BoxDistanceField(CeilingBounds);
        }

        [MethodImpl(AggressiveInlining)]
        public float Distance(Vector3 p)
            =>
                // Min(A,B) = Union with Constructive solid geometry
                MathF.Min(
                    //-Min carves an empty space
                    -MathF.Min(
                        LowerRoom.Distance(p),
                        UpperRoom.Distance(p)
                    ),
                    Ceiling.Distance(
                        // Ceiling "planks" spaced 8 units apart.
                        new(p.X.Abs % PlankDistance, p.Y, p.Z)));

        [MethodImpl(AggressiveInlining)]
        public Vector8 Distance(in Vector3x8 p)
            =>  //-Min carves an empty space
                -LowerRoom.Distance(p).Min(UpperRoom.Distance(p)).Min(
                    Ceiling.Distance(
                        // Ceiling "planks" spaced 8 units apart.
                        new Vector3x8(p.X.Abs.Modulo(new Number(PlankDistance)), p.Y, p.Z)));
    }

    public struct LettersDistanceField : ISignedDistanceField
    {
        public static Line2D[] LinePoints =
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

        // Two Curves (for P and R in PixaR) with hard-coded locations.
        public static readonly Vector3[] CurvePoints =
        {
            (-11, 6, 0),
            (11, 6, 0)
        };

        public LettersDistanceField()
        {
            Lines = LinePoints.Select(l => new LineDistanceField(l)).ToArray();
            Curves = CurvePoints.Select(c => new CurveDistanceField(c)).ToArray();
        }

        public LineDistanceField[] Lines;
        public CurveDistanceField[] Curves;

        [MethodImpl(AggressiveInlining)]
        public float Distance(Vector3 p)
        {
            var d = 1e9f;
            for (var i = 0; i < Lines.Length; i++)
                d = MathF.Min(d, Lines[i].Distance(p));
            for (var i = 0; i < Curves.Length; i++)
                d = MathF.Min(d, Curves[i].Distance(p));
            return (d.Pow(8) + p.Z.Pow(8)).Pow(0.125f) - 0.5f;
        }
        
        [MethodImpl(AggressiveInlining)]
        public Vector8 Distance(in Vector3x8 p)
        {
            var d = new Vector8(1e9f);
            for (var i = 0; i < Lines.Length; i++)
                d = d.Min(Lines[i].Distance(p));
            for (var i = 0; i < Curves.Length; i++)
                d = d.Min(Curves[i].Distance(p));
            return (d.Pow(8) + p.Z.Pow(8)).Pow(0.125f) - new Vector8(0.5f);
        }
    }

    public struct SunDistanceField : ISignedDistanceField
    {
        public const float SkyHeight = 19.9f;

        [MethodImpl(AggressiveInlining)]
        public float Distance(Vector3 p)
            => SkyHeight - p.Y;

        [MethodImpl(AggressiveInlining)]
        public Vector8 Distance(in Vector3x8 p)
            => new Vector8(SkyHeight) - p.Y;
    }
}