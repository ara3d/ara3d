using Ara3D.DataFormat;
using Plato;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace PathTracer
{
    public class Wavefront
    {
        public long Count
        {
            [MethodImpl(AggressiveInlining)]
            get { return Active.Count; }
        }

        [MethodImpl(AggressiveInlining)]
        public void AddRay(Ray3D ray)
        {
            OX.Add(ray.Origin.X);
            OY.Add(ray.Origin.Y);
            OZ.Add(ray.Origin.Z);
            
            PX.Add(ray.Origin.X);
            PY.Add(ray.Origin.Y);
            PZ.Add(ray.Origin.Z);
            
            DX.Add(ray.Direction.X);
            DY.Add(ray.Direction.Y);
            DZ.Add(ray.Direction.Z);
            Distance.Add(0);
            TotalDistance.Add(0);
            Attenuation.Add(1);
            ColorX.Add(0);
            ColorY.Add(0);
            ColorZ.Add(0);
            HitId.Add(0);
            Active.Add(true);
        }

        [MethodImpl(AggressiveInlining)]
        public void Clear()
        {
            OX.Clear();
            OY.Clear();
            OZ.Clear();
            PX.Clear();
            PY.Clear();
            PZ.Clear(); 
            DX.Clear();
            DY.Clear();
            DZ.Clear();
            Distance.Clear();
            TotalDistance.Clear();
            Attenuation.Clear();
            ColorX.Clear();
            ColorY.Clear();
            ColorZ.Clear();
            HitId.Clear();
            Active.Clear();
        }

        [MethodImpl(AggressiveInlining)]
        public Ray GetRay()
            => new Ray() { Wavefront = this };

        [MethodImpl(AggressiveInlining)]
        public Ray GetRay(int i)
            => new Ray() { Wavefront = this, Index = i };

        [MethodImpl(AggressiveInlining)]
        public Vector3 GetColor(int i)
            => new(ColorX[i], ColorY[i], ColorZ[i]);

        public UnmanagedList<float> OX = new();
        public UnmanagedList<float> OY = new();
        public UnmanagedList<float> OZ = new();
        
        public UnmanagedList<float> PX = new();
        public UnmanagedList<float> PY = new();
        public UnmanagedList<float> PZ = new();

        public UnmanagedList<float> DX = new();
        public UnmanagedList<float> DY = new();
        public UnmanagedList<float> DZ = new();
        
        public UnmanagedList<float> Distance = new();
        public UnmanagedList<float> TotalDistance = new();
        public UnmanagedList<float> Attenuation = new();
        public UnmanagedList<float> ColorX = new();
        public UnmanagedList<float> ColorY = new();
        public UnmanagedList<float> ColorZ = new();
        public UnmanagedList<int> HitId = new();
        public UnmanagedList<bool> Active = new();
        
        public class Ray
        {
            public Wavefront Wavefront;
            public int Index;

            public float OX
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.OX[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.OX[Index] = value;
            }

            public float OY
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.OY[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.OY[Index] = value;
            }

            public float OZ 
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.OZ[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.OZ[Index] = value;
            }

            public float PX
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.PX[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.PX[Index] = value;
            }

            public float PY
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.PY[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.PY[Index] = value;
            }

            public float PZ
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.PZ[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.PZ[Index] = value;
            }

            public Vector3 Origin
            {
                [MethodImpl(AggressiveInlining)]
                get => new(OX, OY, OZ);
                [MethodImpl(AggressiveInlining)]
                set
                {
                    OX = value.X;
                    OY = value.Y;
                    OZ = value.Z;
                }
            }

            public float DX
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.DX[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.DX[Index] = value;
            }

            public float DY
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.DY[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.DY[Index] = value;
            }

            public float DZ
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.DZ[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.DZ[Index] = value;
            }

            public Vector3 Direction
            {
                [MethodImpl(AggressiveInlining)]
                get => new(DX, DY, DZ);
                [MethodImpl(AggressiveInlining)]
                set
                {
                    DX = value.X;
                    DY = value.Y;
                    DZ = value.Z;
                }
            }

            public Vector3 HitPosition
            {
                [MethodImpl(AggressiveInlining)]
                get => new(PX, PY, PZ);
                [MethodImpl(AggressiveInlining)]
                set
                {
                    PX = value.X;
                    PY = value.Y;
                    PZ = value.Z;
                }
            }

            public float HitDistance
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.Distance[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.Distance[Index] = value;
            }

            public float TotalDistance
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.TotalDistance[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.TotalDistance[Index] = value;
            }

            public float Attenuation
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.Attenuation[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.Attenuation[Index] = value;
            }

            public float ColorX
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.ColorX[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.ColorX[Index] = value;
            }

            public float ColorY
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.ColorY[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.ColorY[Index] = value;
            }

            public float ColorZ
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.ColorZ[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.ColorZ[Index] = value;
            }

            public Vector3 Color
            {
                [MethodImpl(AggressiveInlining)]
                get => new(ColorX, ColorY, ColorZ);
                [MethodImpl(AggressiveInlining)]
                set
                {
                    ColorX = value.X;
                    ColorY = value.Y;
                    ColorZ = value.Z;
                }
            }

            public int HitId
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.HitId[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.HitId[Index] = value;
            }

            public bool Active
            {
                [MethodImpl(AggressiveInlining)]
                get => Wavefront.Active[Index];
                [MethodImpl(AggressiveInlining)]
                set => Wavefront.Active[Index] = value;
            }
        }
    }
}