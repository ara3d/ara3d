using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Web;

namespace GithubAuthTest
{
    public class JsonHttpClient : HttpClient
    {
        public JsonHttpClient()
        {
            DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // https://stackoverflow.com/questions/4015324/send-http-post-request-in-net
        public async Task<JsonNode> PostAsync(string uri, params (string, string)[] parameters)
        {
            var d = parameters.ToDictionary(pair => pair.Item1, pair => pair.Item2);
            var content = new FormUrlEncodedContent(d);
            var response = await PostAsync(uri, content);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonNode.Parse(responseString);
        }

        // https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get
        public async Task<JsonNode> GetAsync(string uri, params (string, string)[] parameters)
        {
            var builder = new UriBuilder(uri);
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var param in parameters)
            {
                query[param.Item1] = param.Item2;
            }
            builder.Query = query.ToString();
            var url = builder.ToString();
            var response = await base.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonNode.Parse(responseString);
        }
    }

    // https://api.github.com/user

    // Modified from: https://docs.github.com/en/apps/creating-github-apps/writing-code-for-a-github-app/building-a-cli-with-a-github-app
    public static class Program
    {
        public static JsonHttpClient Client = new();

        public static string ClientId = "bdba7cf4ee5ceea3b644";
        public static string GrantType = "urn:ietf:params:oauth:grant-type:device_code";
        public static string AccessTokenUrl = "https://github.com/login/oauth/access_token";
        public static string UserApiUrl = "https://api.github.com/user";
        public static string DeviceCodeUrl = "https://github.com/login/device/code";

        public static async Task<string?> PollRequestToken(string deviceCode, int interval)
        {
            while (true)
            {
                var response = await RequestToken(deviceCode);
                var error = response["error"]?.ToString();
                var accessToken = response["access_token"]?.ToString();
                switch (error)
                {
                    case "authorization_pending":
                        Console.WriteLine("Waiting for authorization");
                        Thread.Sleep(interval * 1000);
                        break;
                    case "slow_down":
                        Console.WriteLine("Slowing down");
                        Thread.Sleep((interval += 5) * 1000);
                        break;
                    case "expired_token":
                        Console.WriteLine("The device code has expired. Please run `login` again.");
                        return null;
                    case "access_denied":
                        Console.WriteLine("Login cancelled by user.");
                        return null;
                    case null:
                        return accessToken;
                    default:
                        Console.WriteLine($"Unknown error {error}");
                        return null;
                }
            }
        }

        public static async Task<JsonNode> RequestToken(string deviceCode)
        {
            return await Client.PostAsync(
                AccessTokenUrl, 
                ("client_id", ClientId),
                ("device_code", deviceCode),
                ("grant_type", GrantType));
        }

        public static async Task WhoAmI()
        {
            var response = await Client.GetAsync(UserApiUrl);
            Console.WriteLine(response);
        }

        public static async Task Main(string[] args)
        {
            //var apiKeyFile = PathUtil.GetCallerSourceFolder().RelativeFile("..", "..", "..", "..", "api-keys", "github-oauth-app-ara3d.txt");
            //var clientSecret = apiKeyFile.ReadAllText();

            var response = await Client.PostAsync(DeviceCodeUrl, ("client_id", ClientId));
            var verificationUri = response["verification_uri"];
            var deviceCode = response["device_code"]?.ToString();
            var userCode = response["user_code"];
            Console.WriteLine($"Go to {verificationUri} and enter {userCode}");

            var accessToken = await PollRequestToken(deviceCode, 5);

            Console.WriteLine($"Access token = {accessToken}");
            Client.DefaultRequestHeaders.Add("User-Agent", "Ara 3D");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
            await WhoAmI();

            /*
https://api.github.com/user

def whoami
  uri = URI("https://api.github.com/user")

  begin
    token = File.read("./.token").strip
  rescue Errno::ENOENT => e
    puts "You are not authorized. Run the `login` command."
    exit 1
  end

  response = Net::HTTP.start(uri.host, uri.port, use_ssl: true) do |http|
    body = {"access_token" => token}.to_json
    headers = {
            "Accept" => "application/vnd.github+json", 
            "Authorization" => "Bearer #{token}"}
             */
        }
    }
}