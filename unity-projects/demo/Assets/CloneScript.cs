using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CloneScript : MonoBehaviour
{
    public int NumRows = 5;
    public int NumColumns = 5;
    public int SpacingRows = 2;
    public int SpacingColumns = 2;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    
    public void DrawMesh(Vector3 position)
    {
        var mesh = GetComponent<MeshFilter>().sharedMesh;
        var material = GetComponent<MeshRenderer>().sharedMaterial;
        Graphics.DrawMesh(mesh, position, Quaternion.identity, material, 0);
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < NumRows; i++)
        {
            for (var j = 0; j < NumColumns; j++)
            {
                DrawMesh(new(j * SpacingColumns, 0, i * SpacingRows));
            }
        }
    }
}
