# Azure Services Configuration for Two LLMs Discussion

This app uses TWO different Azure OpenAI models to create more interesting discussions between Alice and Bob, plus optional Azure Speech Service for voice output.

## Required: Azure OpenAI Setup

### 1. Create Azure OpenAI Resource
- Go to [Azure Portal](https://portal.azure.com)
- Create a new "Azure OpenAI" resource
- Choose your subscription, resource group, and region
- Complete the resource creation

### 2. Deploy TWO Models
- Go to [Azure OpenAI Studio](https://oai.azure.com)
- Navigate to "Deployments" 
- Create your **first deployment** for Alice:
  - Choose a model (e.g., gpt-4o, gpt-4)
  - Give it a deployment name (e.g., "gpt-4o-alice")
- Create your **second deployment** for Bob:
  - Choose a different model (e.g., phi-3, gpt-35-turbo)
  - Give it a deployment name (e.g., "phi-3-bob")
- Note down both deployment names

### 3. Get Your Configuration Values
From your Azure OpenAI resource in the Azure Portal:
- Go to "Keys and Endpoint" section
- Copy "KEY 1" (this is your API Key)
- Copy "Endpoint" (this is your endpoint URL)

## Optional: Azure Speech Service Setup

### 1. Create Azure Speech Service Resource
- Go to [Azure Portal](https://portal.azure.com)
- Create a new "Speech Service" resource
- Choose your subscription, resource group, and region
- Complete the resource creation

### 2. Get Speech Service Configuration
From your Speech Service resource in the Azure Portal:
- Go to "Keys and Endpoint" section
- Copy "KEY 1" (this is your Speech API Key)
- Note the "Region" where you deployed the service

### 3. Example Configuration

```json
{
  "AzureOpenAI": {
    "ApiKey": "abc123def456...",
    "Endpoint": "https://my-openai-resource.openai.azure.com/",
    "Alice": {
      "DeploymentName": "gpt-4o-alice"
    },
    "Bob": {
      "DeploymentName": "phi-3-bob"
    }
  },
  "AzureSpeech": {
    "ApiKey": "xyz789ghi012...",
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

### 4. Use the Setup Script
Run `.\setup.ps1` and enter these values when prompted. The script will configure both Azure OpenAI and Azure Speech Service (speech is optional).

## Popular Neural Voices

### English (US):
- `en-US-JennyNeural` (Female, warm)
- `en-US-BrianNeural` (Male, friendly)
- `en-US-AriaNeural` (Female, cheerful)
- `en-US-DavisNeural` (Male, confident)

### Other Languages:
- **UK English**: `en-GB-SoniaNeural`, `en-GB-RyanNeural`
- **Spanish**: `es-ES-ElviraNeural`, `es-ES-AlvaroNeural`
- **French**: `fr-FR-DeniseNeural`, `fr-FR-HenriNeural`
- **German**: `de-DE-KatjaNeural`, `de-DE-ConradNeural`

## Recommended Model Combinations

### For Variety:
- **Alice: GPT-4o** + **Bob: Phi-3** (Advanced vs Efficient)
- **Alice: GPT-4** + **Bob: GPT-3.5-turbo** (Detailed vs Quick)

### For Performance Testing:
- **Alice: GPT-4** + **Bob: GPT-4o** (Compare generations)
- **Alice: Phi-3** + **Bob: GPT-3.5-turbo** (Compare efficiency)

### For Cost Optimization:
- **Alice: GPT-3.5-turbo** + **Bob: Phi-3** (Both efficient models)

## Important Notes
- Each model brings different capabilities and response styles
- The deployment names are what YOU choose, not the model names
- Both deployments use the same API key and endpoint
- Different models create more interesting and varied discussions!
