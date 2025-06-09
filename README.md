# Two LLMs Discussion App

A C# console application that uses Semantic Kernel to create a discussion between two AI personalities on any given topic using Azure OpenAI.

## Features

- Prompts user for a discussion topic
- Creates two AI personalities using **different models**:
  - Alice (optimistic) - uses one Azure OpenAI model
  - Bob (pragmatic) - uses a different Azure OpenAI model
- Facilitates a 6-turn conversation between the AIs
- Shows which model each participant is using
- Uses Azure OpenAI through Semantic Kernel

## Prerequisites

- .NET 8.0 or later
- Azure OpenAI resource with **TWO deployed models**
  - One for Alice (e.g., GPT-4o, GPT-4)
  - One for Bob (e.g., Phi-3, GPT-3.5-turbo)
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
   
   **Option C: Edit appsettings.json (Not recommended for production):**
   ```json
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
    },
    "Bob": {
      "DeploymentName": "phi-3"
    }
  }
}
```

## Model Combinations

Try different model combinations for varied discussions:
- **GPT-4o vs Phi-3** - Advanced vs efficient reasoning
- **GPT-4 vs GPT-3.5-turbo** - Detailed vs quick responses  
- **GPT-4o vs GPT-4** - Latest vs established capabilities
- **Any combination** - Each brings unique characteristics!

## Getting Azure OpenAI Access

1. **Create an Azure OpenAI resource** in the Azure Portal
2. **Deploy TWO models** in Azure OpenAI Studio:
   - Deploy one model for Alice (e.g., "gpt-4o" deployment)
   - Deploy another model for Bob (e.g., "phi-3" deployment)
3. **Get your configuration**:
   - API Key: Found in "Keys and Endpoint" section
   - Endpoint: Found in "Keys and Endpoint" section  
   - Deployment Names: The names you gave each deployed model

## Usage

1. Run the application
2. Enter a topic when prompted
3. Watch Alice and Bob discuss the topic!

## Example Topics

- "The future of artificial intelligence"
- "Should we colonize Mars?"
- "The best programming language"
- "Climate change solutions"
