using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Chat;

namespace CodingTutor
{
    public class ChatService
    {
        public OpenAIAPI Api { get; }
        public Conversation Conversation { get; }
        public string Prompt { get; set; } = "";

        public ChatService()
        {
            var modelName = "gpt-4-1106-preview";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var apiKeyFile = Path.Combine(folder, "api-keys", "chat-gpt.api.txt");
            var key = File.ReadAllText(apiKeyFile);
            Api = new OpenAIAPI(key);

            var chatRequest = new ChatRequest
            {
                Model = modelName // Replace with the model version you want to use, e.g., "davinci", "curie", etc.
            };
            Conversation = Api.Chat.CreateConversation(chatRequest);
        }

        public async Task SendPromptAsync(Action<int, string> handler)
        {
            Conversation.AppendUserInput(Prompt);
            await Conversation.StreamResponseFromChatbotAsync(handler);
        }
    }
}