#include "fbxsdk.h"
#include "FbxReaderWriter.h"
#include <assert.h>
#include <sstream>
#include <map>
#include <algorithm>

using namespace Ara3D::Mathematics;
using namespace System;

namespace Ara3D 
{
    namespace Serialization 
    {
        namespace FBX 
        {
            using namespace System::Runtime::InteropServices;

            template<typename T>
            Vector3 ToVector3(const T & xs)
            {
                return Vector3((float)xs[0], (float)xs[1], (float)xs[2]);
            }

            String^ ToString(const char* x)
            {
                return gcnew String(x);
            }

            String^ ToString(const std::string & x)
            {
                return ToString(x.c_str());
            }

            String^ ToString(const FbxString & x)
            {
                return ToString(x.Buffer());
            }

            template<typename T>
            Vector4 ToVector4(const T & xs)
            {
                return Vector4((float)xs[0], (float)xs[1], (float)xs[2], (float)xs[3]);
            }

            Matrix4x4 ToMatrix(const FbxAMatrix & x)
            {
                return Matrix4x4::CreateFromRows(
                    ToVector4(x.Buffer()[0]),
                    ToVector4(x.Buffer()[1]),
                    ToVector4(x.Buffer()[2]),
                    ToVector4(x.Buffer()[3]));
            }

            public ref class DotNetFbxReaderWriter
            {
            private:
                FbxManager* mSdkManager = nullptr;
                FbxScene* mScene = nullptr;
            public:
                DotNetFbxReaderWriter();
                ~DotNetFbxReaderWriter();
                bool InternalLoad(DotNetFbxScene^ scene, String^ fileName);
            };

            DotNetFbxReaderWriter::~DotNetFbxReaderWriter()
            {
                if (mScene != nullptr)
                {
                    mScene->Destroy();
                }

                if (mSdkManager != nullptr)
                {
                    mSdkManager->Destroy();
                }

                mScene = nullptr;
                mSdkManager = nullptr;
            }

            DotNetFbxReaderWriter::DotNetFbxReaderWriter()
            {
                //The first thing to do is to create the FBX Manager which is the object allocator for almost all the classes in the SDK
                mSdkManager = FbxManager::Create();
                if (!mSdkManager)
                {
                    FBXSDK_printf("Error: Unable to create FBX Manager!\n");
                    exit(1);
                }

                FBXSDK_printf("Autodesk FBX SDK version %s\n", mSdkManager->GetVersion());

                //Create an IOSettings object. This object holds all import/export settings.
                auto ios = FbxIOSettings::Create(mSdkManager, IOSROOT);
                mSdkManager->SetIOSettings(ios);

                //Load plugins from the executable directory (optional)
                auto lPath = FbxGetApplicationDirectory();
                mSdkManager->LoadPluginsDirectory(lPath.Buffer());

                //Create an FBX scene. This object holds most objects imported/exported from/to files.
                mScene = FbxScene::Create(mSdkManager, "My Scene");
                if (!mScene)
                {
                    FBXSDK_printf("Error: Unable to create FBX scene!\n");
                    exit(1);
                }
            }

            DotNetFbxMesh^ ConvertMesh(FbxMesh * src, int index)
            {
                auto pVertices = src->GetControlPoints();
                auto numVertices = src->GetControlPointsCount();

                DotNetFbxMesh^ r = gcnew DotNetFbxMesh();

                int maxIndex = 0;

                for (int p = 0; p < src->mPolygonVertices.Size(); p++)
                {
                    int index = src->mPolygonVertices[p];
                    r->Indices->Add(index);
                    maxIndex = maxIndex > index ? maxIndex : index;
                }

                for (int p = 0; p < src->mPolygons.Size(); p++)
                {
                    if (src->mPolygons[p].mSize != 3)
                        throw std::exception("Polygonal meshes are not supported");
                }

                for (int k = 0; k < numVertices; k++)
                    r->Vertices->Add(ToVector3(pVertices[k]));

                for (int i = 0; i < src->GetElementSmoothingCount(); i++)
                {
                    auto elem = src->GetElementSmoothing(i);
                }
                for (int i = 0; i < src->GetElementUVCount(); i++)
                {
                    auto elem = src->GetElementUV(i);
                }
                for (int i = 0; i < src->GetElementNormalCount(); i++)
                {
                    auto elem = src->GetElementNormal(i);
                }
                for (int i = 0; i < src->GetElementBinormalCount(); i++)
                {
                    auto elem = src->GetElementBinormal(i);
                }
                for (int i = 0; i < src->GetElementTangentCount(); i++)
                {
                    auto elem = src->GetElementTangent(i);
                }
                for (int i = 0; i < src->GetElementMaterialCount(); i++)
                {
                    auto elem = src->GetElementMaterial(i);
                }
                r->Index = index;
                return r;
            }

