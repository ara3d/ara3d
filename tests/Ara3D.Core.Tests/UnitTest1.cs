using Ara3D.Domo;
using Ara3D.Services;
using Ara3D.Utils;
using NUnit;
using NUnit.Framework.Legacy;

namespace Ara3D.Core.Tests
{
    public class TestModelData
    {
        public FilePath FilePath { get; init; }
    }

    public class RepoTests
    {
        [Test]
        public static void TestBasicRepoUsageAndSerialization()
        {
            var folders = SpecialFolders.Temp;
            var tempFile = PathUtil.CreateTempFile();
            var repo = new SingletonRepository<TestModelData>();
            ClassicAssert.IsTrue(repo.Value.FilePath.Value.IsNullOrEmpty());
            repo.GetDynamicModel().FilePath = tempFile;
            ClassicAssert.AreEqual(tempFile, repo.GetDynamicModel().FilePath);
            ClassicAssert.AreEqual(tempFile, repo.Value.FilePath);

            // TODO: it doesn't really make sense to have the "ToJson" in the services does it? 
            // makes the code less discoverable. 
            var json = repo.ToJson();
            tempFile.WriteAllText(json);

            repo.Update(m => new TestModelData { FilePath = "Razzle" });
            ClassicAssert.AreEqual("Razzle", repo.Value.FilePath.Value);
            repo.LoadFromJson(json);
            ClassicAssert.AreEqual(tempFile, repo.Value.FilePath);
        }
    }
}