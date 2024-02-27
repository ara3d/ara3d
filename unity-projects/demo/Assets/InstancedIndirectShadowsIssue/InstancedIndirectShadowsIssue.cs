using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// This demo shows the use of the procedural instancing features to render objects
/// without need of any position buffer. The values are calculated direclty inside the 
/// shader. 
/// Shadowing is broken when more than one DrawMeshInstancedIndirect is made. 
/// - Tested with different materials, same materials.
/// - Different bounds settings.
/// - Same args buffer (for different draw calls with same mesh).
/// - Different args buffers (with same mesh, and with different meshes).
/// WorkAround: adding an unique mpb per draw call apparently works!
/// 
/// The color buffer is used for debug only.
/// </summary>
[ExecuteAlways]
public class InstancedIndirectShadowsIssue : MonoBehaviour
{
    public int gridDim = 1000;
    public int instanceCount = 0;
    public Material instanceMaterial;

    public ShadowCastingMode castShadows = ShadowCastingMode.Off;
    public bool receiveShadows = false;

    private ComputeBuffer colorBuffer;

    private uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
	private Material[] materials;

	public Mesh[] meshes;
	private ComputeBuffer[] argsBuffers;
	public Shader shader;

	public bool[] render;

	MaterialPropertyBlock[] mpbs;

	void Start()
	{
        instanceCount = gridDim * gridDim;
		
		argsBuffers = new ComputeBuffer[meshes.Length];
		for (int i = 0; i < meshes.Length; i++)
		{
			argsBuffers[i] = new ComputeBuffer(5, sizeof(uint), ComputeBufferType.IndirectArguments);
		}

		mpbs = new MaterialPropertyBlock[meshes.Length];
		materials = new Material[meshes.Length];
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i] = new Material(instanceMaterial);
			mpbs[i] = new MaterialPropertyBlock();
		}

		CreateBuffers();
	}

	void Update()
	{
		
		for (int i = 0; i < meshes.Length; i++)
		{
			materials[i].SetFloat("_Dim", gridDim);
			materials[i].SetVector("_Pos", new Vector4(i * (gridDim + 10), 0, 0, 0));
			materials[i].SetBuffer("colorBuffer", colorBuffer);
			
			/// this is the magic line. Uncomment this for shadows!! 
			//mpbs[i].SetFloat("_Bla", (float)i);

			if (render[i])
				Graphics.DrawMeshInstancedIndirect(meshes[i], 0, materials[i], meshes[i].bounds, argsBuffers[i], 0, mpbs[i], castShadows, receiveShadows);
		}
	}

    void CreateBuffers()
	{ 
		/// Colors - for debug only
        if (colorBuffer != null)
			colorBuffer.Release();

        colorBuffer = new ComputeBuffer(instanceCount, 16);

		Vector4[] colors = new Vector4[instanceCount];
        for (int i = 0; i < instanceCount; i++)
            colors[i] = Random.ColorHSV();

        colorBuffer.SetData(colors);

		// avoid culling
		for (int i = 0; i < meshes.Length; i++)
		{
			meshes[i].bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);
		}

		// indirect args
		for (int i = 0; i < argsBuffers.Length; i++)
		{
			args[0] = meshes[i].GetIndexCount(0);
			args[1] = (uint)instanceCount;
			argsBuffers[i].SetData(args);
		}
	}

    void OnDestroy()
	{
        if (colorBuffer != null)
			colorBuffer.Release();
        colorBuffer = null;

		for (int i = 0; i < argsBuffers.Length; i++)
		{
			if(argsBuffers[i] != null)
				argsBuffers[i].Release();
		}
		argsBuffers = null;
	}

    void OnGUI()
    {
        GUI.Label(new Rect(265, 12, 200, 30), "Instance Count: " + instanceCount.ToString("N0"));
    }
}