            DotNetFbxMaterial^ ConvertMaterial(FbxSurfaceMaterial * src)
            {
                DotNetFbxMaterial^ r = gcnew DotNetFbxMaterial();

                r->Name = ToString(src->GetName());
                r->ShadingModel = ToString(src->ShadingModel.Get().Buffer());
                r->IsHlsl = GetImplementation(src, FBXSDK_IMPLEMENTATION_HLSL) != nullptr;
                r->IsCgfx = GetImplementation(src, FBXSDK_IMPLEMENTATION_HLSL) != nullptr;

                if (src->GetClassId().Is(FbxSurfacePhong::ClassId))
                {
                    auto mat = (FbxSurfacePhong*)src;
                    r->IsPhong = true;
                    r->Ambient = ToVector3(mat->Ambient.Get());
                    r->Diffuse = ToVector3(mat->Diffuse.Get());
                    r->Specular = ToVector3(mat->Specular.Get());
                    r->Emissive = ToVector3(mat->Emissive.Get());
                    r->Transparency = (float)mat->TransparencyFactor.Get();
                    r->Shininess = (float)mat->Shininess.Get();
                    r->Reflectivity = (float)mat->ReflectionFactor.Get();
                }
                else if (src->GetClassId().Is(FbxSurfaceLambert::ClassId))
                {
                    auto mat = (FbxSurfaceLambert*)src;
                    r->IsLambert = true;
                    r->Ambient = ToVector3(mat->Ambient.Get());
                    r->Diffuse = ToVector3(mat->Diffuse.Get());
                    r->Emissive = ToVector3(mat->Emissive.Get());
                    r->Transparency = (float)mat->TransparencyFactor.Get();
                }
                else
                {
                    r->Diffuse = Vector3(0.5, 0.5, 0.5);
                    r->Transparency = 0;
                }
                return r;
            }

            void ExtractNodeMaterials(FbxNode * src, DotNetFbxNode ^ node, DotNetFbxScene ^ scene)
            {
                for (int i = 0; i < src->GetMaterialCount(); i++)
                {
                    auto material = ConvertMaterial(src->GetMaterial(i));
                    if (!scene->Materials->ContainsKey(material->Name))
                    {
                        material->Index = scene->Materials->Count;
                        scene->Materials->Add(material->Name, material);
                    }
                    else
                    {
                        material = scene->Materials[material->Name];
                    }
                    node->Materials->Add(material);
                }
            }

            /*
            DAABox GetBoundingBox(FbxNode* pNode, DAABox box)
            {
                if (pNode->GetNodeAttribute())
                {
                    auto AttributeType = pNode->GetNodeAttribute()->GetAttributeType();
                    if (AttributeType == FbxNodeAttribute::eMesh)
                    {
                        auto& tform = pNode->EvaluateGlobalTransform();
                        auto translation = tform.GetT();
                        auto minX = std::min(box.Min.X, translation[0]);
                        auto minX = std::min(box.Min.X, translation[0]);
                        auto minX = std::min(box.Min.X, translation[0]);
                        //box = DAABox(min,
                    }
                }
            }
            */

