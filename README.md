# Two LLMs Discussion App

A C# console application that uses Semantic Kernel to create a discussion between two AI personalities on any given topic using Azure OpenAI.

## Features

- Prompts user for a discussion topic
- Creates two AI personalities (Alice - optimistic, Bob - pragmatic)
- Facilitates a 6-turn conversation between the AIs
- Uses Azure OpenAI through Semantic Kernel

## Prerequisites

- .NET 8.0 or later
- Azure OpenAI resource with a deployed model (e.g., GPT-4, GPT-3.5-turbo)

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
   dotnet user-secrets set "AzureOpenAI:DeploymentName" "gpt-4"
   ```
   
   **Option C: Edit appsettings.json (Not recommended for production):**
   ```json
   {
     "AzureOpenAI": {
       "ApiKey": "your-azure-openai-api-key",
       "Endpoint": "https://your-resource.openai.azure.com/",
       "DeploymentName": "gpt-4"
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
    "DeploymentName": "gpt-4"
  }
}
```

## Getting Azure OpenAI Access

1. **Create an Azure OpenAI resource** in the Azure Portal
2. **Deploy a model** (e.g., GPT-4, GPT-3.5-turbo) in Azure OpenAI Studio
3. **Get your configuration**:
   - API Key: Found in "Keys and Endpoint" section
   - Endpoint: Found in "Keys and Endpoint" section  
   - Deployment Name: The name you gave your deployed model

## Usage

1. Run the application
2. Enter a topic when prompted
3. Watch Alice and Bob discuss the topic!

## Example Topics

- "The future of artificial intelligence"
- "Should we colonize Mars?"
- "The best programming language"
- "Climate change solutions"
