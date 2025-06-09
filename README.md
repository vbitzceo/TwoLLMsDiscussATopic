# Two LLMs Discussion App

A C# console application that uses Semantic Kernel to create a discussion between two AI personalities on any given topic using Azure OpenAI, with optional Azure Text-to-Speech for voice output.

## Features

- Prompts user for a discussion topic
- Creates two AI personalities using **different models**:
  - Alice (optimistic) - uses one Azure OpenAI model
  - Bob (pragmatic) - uses a different Azure OpenAI model
- Facilitates a 6-turn conversation between the AIs
- Shows which model each participant is using
- **Voice Output**: Optional Azure Text-to-Speech integration
  - Alice speaks with one voice (default: Jenny Neural)
  - Bob speaks with another voice (default: Brian Neural)
- Uses Azure OpenAI through Semantic Kernel

## Prerequisites

- .NET 8.0 or later
- Azure OpenAI resource with **TWO deployed models**
  - One for Alice (e.g., GPT-4o, GPT-4)
  - One for Bob (e.g., Phi-3, GPT-3.5-turbo)
- **Optional**: Azure Speech Service for voice output
- Different models create more interesting discussions!

## Setup

1. **Install dependencies:**
   ```bash
   dotnet restore
   ```

2. **Configure Azure OpenAI (Choose one option):**
   
   **Option A: Using the setup script (Recommended):**
   ```powershell
   .\setup.ps1
   ```
     **Option B: Manual setup with User Secrets:**
   ```bash
   dotnet user-secrets set "AzureOpenAI:ApiKey" "your-azure-openai-api-key"
   dotnet user-secrets set "AzureOpenAI:Endpoint" "https://your-resource.openai.azure.com/"
   dotnet user-secrets set "AzureOpenAI:Alice:DeploymentName" "gpt-4o"
   dotnet user-secrets set "AzureOpenAI:Bob:DeploymentName" "phi-3"
   ```
   
   **Option C: Edit appsettings.json (Not recommended for production):**   ```json
   {
     "AzureOpenAI": {
       "ApiKey": "your-azure-openai-api-key",
       "Endpoint": "https://your-resource.openai.azure.com/",
       "Alice": {
         "DeploymentName": "gpt-4o"
       },
       "Bob": {
         "DeploymentName": "phi-3"
       }
     },
     "AzureSpeech": {
       "ApiKey": "your-azure-speech-api-key",
       "Region": "eastus",
       "Alice": {
         "VoiceName": "en-US-JennyNeural"
       },
       "Bob": {
         "VoiceName": "en-US-BrianNeural"
       }
     }
   }
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

## Configuration

The app supports configuration through `appsettings.json` or User Secrets:

```json
{
  "AzureOpenAI": {
    "ApiKey": "your-azure-openai-api-key",
    "Endpoint": "https://your-resource.openai.azure.com/",
    "Alice": {
      "DeploymentName": "gpt-4o"
    },    "Bob": {
      "DeploymentName": "phi-3"
    }
  },
  "AzureSpeech": {
    "ApiKey": "your-azure-speech-api-key",
    "Region": "eastus",
    "Alice": {
      "VoiceName": "en-US-JennyNeural"
    },
    "Bob": {
      "VoiceName": "en-US-BrianNeural"
    }
  }
}
```

### Available Voices

Popular Azure Neural voices for different languages:
- **English (US)**: `en-US-JennyNeural`, `en-US-BrianNeural`, `en-US-AriaNeural`
- **English (UK)**: `en-GB-SoniaNeural`, `en-GB-RyanNeural`
- **Spanish**: `es-ES-ElviraNeural`, `es-ES-AlvaroNeural`
- **French**: `fr-FR-DeniseNeural`, `fr-FR-HenriNeural`
- **German**: `de-DE-KatjaNeural`, `de-DE-ConradNeural`

## Model Combinations

Try different model combinations for varied discussions:
- **GPT-4o vs Phi-3** - Advanced vs efficient reasoning
- **GPT-4 vs GPT-3.5-turbo** - Detailed vs quick responses  
- **GPT-4o vs GPT-4** - Latest vs established capabilities
- **Any combination** - Each brings unique characteristics!

## Getting Azure Services Access

### Azure OpenAI

1. **Create an Azure OpenAI resource** in the Azure Portal
2. **Deploy TWO models** in Azure OpenAI Studio:
   - Deploy one model for Alice (e.g., "gpt-4o" deployment)
   - Deploy another model for Bob (e.g., "phi-3" deployment)
3. **Get your configuration**:
   - API Key: Found in "Keys and Endpoint" section
   - Endpoint: Found in "Keys and Endpoint" section  
   - Deployment Names: The names you gave each deployed model

### Azure Speech Service (Optional)

1. **Create a Speech Service resource** in the Azure Portal
2. **Get your configuration**:
   - API Key: Found in "Keys and Endpoint" section
   - Region: The region where you deployed the service

## Usage

1. Run the application: `dotnet run`
2. Enter a topic when prompted
3. Choose whether to enable voice output (requires Azure Speech Service)
4. Watch (and optionally listen to) Alice and Bob discuss the topic!

### Example Output

```
🤖 Two LLMs Discussion App
==========================

Enter a topic for the LLMs to discuss: climate change
Enable voice output? (y/N): y

🔊 Speech synthesis initialized successfully!
   💭 Alice: en-US-JennyNeural
   🤔 Bob: en-US-BrianNeural

🎭 Starting discussion about: climate change
===============================================

🤖 Participants:
   💭 Alice (using gpt-4o) - Optimistic & Enthusiastic
   🤔 Bob (using phi-3) - Pragmatic & Analytical

💭 Alice (gpt-4o):
   I'm really excited to discuss climate change! While it's a serious challenge, I see so many innovative solutions emerging...
🔊 Alice is speaking... ✅

🤔 Bob (phi-3):
   That's true, but we need to be realistic about the scale of the problem. The data shows we're running out of time...
🔊 Bob is speaking... ✅
```

## Example Topics

- "The future of artificial intelligence"
- "Should we colonize Mars?"
- "The best programming language"
- "Climate change solutions"
