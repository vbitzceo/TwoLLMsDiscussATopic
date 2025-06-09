# Azure OpenAI Configuration Example

This file shows you what information you need to gather from your Azure OpenAI resource.

## Steps to get your configuration:

### 1. Create Azure OpenAI Resource
- Go to [Azure Portal](https://portal.azure.com)
- Create a new "Azure OpenAI" resource
- Choose your subscription, resource group, and region
- Complete the resource creation

### 2. Deploy a Model
- Go to [Azure OpenAI Studio](https://oai.azure.com)
- Navigate to "Deployments" 
- Create a new deployment
- Choose a model (e.g., gpt-4, gpt-35-turbo)
- Give it a deployment name (e.g., "gpt-4-chat")
- Note down this deployment name

### 3. Get Your Configuration Values
From your Azure OpenAI resource in the Azure Portal:
- Go to "Keys and Endpoint" section
- Copy "KEY 1" (this is your API Key)
- Copy "Endpoint" (this is your endpoint URL)

### 4. Example Configuration

```json
{
  "AzureOpenAI": {
    "ApiKey": "abc123def456...",
    "Endpoint": "https://my-openai-resource.openai.azure.com/",
    "DeploymentName": "gpt-4-chat"
  }
}
```

### 5. Use the Setup Script
Run `.\setup.ps1` and enter these values when prompted.

## Common Deployment Names
- `gpt-4` - For GPT-4 models
- `gpt-35-turbo` - For GPT-3.5 Turbo models  
- `gpt-4-turbo` - For GPT-4 Turbo models

The deployment name is what YOU chose when deploying the model, not the model name itself.
