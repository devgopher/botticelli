using System;
using System.Text.Json;
using System.Threading.Tasks;
using Botticelli.AI.ChatGpt.Provider;
using Botticelli.AI.ChatGpt.Settings;
using Botticelli.AI.DeepSeekGpt.Message.DeepSeek;
using Botticelli.AI.DeepSeekGpt.Provider;
using NUnit.Framework;
using Shared;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Usage = Botticelli.AI.DeepSeekGpt.Message.DeepSeek.Usage;

namespace Botticelli.AI.Test.AIProvider;

[TestFixture]
[TestOf(typeof(DeepSeekGptProvider))]
public class DeepSeekGptProviderTest : BaseAiProviderTest
{
    [SetUp]
    public void StartMockServer()
    {
        Setup();
        var responseMessage = new DeepSeekOutputMessage
        {
            Id = Guid.NewGuid().ToString(),
            Object = "test",
            Created = 111111,
            Model = "testapi",
           Usage = new Usage
           {
               CompletionTokens = 1000,
               PromptTokens = 1000,
               TotalTokens = 2000
           },
            Choices =
            [
                new()
                {
                    DeepSeekMessage = new DeepSeekInnerOutputMessage
                    {
                        Content = ResponseString,
                        Role = "none",
                        FunctionCall = null,
                        ToolCalls = null
                    },
                    FinishReason = null,
                    Index = 0,
                    Logprobs = null
                }
            ]
        };

        Server.Given(Request.Create().WithPath("/completions").UsingPost())
              .RespondWith(
                           Response.Create()
                                   .WithStatusCode(200)
                                   .WithBody(JsonSerializer.Serialize(responseMessage))
                          );

        AiProvider = new ChatGptProvider(new OptionsMock<GptSettings>(GptSettings),
                                         ClientFactory,
                                         LoggerMocks.CreateConsoleLogger<ChatGptProvider>(),
                                         BusClient,
                                         Validator);
    }

    [Test]
    [TestCase("test query")]
    public async Task SendAsyncTest(string query) => await InnerSendAsyncTest(query);

    [TearDown]
    public void TearDown() => Server.Stop();
}