            DotNetFbxNode^ ProcessNode(FbxNode * pNode, DotNetFbxScene ^ scene, int parentIndex, std::map<FbxMesh*, int>&meshMap, const FbxVector4 & center)
            {
                //DisplayMetaDataConnections(pNode);
                //DisplayProperties(pNode);

                // Create the result
                DotNetFbxNode^ r = gcnew DotNetFbxNode();
                auto nodeIndex = scene->Nodes->Count;
                scene->Nodes->Add(r);

                // Fill out the basic properties
                r->Index = nodeIndex;
                r->Name = ToString(pNode->GetName());
                r->ParentIndex = parentIndex;

                // Get node's default TRS properties as a transformation matrix
                auto& transform = pNode->EvaluateGlobalTransform();

                // Offset the transform by the center of the scene bounding box 
                transform.SetT(transform.GetT() - center);

                // Set the matrix
                r->Matrix = ToMatrix(transform);

                ExtractNodeMaterials(pNode, r, scene);

                r->MeshIndex = -1;
                if (pNode->GetNodeAttribute())
                {
                    auto AttributeType = pNode->GetNodeAttribute()->GetAttributeType();
                    if (AttributeType == FbxNodeAttribute::eMesh)
                    {
                        auto pMesh = (FbxMesh*)pNode->GetNodeAttribute();

                        if (meshMap.find(pMesh) != meshMap.end())
                        {
                            r->MeshIndex = meshMap[pMesh];
                        }
                        else
                        {
                            r->MeshIndex = scene->Meshes->Count;
                            auto mesh = ConvertMesh(pMesh, r->MeshIndex);
                            scene->Meshes->Add(mesh);
                            meshMap[pMesh] = r->MeshIndex;
                        }
                    }
                    else
                    {
                        const char* typeTable[] = {
                            "Unknown",
                            "Null",
                            "Marker",
                            "Skeleton",
                            "Mesh",
                            "Nurbs",
                            "Patch",
                            "Camera",
                            "CameraStereo",
                            "CameraSwitcher",
                            "Light",
                            "OpticalReference",
                            "OpticalMarker",
                            "NurbsCurve",
                            "TrimNurbsSurface",
                            "Boundary",
                            "NurbsSurface",
                            "Shape",
                            "LODGroup",
                            "SubDiv",
                            "CachedEffect",
                            "Line"
                        };
                        r->Type = ToString(typeTable[AttributeType]);
                    }
                }

                // Recursively process the children.
                for (int j = 0; j < pNode->GetChildCount(); j++)
                {
                    ProcessNode(pNode->GetChild(j), scene, nodeIndex, meshMap, center);
                }

                return r;
            }

            DotNetFbxScene^ DotNetFbxScene::Load(String ^ fileName)
            {
                auto r = gcnew DotNetFbxScene();
                DotNetFbxReaderWriter reader;
                if (!reader.InternalLoad(r, fileName))
                    return nullptr;
                return r;
            }

