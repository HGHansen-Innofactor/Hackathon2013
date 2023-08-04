using Azure;

using Azure.AI.OpenAI;
using System.ComponentModel;

namespace Hackathon2023.Data
{

    public class OpenAIService
    {
        OpenAIClient client = new OpenAIClient(
    new Uri("https://openai-henriksbodega.openai.azure.com/"),
    new AzureKeyCredential("878c71d3f0a249e39c2332a8f12bad2a"));

        ChatCompletionsOptions ChatContext = new ChatCompletionsOptions()
        {
            Messages =
                    {
                new ChatMessage(ChatRole.System, @"You are an AI assistant that helps people find information.")
                    },
            Temperature = (float)0.7,
            MaxTokens = 2000,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            NucleusSamplingFactor = (float)0.95
        };

        public List<ChatMessage> ChatMessages { get { return ChatContext.Messages.ToList(); } }
        private string Model { get; set; } = "GPT-Test1";
        private AIType Site { get; set; } = OpenAIService.AIType.CV;

        public void Init(double temp, double topP, string model, string initText, AIType siteType)
        {
            Model = model;

            ChatMessage systemstart;
            switch (siteType)
            {
                case AIType.CV:
                    systemstart = new ChatMessage(ChatRole.System, "You are a pretend CV database");
                    break;
                case AIType.JobDesc:
                    systemstart = new ChatMessage(ChatRole.System, "You are an agent that generates job description");
                    break;
                case AIType.WorkItem:
                    systemstart = new ChatMessage(ChatRole.System, "You are an agent that generates DevOps workitem examples");
                    break;
                case AIType.General:
                    systemstart = new ChatMessage(ChatRole.System, "You are a general AI assistant");
                    break;
                default:
                    systemstart = new ChatMessage(ChatRole.System, "You are a general AI assistant");
                    break;
            }

            ChatContext = new ChatCompletionsOptions()
            {
                Messages =  {
                    systemstart,
                    new ChatMessage(ChatRole.Assistant, initText)
                    },
                Temperature = (float)temp,
                MaxTokens = 2000,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                NucleusSamplingFactor = (float)topP
            };
            Site = siteType;

        }

        public async Task<List<ChatMessage>> MakeRequest(string message)
        {
            ChatContext.Messages.Add(new ChatMessage(ChatRole.User, message));

            // ### If streaming is not selected
            Response<ChatCompletions> responseWithoutStream = await client.GetChatCompletionsAsync("GPT-Test1", ChatContext);

            ChatCompletions completions = responseWithoutStream.Value;

            ChatContext.Messages.Add(completions.Choices.First().Message);

            return ChatContext.Messages.ToList();
        }

        private List<string> ToStringList(IList<ChatMessage> chatlist)
        {
            List<string> result = new List<string>();

            foreach (ChatMessage chat in chatlist)
            {
                result.Add(chat.Content);
            }

            return result;
        }

        public List<ChatMessage> CleanSystemMessages(List<ChatMessage> messages )
        {
            List<ChatMessage> clean = new List<ChatMessage>();

            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].Role != ChatRole.System)
                {
                    clean.Add(messages[i]);
                }
            }

            return clean;
        }

        public enum AIType
        {
            CV,
            JobDesc,
            WorkItem,
            General
        }
    }
}

