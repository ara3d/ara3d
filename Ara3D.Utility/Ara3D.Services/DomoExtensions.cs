using Ara3D.Domo;
using System;
using System.Linq;
using Ara3D.Utils;
using Newtonsoft.Json;

namespace Ara3D.Services
{
    public static class DomoExtensions
    {
        public static JsonText ToJson(this IRepository repo)
            => JsonConvert.SerializeObject(repo.GetValues().ToArray(), Formatting.Indented);

        public static T LoadFromJson<T>(this T repo, JsonText content) where T: IRepository
        {
            var type = repo.ValueType.MakeArrayType();
            var tmp = (Array)JsonConvert.DeserializeObject(content, type);

            if (repo.IsSingleton)
            {
                if (tmp.Length != 1) throw new Exception("Singleton repository can only have one item");
                var model = repo.GetSingleModel();
                model.Value = tmp.GetValue(0);
            }
            else
            {
                repo.Clear();
                for (var i = 0; i < tmp.Length; ++i)
                {
                    repo.Add(Guid.NewGuid(), tmp.GetValue(i));
                }
            }

            return repo;
        }
    }
}
