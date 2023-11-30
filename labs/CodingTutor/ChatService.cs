using System;
using System.IO;
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
            const string modelName = "gpt-4-1106-preview";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var apiKeyFile = Path.Combine(folder, "api-keys", "chat-gpt.api.txt");
            var key = File.ReadAllText(apiKeyFile);
            Api = new OpenAIAPI(key);

            var chatRequest = new ChatRequest
            {
                Model = modelName 
            };
            Conversation = Api.Chat.CreateConversation(chatRequest);
        }

        public void SendSystemPrompt(string content)
        {
            Conversation.AppendSystemMessage(content);
        }

        public async Task SendPromptAsync(string prompt, Action<int, string> handler)
        {
            Prompt = prompt;
            await SendPromptAsync(handler);
        }

        public async Task SendPromptAsync(Action<int, string> handler)
        {
            Conversation.AppendUserInput(Prompt);
            await Conversation.StreamResponseFromChatbotAsync(handler);
        }
    }
}