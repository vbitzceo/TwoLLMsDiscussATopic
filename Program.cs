using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace TwoLLMsDiscussion;

class Program
{
    private static IConfiguration? _configuration;
    private static SpeechSynthesizer? _aliceSynthesizer;
    private static SpeechSynthesizer? _bobSynthesizer;
    
    static async Task Main(string[] args)
    {
        // Load configuration
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        Console.WriteLine("🤖 Two LLMs Discussion App");
        Console.WriteLine("==========================");
        Console.WriteLine();        // Get topic from user
        Console.Write("Enter a topic for the LLMs to discuss: ");
        string? topic = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(topic))
        {
            Console.WriteLine("No topic provided. Exiting...");
            return;
        }

        // Ask about speech output
        Console.Write("Enable voice output? (y/N): ");
        string? enableSpeech = Console.ReadLine();
        bool speechEnabled = enableSpeech?.ToLower().StartsWith("y") == true;        try
        {            // Initialize speech synthesizers if enabled
            if (speechEnabled)
            {
                await InitializeSpeechSynthesizers();
            }
            
            // Create two different kernels with different models and personalities
            var alice = CreateKernel("Alice", 
                "You are Alice, an optimistic and enthusiastic AI who loves exploring new ideas. You tend to see the positive side of things and ask thoughtful questions.",
                "Alice");
            var bob = CreateKernel("Bob", 
                "You are Bob, a pragmatic and analytical AI who likes to examine things critically. You tend to focus on practical implications and potential challenges.",
                "Bob");

            await StartDiscussion(alice, bob, topic, speechEnabled);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");        }
        finally
        {
            // Clean up speech synthesizers
            _aliceSynthesizer?.Dispose();
            _bobSynthesizer?.Dispose();
        }
    }    private static Task InitializeSpeechSynthesizers()
    {
        var speechApiKey = _configuration?["AzureSpeech:ApiKey"];
        var speechRegion = _configuration?["AzureSpeech:Region"];
        var aliceVoice = _configuration?["AzureSpeech:Alice:VoiceName"];
        var bobVoice = _configuration?["AzureSpeech:Bob:VoiceName"];

        if (string.IsNullOrEmpty(speechApiKey) || string.IsNullOrEmpty(speechRegion))
        {
            Console.WriteLine("⚠️  Azure Speech Service not configured. Continuing without voice output...");
            return Task.CompletedTask;
        }

        try
        {
            var speechConfig = SpeechConfig.FromSubscription(speechApiKey, speechRegion);
            
            // Initialize Alice's synthesizer
            speechConfig.SpeechSynthesisVoiceName = aliceVoice ?? "en-US-JennyNeural";
            _aliceSynthesizer = new SpeechSynthesizer(speechConfig);
            
            // Initialize Bob's synthesizer  
            speechConfig.SpeechSynthesisVoiceName = bobVoice ?? "en-US-BrianNeural";
            _bobSynthesizer = new SpeechSynthesizer(speechConfig);
            
            Console.WriteLine("🔊 Speech synthesis initialized successfully!");
            Console.WriteLine($"   💭 Alice: {aliceVoice}");
            Console.WriteLine($"   🤔 Bob: {bobVoice}");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  Failed to initialize speech synthesis: {ex.Message}");
            Console.WriteLine("Continuing without voice output...");
            Console.WriteLine();
        }

        return Task.CompletedTask;
    }

    private static async Task SpeakTextAsync(string text, SpeechSynthesizer? synthesizer, string speaker)
    {
        if (synthesizer == null || string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        try
        {
            Console.Write($"🔊 {speaker} is speaking... ");
            var result = await synthesizer.SpeakTextAsync(text);
            
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine("✅");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Console.WriteLine($"❌ Speech synthesis canceled: {cancellation.Reason}");
                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Speech synthesis error: {ex.Message}");
        }
    }

    private static Kernel CreateKernel(string name, string personality, string configKey)
    {
        var builder = Kernel.CreateBuilder();
        
        // Add Azure OpenAI chat completion service
        var apiKey = _configuration?["AzureOpenAI:ApiKey"];
        var endpoint = _configuration?["AzureOpenAI:Endpoint"];
        var deploymentName = _configuration?[$"AzureOpenAI:{configKey}:DeploymentName"];
        
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new InvalidOperationException("Azure OpenAI API key not found in configuration.");
        }
        
        if (string.IsNullOrEmpty(endpoint))
        {
            throw new InvalidOperationException("Azure OpenAI endpoint not found in configuration.");
        }
        
        if (string.IsNullOrEmpty(deploymentName))
        {
            throw new InvalidOperationException($"Azure OpenAI deployment name for {name} not found in configuration.");
        }

        builder.AddAzureOpenAIChatCompletion(deploymentName, endpoint, apiKey);
        
        var kernel = builder.Build();
        
        // Set the personality and model info
        kernel.Data["Personality"] = personality;
        kernel.Data["Name"] = name;
        kernel.Data["Model"] = deploymentName;
        
        return kernel;
    }    private static async Task StartDiscussion(Kernel alice, Kernel bob, string topic, bool speechEnabled)
    {
        Console.WriteLine($"\n🎭 Starting discussion about: {topic}");
        Console.WriteLine("=" + new string('=', topic.Length + 25));
        Console.WriteLine();
        
        // Display model information
        Console.WriteLine("🤖 Participants:");
        Console.WriteLine($"   💭 Alice (using {alice.Data["Model"]}) - Optimistic & Enthusiastic");
        Console.WriteLine($"   🤔 Bob (using {bob.Data["Model"]}) - Pragmatic & Analytical");
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
        {            if (turn % 2 == 0) // Alice's turn
            {
                Console.WriteLine($"💭 Alice ({alice.Data["Model"]}):");
                
                aliceHistory.AddUserMessage(currentMessage);
                var aliceResponse = await aliceChatService.GetChatMessageContentAsync(aliceHistory);
                aliceHistory.AddAssistantMessage(aliceResponse.Content ?? "");
                
                Console.WriteLine($"   {aliceResponse.Content}");
                Console.WriteLine();
                  // Speak Alice's response
                if (speechEnabled)
                {
                    await SpeakTextAsync(aliceResponse.Content ?? "", _aliceSynthesizer, "Alice");
                    Console.WriteLine();
                }
                
                // Bob will respond to Alice's message
                currentMessage = $"Alice said: {aliceResponse.Content}\n\nWhat's your response to this?";
            }
            else // Bob's turn
            {
                Console.WriteLine($"🤔 Bob ({bob.Data["Model"]}):");
                
                bobHistory.AddUserMessage(currentMessage);
                var bobResponse = await bobChatService.GetChatMessageContentAsync(bobHistory);
                bobHistory.AddAssistantMessage(bobResponse.Content ?? "");
                
                Console.WriteLine($"   {bobResponse.Content}");
                Console.WriteLine();
                  // Speak Bob's response
                if (speechEnabled)
                {
                    await SpeakTextAsync(bobResponse.Content ?? "", _bobSynthesizer, "Bob");
                    Console.WriteLine();
                }
                
                // Alice will respond to Bob's message
                currentMessage = $"Bob said: {bobResponse.Content}\n\nWhat's your response to this?";
            }

            // Add a small delay to make it feel more natural
            await Task.Delay(200);
        }

        Console.WriteLine("🎯 Discussion completed!");
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
