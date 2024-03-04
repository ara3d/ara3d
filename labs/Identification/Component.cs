using System;
using System.Collections.Generic;
using Ara3D.Mathematics;

namespace Identification
{
    // When generating components, use these 
    public static class Options
    {
        public static string CurrentSourceFileName;
        public static string CurrentSourceSoftware;
    }

    /// <summary>
    /// A component is a piece of data related to an entity, generated from some
    /// piece of software. 
    /// </summary>
    public class Component
    {
        public string EntityId;
        public Guid ComponentId;
        public string ComponentType;
        public string SourceFileName = Options.CurrentSourceFileName;
        public string SourceSoftware = Options.CurrentSourceSoftware;
        public string LocalElementId;

        public Component()
        {
            ComponentType = GetType().Name;
            if (ComponentType.EndsWith("Component"))
                ComponentType = ComponentType.Substring(0, ComponentType.LastIndexOf("Component", StringComparison.Ordinal));
            ComponentId = Guid.NewGuid();
        }
    }

    public class MetaDataComponent : Component
    {
        public string Entity;
        public string Project;
        public string Building;
        public string Floor;
        public string Room;
        public int Asset;
    }

    public class BksComponent : Component
    {
        public string Bks;
    }

    public class ParameterSetComponent : Component
    {
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
    }

    public class LocationComponent : Component
    {
        public DVector3 Position;
    }

    public class DimensionsComponent : Component
    {
        public DVector3 Dimensions;
    }

    public class GeometryComponent : Component
    {
        public string FileName;
        public DAABox LocalBoundingBox;
    }
}