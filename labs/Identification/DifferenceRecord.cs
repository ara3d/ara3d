using System.Collections.Generic;
using Ara3D.Mathematics;

namespace Identification
{
    public class DifferenceRecord
    {
        public string Name;

        public bool DidCenterPointChange;
        public Vector3 OldCenterPoint;
        public Vector3 NewCenterPoint;
        public Vector3 CenterPointChange;

        public bool DidDimensionsChange;// => DimensionsChange.AlmostZero();
        public Vector3 OldDimensions;
        public Vector3 NewDimensions;
        public Vector3 DimensionsChange;// => NewDimensions - OldDimensions;

        public Dictionary<string, string> OldParameters = new Dictionary<string, string>();
        public Dictionary<string, string> NewParameters = new Dictionary<string, string>();

        public Dictionary<string, string> ParametersAdded = new Dictionary<string, string>();
        public Dictionary<string, string> ParametersChanged = new Dictionary<string, string>();
        public Dictionary<string, string> ParametersRemoved = new Dictionary<string, string>();

        public bool DidGeometryChange;
        public double GeometryChangeDelta;

        // Path to mesh in OBJ format 
        public string GeometryObjFile;
    }
}