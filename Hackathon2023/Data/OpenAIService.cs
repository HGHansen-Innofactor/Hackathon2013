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
            NucleusSamplingFactor = (float)0.95,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
        };

        public List<ChatMessage> ChatMessages { get { return ChatContext.Messages.ToList(); } }
        private string Model { get; set; } = "GPT-Test1";
        private AIType Site { get; set; } = OpenAIService.AIType.CV;

        public void Init(double temp, double topP, string model, string initText, AIType siteType)
        {
            Model = model;
            ChatContext = new ChatCompletionsOptions()
            {
                Messages =
                    {
                new ChatMessage(ChatRole.Assistant, initText)
                    },
                Temperature = (float)temp,
                MaxTokens = 2000,
                NucleusSamplingFactor = (float)topP,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };
            Site = siteType;

            switch (siteType)
            {
                case AIType.CV:
                    var dumpcv = MakeRequest("Can you pretend to be a CV database?");
                    break;
                case AIType.JobDesc:
                    var dumpjd = MakeRequest("Get ready to generate a job description");
                    break;
                case AIType.WorkItem:
                    var dumpwi = MakeRequest("Are you ready to generate DevOps work item examples!");
                    break;
                case AIType.General:
                    var dumpgr = MakeRequest("Can you be a general AI assistant");
                    break;
                default:
                    break;
            }

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

        public enum AIType
        {
            CV,
            JobDesc,
            WorkItem,
            General
        }
    }
}

