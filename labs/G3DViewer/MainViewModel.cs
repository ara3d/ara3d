using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Ara3D.Math;
using Ara3D.Serialization.G3D;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using Color = System.Windows.Media.Color;
using Matrix = SharpDX.Matrix;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
using Vector2 = SharpDX.Vector2;
using Vector3 = SharpDX.Vector3;

namespace G3DViewer
{
   

    public class MainViewModelMesh : ObservableObject
    {
        public MeshGeometry3D Model { get; set; }
        public ObservableCollection<Matrix> ModelInstances { get; } = new ObservableCollection<Matrix>();
        public ObservableCollection<InstanceParameter> InstanceParams { get; } = new ObservableCollection<InstanceParameter>();
        public PhongMaterial ModelMaterial { get; set; }
        public Transform3D ModelTransform { get; set; }

        public void AddInstance(Matrix Mat, InstanceParameter InstanceParam)
        {
            ModelInstances.Add(Mat);
            InstanceParams.Add(InstanceParam);
            OnPropertyChanged(nameof(InstanceParams));
            OnPropertyChanged(nameof(ModelInstances));
        }
    }

    // TODO: Add comments
    public class MainViewModel : BaseViewModel
    {
        public const int SubMeshSplitSize = 100000; // Size of the mesh (in faces) that get rendered. The main mesh is split into submeshes for rendering.
        public ObservableCollection<MainViewModelMesh> Models { get; set; } = new ObservableCollection<MainViewModelMesh>();
        public Matrix ModelsTransform = Matrix.Identity;
        public LineGeometry3D Lines { get; private set; }
        public LineGeometry3D Grid { get; private set; }
        public Vector3D DirectionalLightDirection { get; private set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }
        public DisplayStats _displayStats;
        public DisplayStats displayStats
        {
            get
            {
                return _displayStats;
            }
            set
            {
                _displayStats = value;
                OnPropertyChanged();
            }
        }

        private int InstanceCount;
        private int ModelCount;
        private int TriangleCount;

        public MainViewModel()
        {
            Title = "G3D Viewer";
            EffectsManager = new DefaultEffectsManager();

            Camera = new PerspectiveCamera { Position = new Point3D(40, 40, 40), LookDirection = new Vector3D(-40, -40, -40), UpDirection = new Vector3D(0, 1, 0), FarPlaneDistance = 10000.0, NearPlaneDistance = 5.0 };

            // setup lighting            
            AmbientLightColor = Colors.DarkGray;
            DirectionalLightColor = Colors.White;
            DirectionalLightDirection = new Vector3D(-2, -5, -2);
        }

        public void UpdateSubTitle()
        {
            SubTitle = "Models: " + ModelCount + "\nInstances: " + InstanceCount + "\nTriangleCount: " + TriangleCount;
        }

        public int AddModel(MeshGeometry3D Model)
        {
            MainViewModelMesh newViewModelMesh = new MainViewModelMesh();

            newViewModelMesh.Model = Model;
            newViewModelMesh.ModelTransform = Transform3D.Identity;
            newViewModelMesh.ModelMaterial = PhongMaterials.White;

            Models.Add(newViewModelMesh);
            OnPropertyChanged(nameof(Models));

            ModelCount++;

            return Models.Count - 1;
        }

        public void AddInstance(int ModelIndex, Matrix4x4 Transform)
        {
            var model = Models[ModelIndex];

            var color = new Color4(1, 1, 1, 1);
            model.AddInstance(Ara3DToSharpDX(Transform), new InstanceParameter
            { DiffuseColor = color, 
                TexCoordOffset = new Vector2(0, 0) });

            TriangleCount += model.Model.Triangles.Count();

            InstanceCount++;
        }
        public void OnMouseLeftButtonDownHandler(object sender, MouseButtonEventArgs e)
        {
            var viewport = sender as Viewport3DX;
            if (viewport == null) { return; }
            var point = e.GetPosition(viewport);
            var hitTests = viewport.FindHits(point);
            if (hitTests.Count > 0)
            {
                foreach(var hit in hitTests)
                {                  
                 /*   if (hit.ModelHit is InstancingMeshGeometryModel3D)
                    {
                        var index = (int)hit.Tag;
                        InstanceParam[index].EmissiveColor = InstanceParam[index].EmissiveColor != Colors.Yellow.ToColor4()? Colors.Yellow.ToColor4() : Colors.Black.ToColor4();
                        InstanceParam = (InstanceParameter[])InstanceParam.Clone();
                        break;
                    }
                    else if(hit.ModelHit is LineGeometryModel3D)
                    {
                        var index = (int)hit.Tag;
                        SelectedLineInstances = new Matrix[] { ModelInstances[index] };
                        break;
                    }*/
                }
            }
        }
        
