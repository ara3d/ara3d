using Ara3D.Utils;
using System.Net.Http.Headers;
using System.Text;
using Ara3D.Parsing.Json;

namespace Ara3D.DevOpToolsAsTests
{
    public static class DevOpTools
    {
        public static IEnumerable<FilePath> GetAllProjects()
            => SourceCodeLocation.GetFolder().RelativeFolder("..", "..").GetFiles("*.csproj", true);
        
        // NOTE: Does not work as hoped. Causes bugs in system
        [Test, Explicit]
        public static void UpgradeVersion()
        {
            throw new Exception("Does not work");
            var oldVer = "1.3.1";
            var newVer = "1.4.0";

            foreach (var project in GetAllProjects())
            {
                Console.WriteLine($"Loading {project}");
                project.LoadXml()
                    .SetAttributesWhere(x => x.Name == "PackageReference"
                                             && x.Attribute("Include")?.Value?.StartsWith("Ara3D.") == true
                                             && x.Attribute("Version")?.Value?.Equals(oldVer) == true,
                        "Version",
                        newVer)
                    .SaveXml(project);
            }
        }

        public static HttpContent EncodeFormContent(params (string key, string value)[] formData) 
            => new FormUrlEncodedContent(formData.Select(tuple => new KeyValuePair<string, string>(tuple.key, tuple.value)));

        // https://stackoverflow.com/questions/5152723/curl-with-user-authentication-in-c-sharp
        // https://stackoverflow.com/questions/7929013/making-a-curl-call-in-c-sharp
        // https://stackoverflow.com/questions/21255725/webrequest-equivalent-to-curl-command
        public static async Task<string> PostWithAuthentication(string baseUrl, string path, string user, string pass, params (string key, string value)[] formData)
        {
            var authValue = new AuthenticationHeaderValue("Basic", 
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}")));
            try
            {
                var client = new HttpClient { DefaultRequestHeaders = { Authorization = authValue } };
                client.BaseAddress = new Uri(baseUrl);
                var content = EncodeFormContent(formData);
                var response = await client.PostAsync($"/{path}", content);
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        public static FilePath GithubApiKeyFile
            => @"C:\Users\cdigg\api-keys\github-public-api-token.txt";

        public static string GithubApiKey
            => GithubApiKeyFile.ReadAllText();

        // curl -d "text=This is a block of text" 
        // http://api.repustate.com/v2/demokey/score.json
        // curl -u YOUR_CLIENT_ID:YOUR_CLIENT_SECRET -I https://api.github.com/meta
        // curl -u YOUR_CLIENT_ID:YOUR_CLIENT_SECRET -I https://api.github.com/meta

        public static string[] Repos =
        {
            "ara3d/ara3d",
            "cdiggins/plato",
            "ara3d/parakeet",
        };

        // https://gist.github.com/MaximRouiller/74ae40aa994579393f52747e78f26441
        public static async Task<string> GithubApiQuery(string path, string token)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            var response = await client.GetAsync($"/{path}");
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsStringAsync().Result;
        }

        [TestCaseSource(nameof(Repos))]
        public static void QueryGithub(string repoName)
        {
            var response = GithubApiQuery($"repos/{repoName}", GithubApiKey).Result;
            Console.WriteLine(response);
            var parser = new JsonParser(response);
            var xml = parser.Parser.ParseXml;
            Console.WriteLine(xml);
            var prettifiedJson = parser.Root.BuildString().ToString();
            Console.WriteLine(prettifiedJson);
        }

    }
}