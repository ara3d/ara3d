#pragma once

namespace Ara3D
{
    namespace Serialization
    {
        namespace FBX
        {
            using namespace System;
            using namespace System::Collections::Generic;
            using namespace Ara3D::Mathematics;

            public ref class DotNetFbxMaterial
            {
            public:
                String^ Name;
                Int32 Id;
                Int32 Index;
                bool IsPhong;
                bool IsLambert;
                Vector3 Ambient;
                Vector3 Diffuse;
                Vector3 Specular;
                Vector3 Emissive;
                float Transparency;
                float Shininess;
                float Reflectivity;
                bool IsHlsl;
                bool IsCgfx;
                String^ ShadingModel;
            };

            public ref class DotNetFbxMesh
            {
            public:
                Int32 Index;
                List<Vector3>^ Vertices = gcnew List<Vector3>();
                List<Int32>^ Indices = gcnew List<Int32>();
            };

            public ref class DotNetFbxNode
            {
            public:
                Int32 MeshIndex;
                Matrix4x4 Matrix;
                Int32 ParentIndex;
                Int32 Index;
                String^ Name;
                String^ Type;
                List<DotNetFbxMaterial^>^ Materials = gcnew List<DotNetFbxMaterial^>();
            };

            public ref class DotNetFbxScene
            {
            public:
                Dictionary<String^, DotNetFbxMaterial^>^ Materials = gcnew Dictionary<String^, DotNetFbxMaterial^>();
                List<DotNetFbxMesh^>^ Meshes = gcnew List<DotNetFbxMesh^>();
                List<DotNetFbxNode^>^ Nodes = gcnew List<DotNetFbxNode^>();
                String^ Title;
                String^ Subject;
                String^ Author;
                String^ Keywords;
                String^ Revision;
                String^ Comment;
                String^ ApplicationName;
                String^ ApplicationVendor;
                String^ ApplicationVersion;
                int FileVersionMajor;
                int FileVersionMinor;
                int FileVersionRevision;
                static DotNetFbxScene^ Load(String^ fileName);
            };
        }
    }
}
