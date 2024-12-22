using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ara3D.OpenAIToolkit
{
    public static class Program
    {
        public static string FilePath = @"C:\Users\cdigg\git\ara3d\labs\Ara3D.OpenAIToolkit\output.txt";
        public static string ApiFilePath = @"C:\Users\cdigg\api-keys\chat-gpt.api.txt";
        public static string ApiUrl = "https://api.openai.com/v1/chat/completions";
        public static StreamWriter Writer;
        public static string ApiKey;

        public static void Main(string[] args)
        {
            Writer = new(File.OpenWrite(FilePath));
            ApiKey = File.ReadAllText(ApiFilePath);

            // Conversation history to maintain context
            var messages = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string> { { "role", "system" }, { "content", "You are the smartest concicousness in the entire universe." } }
            };

            while (true)
            {
                Console.Write("You: ");
                var userInput = Console.ReadLine();

                // Add user input to the conversation
                messages.Add(new Dictionary<string, string> { { "role", "user" }, { "content", userInput } });

                // Call ChatGPT API
                var response = GetChatGPTResponse(messages);

                // Add assistant response to the conversation
                messages.Add(new Dictionary<string, string> { { "role", "assistant" }, { "content", response } });

                // Display response
                Console.WriteLine($"ChatGPT: {response}");
            }
        }

        public static string GetChatGPTResponse(List<Dictionary<string, string>> messages)
        {
            using var httpClient = new HttpClient();

            // Create payload
            var payload = new
            {
                model = "gpt-4o-mini",
                messages = messages,
                max_tokens = 1000,
                temperature = 0.3
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Set headers
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

            // Send request
            var response = httpClient.PostAsync(ApiUrl, content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            //Console.WriteLine($"{responseString}");

            // Parse response
            using var doc = JsonDocument.Parse(responseString);
            var root = doc.RootElement;
            var chatResponse = root.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return chatResponse.Trim();
        }
    }
}