        static Vector3 Ara3DToSharpDX(Ara3D.Math.Vector3 input)
        {
            return new Vector3(input.X, input.Z, input.Y);
        }
        static Vector2 Ara3DToSharpDX(Ara3D.Math.Vector2 input)
        {
            return new Vector2(input.X, input.Y);
        }
        static Matrix Ara3DToSharpDX(Matrix4x4 input)
        {
            var ret = new Matrix(
                input.M11, input.M12, input.M13, input.M14,
                input.M21, input.M22, input.M23, input.M24,
                input.M31, input.M32, input.M33, input.M34,
                input.M41, input.M42, input.M43, input.M44
                );

            return ret;
        }

        MeshBuilder bakedMeshBuilder = null;


        internal void AddTriangle(int i0, int i1, int i2, ref float faceArea, ref int minIndex, ref int maxIndex, ref Vector3Collection sharpDxVectos, ref IntCollection triangleIndices)
        {
            var v0 = sharpDxVectos[i0];
            var v1 = sharpDxVectos[i1];
            var v2 = sharpDxVectos[i2];
            float triangleArea = Vector3.Cross(v0 - v1, v0 - v2).Length() * 0.5f;
            faceArea += triangleArea;

            displayStats.NumTriangles++;

            if (triangleArea > 0.0f)
            {
                if (triangleArea < DisplayStats.SmallTriangleSize)
                {
                    displayStats.NumSmallTriangles++;
                }

                // Ignore small triangles
                float diagonalArea = displayStats.AABB.Diagonal * 0.0002f;
                if (triangleArea > diagonalArea * diagonalArea)
                {
                    minIndex = Math.Min(minIndex, i0);
                    maxIndex = Math.Max(maxIndex, i0);
                    minIndex = Math.Min(minIndex, i1);
                    maxIndex = Math.Max(maxIndex, i1);
                    minIndex = Math.Min(minIndex, i2);
                    maxIndex = Math.Max(maxIndex, i2);
                    triangleIndices.Add(i0);
                    triangleIndices.Add(i1);
                    triangleIndices.Add(i2);
                }


                displayStats.MinTriangleArea = Math.Min(triangleArea, displayStats.MinTriangleArea);
                displayStats.MaxTriangleArea = Math.Max(triangleArea, displayStats.MaxTriangleArea);
            }
            else
            {
                displayStats.NumDegenerateTriangles++;
            }
        }

        internal int AddG3DData(G3D g3dFile)
        {
            var vertexData = g3dFile.Vertices;
            var indexData = g3dFile.Indices;
            var faceCount = g3dFile.NumFaces;

            var sharpDxVectos = new Vector3Collection(vertexData.Count);

            displayStats.NumVertices += vertexData.Count;
            displayStats.NumFaces += faceCount;

            int currentFace = 0;
            int globalVectorIndex = 0;

            while (currentFace < faceCount)
            {
                var geometry3d = new MeshGeometry3D();
                var triangleIndices = new IntCollection(SubMeshSplitSize * 3);
                int subFaceCount = 0;
                int minIndex = int.MaxValue;
                int maxIndex = 0;
                for (; currentFace < faceCount && subFaceCount < SubMeshSplitSize; currentFace++, subFaceCount++)
                {
                    float faceArea = 0.0f;
                    int i0 = indexData[globalVectorIndex];
                    int i1 = indexData[globalVectorIndex + 2];
                    int i2 = indexData[globalVectorIndex + 1];

                    AddTriangle(i0, i1, i2, ref faceArea, ref minIndex, ref maxIndex, ref sharpDxVectos, ref triangleIndices);
                
                    if (faceArea == 0.0f)
                    {
                        displayStats.NumDegenerateFaces++;
                    }

                    if (faceArea < DisplayStats.SmallTriangleSize)
                    {
                        displayStats.NumSmallFaces++;
                    }

                    globalVectorIndex += 3;
                }

                for (int index =0; index < triangleIndices.Count; index++)
                {
                    triangleIndices[index] -= minIndex;
                }
                if (triangleIndices.Count > 0)
                {
                    geometry3d.Positions = new Vector3Collection(sharpDxVectos.GetRange(minIndex, maxIndex - minIndex + 1));
                    geometry3d.Indices = triangleIndices;
                    geometry3d.Normals = geometry3d.CalculateNormals();

                    int modelIndex = AddModel(geometry3d);
                    AddInstance(modelIndex, Matrix4x4.CreateTranslation(-displayStats.AABB.Center));
                }
            }

            currentFace = 0;
            globalVectorIndex = 0;

           

            for (int i = 0; i < DisplayStats.NumHistogramDivisions; i++)
            {
                float area0 = i / (float)(DisplayStats.NumHistogramDivisions - 1) * (displayStats.MaxTriangleArea - displayStats.MinTriangleArea) + displayStats.MinTriangleArea;
                float area1 = (i + 1) / (float)(DisplayStats.NumHistogramDivisions - 1) * (displayStats.MaxTriangleArea - displayStats.MinTriangleArea) + displayStats.MinTriangleArea;

                string key = string.Format("{0:0.00} - {1:0.00}", area0, area1);

                displayStats.AreaHistogram[key] = displayStats.AreaHistogramArray[i];
                displayStats.AreaHistogramLog[key] = (float)Math.Log10(displayStats.AreaHistogramArray[i] + 1);
            }

            return 0;
        }
    }
}