using Ara3D.Domo;
using Ara3D.Services;
using Ara3D.Utils;

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
            Assert.IsTrue(repo.Value.FilePath.Value.IsNullOrEmpty());
            repo.GetDynamicModel().FilePath = tempFile;
            Assert.AreEqual(tempFile, repo.GetDynamicModel().FilePath);
            Assert.AreEqual(tempFile, repo.Value.FilePath);

            // TODO: it doesn't really make sense to have the "ToJson" in the services does it? 
            // makes the code less discoverable. 
            var json = repo.ToJson();
            tempFile.WriteAllText(json);

            repo.Update(m => new TestModelData { FilePath = "Razzle" });
            Assert.AreEqual("Razzle", repo.Value.FilePath.Value);
            repo.LoadFromJson(json);
            Assert.AreEqual(tempFile, repo.Value.FilePath);
        }
    }
}