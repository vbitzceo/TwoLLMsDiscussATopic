using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace TwoLLMsDiscussion;

class Program
{
    private static IConfiguration? _configuration;
    
    static async Task Main(string[] args)
    {
        // Load configuration
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        Console.WriteLine("🤖 Two LLMs Discussion App");
        Console.WriteLine("==========================");
        Console.WriteLine();

        // Get topic from user
        Console.Write("Enter a topic for the LLMs to discuss: ");
        string? topic = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(topic))
        {
            Console.WriteLine("No topic provided. Exiting...");
            return;
        }

        try
        {
            // Create two different kernels with different personalities
            var alice = CreateKernel("Alice", "You are Alice, an optimistic and enthusiastic AI who loves exploring new ideas. You tend to see the positive side of things and ask thoughtful questions.");
            var bob = CreateKernel("Bob", "You are Bob, a pragmatic and analytical AI who likes to examine things critically. You tend to focus on practical implications and potential challenges.");

            await StartDiscussion(alice, bob, topic);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }    private static Kernel CreateKernel(string name, string personality)
    {
        var builder = Kernel.CreateBuilder();
        
        // Add Azure OpenAI chat completion service
        var apiKey = _configuration?["AzureOpenAI:ApiKey"];
        var endpoint = _configuration?["AzureOpenAI:Endpoint"];
        var deploymentName = _configuration?["AzureOpenAI:DeploymentName"] ?? "gpt-4";
        
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Azure OpenAI API key not found in configuration.");
        }
        
        if (string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Azure OpenAI endpoint not found in configuration.");
        }

        builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey);
        
        var kernel = builder.Build();
        
        // Set the personality as a system message
        kernel.Data["Personality"] = personality;
        kernel.Data["Name"] = name;
        
        return kernel;
    }

    private static async Task StartDiscussion(Kernel alice, Kernel bob, string topic)
    {
        Console.WriteLine($"\n🎭 Starting discussion about: {topic}");
        Console.WriteLine("=" + new string('=', topic.Length + 25));
        Console.WriteLine();

        var aliceChatService = alice.GetRequiredService<IChatCompletionService>();
        var bobChatService = bob.GetRequiredService<IChatCompletionService>();

        // Initialize conversation histories
        var aliceHistory = new ChatHistory();
        var bobHistory = new ChatHistory();

        // Set personalities
        aliceHistory.AddSystemMessage(alice.Data["Personality"]?.ToString() ?? "");
        bobHistory.AddSystemMessage(bob.Data["Personality"]?.ToString() ?? "");

        // Start the discussion
        string currentMessage = $"Let's discuss the topic: {topic}. What are your initial thoughts?";
        
        for (int turn = 0; turn < 6; turn++) // 6 turns total (3 each)
        {
            if (turn % 2 == 0) // Alice's turn
            {
                Console.WriteLine($"💭 Alice:");
                
                aliceHistory.AddUserMessage(currentMessage);
                var aliceResponse = await aliceChatService.GetChatMessageContentAsync(aliceHistory);
                aliceHistory.AddAssistantMessage(aliceResponse.Content ?? "");
                
                Console.WriteLine($"   {aliceResponse.Content}");
                Console.WriteLine();
                
                // Bob will respond to Alice's message
                currentMessage = $"Alice said: {aliceResponse.Content}\n\nWhat's your response to this?";
            }
            else // Bob's turn
            {
                Console.WriteLine($"🤔 Bob:");
                
                bobHistory.AddUserMessage(currentMessage);
                var bobResponse = await bobChatService.GetChatMessageContentAsync(bobHistory);
                bobHistory.AddAssistantMessage(bobResponse.Content ?? "");
                
                Console.WriteLine($"   {bobResponse.Content}");
                Console.WriteLine();
                
                // Alice will respond to Bob's message
                currentMessage = $"Bob said: {bobResponse.Content}\n\nWhat's your response to this?";
            }

            // Add a small delay to make it feel more natural
            await Task.Delay(1000);
        }

        Console.WriteLine("🎯 Discussion completed!");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