            bool DotNetFbxReaderWriter::InternalLoad(DotNetFbxScene ^ scene, String ^ fileName)
            {
                int lFileMajor, lFileMinor, lFileRevision;
                int lSDKMajor, lSDKMinor, lSDKRevision;
                //int lFileFormat = -1;
                bool lStatus;

                // Get the file version number generate by the FBX SDK.
                FbxManager::GetFileFormatVersion(lSDKMajor, lSDKMinor, lSDKRevision);

                // Create an importer.
                auto lImporter = FbxImporter::Create(mSdkManager, "");

                // Initialize the importer by providing a filename.
                auto pFileName = Marshal::StringToHGlobalAnsi(fileName);
                auto lImportStatus = lImporter->Initialize((const char*)(void*)pFileName, -1, mSdkManager->GetIOSettings());

                lImporter->GetFileVersion(lFileMajor, lFileMinor, lFileRevision);

                if (!lImportStatus)
                {
                    auto error = lImporter->GetStatus().GetErrorString();
                    printf("Call to FbxImporter::Initialize() failed.\n");
                    printf("Error returned: %s\n\n", error);

                    if (lImporter->GetStatus().GetCode() == FbxStatus::eInvalidFileVersion)
                    {
                        printf("FBX file format version for this FBX SDK is %d.%d.%d\n", lSDKMajor, lSDKMinor, lSDKRevision);
                        printf("FBX file format version for this file is %d.%d.%d\n\n", lFileMajor, lFileMinor, lFileRevision);
                    }

                    return false;
                }

                printf("FBX file format version for this FBX SDK is %d.%d.%d\n", lSDKMajor, lSDKMinor, lSDKRevision);

                if (lImporter->IsFBX())
                {
                    printf("FBX file format version for file is %d.%d.%d\n\n", lFileMajor, lFileMinor, lFileRevision);

                    // Set the import states. By default, the import states are always set to 
                    // true. The code below shows how to change these states.
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_MATERIAL, true);
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_TEXTURE, true);
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_LINK, true);
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_SHAPE, true);
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_GOBO, true);
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_ANIMATION, true);
                    mSdkManager->GetIOSettings()->SetBoolProp(IMP_FBX_GLOBAL_SETTINGS, true);
                }

                // Import the scene.
                lStatus = lImporter->Import(mScene);

                // Fail if a password is required
                if (lStatus == false && lImporter->GetStatus().GetCode() == FbxStatus::ePasswordError)
                {
                    printf("Password required");
                    exit(1);
                }

                // Destroy the importer.
                lImporter->Destroy();
                Marshal::FreeHGlobal(pFileName);

                auto* sceneInfo = mScene->GetDocumentInfo();
                scene->FileVersionMajor = lFileMajor;
                scene->FileVersionMinor = lFileMinor;
                scene->FileVersionRevision = lFileRevision;
                scene->Title = Ara3D::Serialization::FBX::ToString(sceneInfo->mTitle);
                scene->Subject = Ara3D::Serialization::FBX::ToString(sceneInfo->mSubject);
                scene->Author = Ara3D::Serialization::FBX::ToString(sceneInfo->mAuthor);
                scene->Keywords = Ara3D::Serialization::FBX::ToString(sceneInfo->mKeywords);
                scene->Revision = Ara3D::Serialization::FBX::ToString(sceneInfo->mRevision);
                scene->Comment = Ara3D::Serialization::FBX::ToString(sceneInfo->mComment);
                scene->Subject = Ara3D::Serialization::FBX::ToString(sceneInfo->mSubject);
                scene->ApplicationName = Ara3D::Serialization::FBX::ToString(sceneInfo->LastSaved_ApplicationName.Get());
                scene->ApplicationVendor = Ara3D::Serialization::FBX::ToString(sceneInfo->LastSaved_ApplicationVendor.Get());
                scene->ApplicationVersion = Ara3D::Serialization::FBX::ToString(sceneInfo->LastSaved_ApplicationVersion.Get());

                //DisplayGenericInfo(mScene);

                // Force triangulation
                FbxGeometryConverter geoConverter(mSdkManager);
                geoConverter.Triangulate(mScene, true);

                auto axisSytem = mScene->GetGlobalSettings().GetAxisSystem();

                FbxAxisSystem newAxisSystem(
                    FbxAxisSystem::EUpVector::eZAxis,
                    FbxAxisSystem::EFrontVector::eParityOdd,
                    FbxAxisSystem::ECoordSystem::eRightHanded);

                newAxisSystem.ConvertScene(mScene);

                const FbxSystemUnit::ConversionOptions lConversionOptions = {
                    false, // mConvertRrsNodes 
                    true, // mConvertLimits 
                    true, // mConvertClusters 
                    true, // mConvertLightIntensity 
                    true, // mConvertPhotometricLProperties 
                    true  // mConvertCameraClipPlanes 
                };

                // Convert the scene to feet 
                //FbxSystemUnit::Foot.ConvertScene(mScene, lConversionOptions);

                FbxVector4 min, max, center;
                mScene->ComputeBoundingBoxMinMaxCenter(min, max, center);

                auto lRootNode = mScene->GetRootNode();
                if (lRootNode)
                {
                    ProcessNode(lRootNode, scene, -1, std::map<FbxMesh*, int>(), center);
                }

                return lStatus;
            }
        }
    }
};
