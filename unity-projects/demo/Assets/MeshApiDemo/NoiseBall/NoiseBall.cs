// Adapted from Keijiro's NoiseBall2 project from 2017
// https://github.com/keijiro/NoiseBall2

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class NoiseBall : MonoBehaviour
{
    public enum Mode
    {
        CPU,
        CPUBurst,
        CPUBurstThreaded,
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2
        GPU
#endif
    }

    public Mode m_Mode = Mode.CPUBurstThreaded;
    public int m_TriangleCount = 100000;
    public float m_TriangleExtent = 0.1f;
    public float m_ShuffleSpeed = 4.0f;
    public float m_NoiseAmplitude = 1.0f;
    public float m_NoiseFrequency = 1.0f;
    public Vector3 m_NoiseMotion = Vector3.up;

    public ComputeShader m_ComputeShader;

    Mesh m_Mesh;
    NativeArray<Vector3> m_VertexPos;
    NativeArray<Vector3> m_VertexNor;
    Vector3 m_NoiseOffset;
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2    
    GraphicsBuffer m_BufferPos;
    GraphicsBuffer m_BufferNor;
#endif

    GUIContent[] m_UIOptions;
    
    const int kThreadCount = 64;

    public void OnValidate()
    {
        m_TriangleCount = Mathf.Max(0, m_TriangleCount);
        m_TriangleExtent = Mathf.Max(0, m_TriangleExtent);
        m_NoiseFrequency = Mathf.Max(0, m_NoiseFrequency);
    }

    public void OnDestroy()
    {
        CleanupResources();
    }

    void CleanupResources()
    {
        DestroyImmediate(m_Mesh);
        m_Mesh = null;
        if (m_VertexPos.IsCreated) m_VertexPos.Dispose();
        if (m_VertexNor.IsCreated) m_VertexNor.Dispose();
        CleanupGpuResources();
    }

    void CleanupGpuResources()
    {
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2        
        m_BufferPos?.Dispose();
        m_BufferPos = null;
        m_BufferNor?.Dispose();
        m_BufferNor = null;
#endif
    }

    public void Update()
    {
        if (m_Mesh && m_Mesh.vertexCount != m_TriangleCount * 3)
        {
            CleanupResources();
        }

        if (m_Mesh == null)
        {
            m_Mesh = new Mesh();
            m_Mesh.name = "NoiseBallMesh";
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2            
            m_Mesh.vertexBufferTarget |= GraphicsBuffer.Target.Raw;
#endif
            
            m_Mesh.SetVertexBufferParams(m_TriangleCount * 3, new VertexAttributeDescriptor(VertexAttribute.Position, stream:0), new VertexAttributeDescriptor(VertexAttribute.Normal, stream:1));
            m_VertexPos = new NativeArray<Vector3>(m_TriangleCount * 3, Allocator.Persistent);
            m_VertexNor = new NativeArray<Vector3>(m_TriangleCount * 3, Allocator.Persistent);
            
            m_Mesh.SetIndexBufferParams(m_TriangleCount * 3, IndexFormat.UInt32);
            var ib = new NativeArray<int>(m_TriangleCount * 3, Allocator.Temp);
            for (var i = 0; i < m_TriangleCount * 3; ++i)
                ib[i] = i;
            m_Mesh.SetIndexBufferData(ib, 0, 0, ib.Length, MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);
            ib.Dispose();
            var submesh = new SubMeshDescriptor(0, m_TriangleCount * 3, MeshTopology.Triangles);
            submesh.bounds = new Bounds(Vector3.zero, new Vector3(10, 10, 10));
            m_Mesh.SetSubMesh(0, submesh);
            m_Mesh.bounds = submesh.bounds;
            GetComponent<MeshFilter>().sharedMesh = m_Mesh;
        }
        
        UpdateMesh(Time.time);
        
        // move the noise field
        m_NoiseOffset += m_NoiseMotion * Time.deltaTime;
    }
    
    [BurstCompile]
    struct NoiseMeshJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeArray<Vector3> vertices;
        [NativeDisableParallelForRestriction] public NativeArray<Vector3> normals;
        public float pTime;
        public float pExtent;
        public float pNoiseFrequency;
        public float pNoiseAmplitude;
        public float3 pNoiseOffset;

        public void Execute(int id)
        {
            int idx1 = id * 3;
            int idx2 = id * 3 + 1;
            int idx3 = id * 3 + 2;

            float seed = floor(pTime + id * 0.1f) * 0.1f;
            float3 v1 = RandomPoint(idx1 + seed);
            float3 v2 = RandomPoint(idx2 + seed);
            float3 v3 = RandomPoint(idx3 + seed);

            v2 = normalize(v1 + normalize(v2 - v1) * pExtent);
            v3 = normalize(v1 + normalize(v3 - v1) * pExtent);

            float l1 = SimplexNoise3D.snoise(v1 * pNoiseFrequency + pNoiseOffset).w;
            float l2 = SimplexNoise3D.snoise(v2 * pNoiseFrequency + pNoiseOffset).w;
            float l3 = SimplexNoise3D.snoise(v3 * pNoiseFrequency + pNoiseOffset).w;

            l1 = abs(l1 * l1 * l1);
            l2 = abs(l2 * l2 * l2);
            l3 = abs(l3 * l3 * l3);

            v1 *= 1 + l1 * pNoiseAmplitude;
            v2 *= 1 + l2 * pNoiseAmplitude;
            v3 *= 1 + l3 * pNoiseAmplitude;

            float3 n = normalize(cross(v2 - v1, v3 - v2));

            vertices[idx1] = v1;
            vertices[idx2] = v2;
            vertices[idx3] = v3;
            normals[idx1] = n;
            normals[idx2] = n;
            normals[idx3] = n;
        }
    }
    
    void UpdateMesh(float t)
    {
        var job = new NoiseMeshJob
        {
            pTime = t * m_ShuffleSpeed,
            pExtent = m_TriangleExtent * (cos(t*1.3f) * 0.3f + 1),
            pNoiseFrequency = m_NoiseFrequency * (sin(t) * 0.5f + 1),
            pNoiseAmplitude = m_NoiseAmplitude * (cos(t*1.7f) * 0.3f + 1),
            pNoiseOffset = m_NoiseOffset,
            vertices = m_VertexPos,
            normals = m_VertexNor
        };
        
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2        
        if (m_Mode == Mode.GPU)
        {
            m_BufferPos ??= m_Mesh.GetVertexBuffer(0);
            m_BufferNor ??= m_Mesh.GetVertexBuffer(1);
            
            m_ComputeShader.SetFloat("pTime", job.pTime);
            m_ComputeShader.SetFloat("pExtent", job.pExtent);
            m_ComputeShader.SetFloat("pNoiseFrequency", job.pNoiseFrequency);
            m_ComputeShader.SetFloat("pNoiseAmplitude", job.pNoiseAmplitude);
            m_ComputeShader.SetVector("pNoiseOffset", new Vector4(job.pNoiseOffset.x, job.pNoiseOffset.y, job.pNoiseOffset.z, 0));
            m_ComputeShader.SetInt("pTriCount", m_TriangleCount);
            m_ComputeShader.SetBuffer(0, "BufVertices", m_BufferPos);
            m_ComputeShader.SetBuffer(0, "BufNormals", m_BufferNor);
            m_ComputeShader.Dispatch(0, (m_TriangleCount+kThreadCount-1)/kThreadCount, 1, 1);
            return;
        }
#endif
        
        if (m_Mode == Mode.CPU)
        {
            for (int id = 0; id < m_TriangleCount; ++id)
                job.Execute(id);
        }
        else if (m_Mode == Mode.CPUBurst)
        {
            job.Schedule(m_TriangleCount, m_TriangleCount).Complete();
        }
        else if (m_Mode == Mode.CPUBurstThreaded)
        {
            job.Schedule(m_TriangleCount, 4).Complete();
        }
        m_Mesh.SetVertexBufferData(m_VertexPos, 0, 0, m_VertexPos.Length, 0, MeshUpdateFlags.DontRecalculateBounds);
        m_Mesh.SetVertexBufferData(m_VertexNor, 0, 0, m_VertexNor.Length, 1, MeshUpdateFlags.DontRecalculateBounds);
        CleanupGpuResources();
    }
    
    static float Random(float u, float v)
    {
        float f = dot(float2(12.9898f, 78.233f), float2(u, v));
        return frac(43758.5453f * sin(f));
    }

    static float3 RandomPoint(float id)
    {
        float u = Random(id * 0.01334f, 0.3728f) * math.PI * 2;
        float z = Random(0.8372f, id * 0.01197f) * 2 - 1;
        return float3(float2(cos(u), sin(u)) * sqrt(1 - z * z), z);
    }

    static int TrisToIndex(int tris)
    {
        return tris / 100000;
    }

    static int IndexToTris(int index)
    {
        return index * 100000;
    }

    public void OnGUI()
    {
        if (m_UIOptions == null)
        {
            m_UIOptions = new[]
            {
                new GUIContent("C# 1 thread"),
                new GUIContent("Burst 1 thread"),
                new GUIContent("Burst threaded"),
#if UNITY_2021_2_OR_NEWER // Mesh GPU buffer access is since 2021.2            
                new GUIContent("GPU compute"),
#endif
            };
        }
        GUI.matrix = Matrix4x4.Scale(Vector3.one * 2);
        GUILayout.BeginArea(new Rect(5,25,420,80), "Options", GUI.skin.window);
        m_Mode = (Mode)GUILayout.Toolbar((int)m_Mode, m_UIOptions);
        GUILayout.BeginHorizontal();
        GUILayout.Label($"Triangles: {m_TriangleCount/1000}K", GUILayout.Width(120));

        int triIndex = TrisToIndex(m_TriangleCount);
        int newTriIndex = Mathf.RoundToInt(GUILayout.HorizontalSlider(triIndex, 1, 10));
        if (triIndex != newTriIndex)
            m_TriangleCount = IndexToTris(newTriIndex);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}
