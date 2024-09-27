using System.Collections.Generic;
using System.Linq;

namespace Ara3D.StepParser
{
    public class StepInstance
    {
        public readonly StepEntity Entity;
        public readonly int Id;

        public List<StepValue> AttributeValues
            => Entity.Attributes.Values;

        public string EntityType
            => Entity?.EntityType.ToString() ?? "";

        public StepInstance(int id, StepEntity entity)
        {
            Id = id;
            Entity = entity;
        }

        public bool IsEntityType(string str)
            => EntityType == str;

        public override string ToString()
            => $"#{Id}={Entity};";

        public int Count 
            => AttributeValues.Count;

        public StepValue this[int i]
            => i < Count ? AttributeValues[i] : null;
    }

}