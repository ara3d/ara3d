using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ara3D.Geometry;

public class PolygonTester : MonoBehaviour
{
    public int Count = 4;
    public float Spacing = 3; 
    public float Size = 2;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        var mesh = Polygons.Triangle.To3D;
    }
}
