using Ara3D.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Identification
{
    public class Entity
    {
        public string Id = Guid.NewGuid().ToString();
        public List<GeometryComponent> Geometries = new List<GeometryComponent>();
        public List<ParameterSetComponent> ParameterSets = new List<ParameterSetComponent>();
    }

    public class IdentityModel
    {
        public IdentityModel(IEnumerable<Entity> entities)
        {
            Entities = entities.OrderBy(e => e.Id).ToList();
            foreach (var entity in Entities)
            {
                entity.Geometries.SortInPlaceBy(c => c.LocalElementId);
                entity.ParameterSets.SortInPlaceBy(c => c.LocalElementId);
            }
        }

        public IdentityModel()
        { }

        public readonly List<Entity> Entities = new List<Entity>();
        
        private Dictionary<string, Entity> _entityLookup;

        public Dictionary<string, Entity> GetLookup()
        {
            return _entityLookup ?? (_entityLookup = Entities.ToDictionary(e => e.Id, e => e));
        }

        public static IdentityModel CreateFromFolder(DirectoryPath dir, string software)
        {
            var d = new Dictionary<string, Entity>();

            var sourceFile = "";
            foreach (var f in dir.GetFiles("*.json"))
            {
                var e = new Entity();
                var id = f.GetFileNameWithoutExtension();
                e.Id = id;
                var text = f.ReadAllText();
                d.Add(id, e);
                var ps = JsonSerializer.Deserialize<Dictionary<string, string>>(text, JsonOptions);
                ps.TryGetValue("filename", out sourceFile);

                var comp = new ParameterSetComponent
                {
                    Parameters = ps,
                    SourceSoftware = software,
                    EntityId = e.Id,
                    SourceFileName = sourceFile,
                    LocalElementId = id,
                };
                e.ParameterSets.Add(comp);
            }

            foreach (var f in dir.GetFiles("*.obj"))
            {
                var id = f.GetFileNameWithoutExtension();
                var objMesh = ObjMesh.Load(f);
                var e = new Entity();
                if (d.ContainsKey(id))
                    e = d[id];
                var comp = new GeometryComponent
                {
                    EntityId = e.Id,
                    FileName = f,
                    SourceFileName = sourceFile,
                    SourceSoftware = software,
                    LocalBoundingBox = objMesh.Box,
                    LocalElementId = id,
                };
                e.Geometries.Add(comp);
            }

            return new IdentityModel(d.Values);
        }

        public static JsonSerializerOptions JsonOptions =
            new JsonSerializerOptions()
            {
                IncludeFields = true,
                WriteIndented = true,
                IgnoreReadOnlyProperties = true,
            };

    public static IdentityModel Load(FilePath filePath)
        {
            return new IdentityModel(
                JsonSerializer.Deserialize<List<Entity>>(filePath.ReadAllText(), JsonOptions));
        }

        public FilePath Serialize(FilePath filePath)
        {
            var json = JsonSerializer.Serialize(Entities, JsonOptions);
            return filePath.WriteAllText(json);
        }

        public IEnumerable<Entity> GetRemovedEntities(IdentityModel newModel)
            => Entities.Where(e => !newModel.GetLookup().ContainsKey(e.Id));

        public IEnumerable<Entity> GetAddedEntities(IdentityModel newModel)
            => newModel.GetRemovedEntities(this);
        }
}