# Setup script for Two LLMs Discussion App

Write-Host "🔐 Setting up Azure OpenAI for Two LLMs Discussion App" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Green
Write-Host ""

Write-Host "This app requires Azure OpenAI service configuration." -ForegroundColor Yellow
Write-Host "You can get these from your Azure OpenAI resource in the Azure Portal:" -ForegroundColor Cyan
Write-Host "- Go to Azure Portal -> Your OpenAI Resource -> Keys and Endpoint" -ForegroundColor Cyan
Write-Host ""

$apiKey = Read-Host "Enter your Azure OpenAI API Key"
$endpoint = Read-Host "Enter your Azure OpenAI Endpoint (e.g., https://your-resource.openai.azure.com/)"
$deploymentName = Read-Host "Enter your deployment name (e.g., gpt-4, gpt-35-turbo)"

if ([string]::IsNullOrWhiteSpace($apiKey)) {
    Write-Host "No API key provided. Exiting..." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($endpoint)) {
    Write-Host "No endpoint provided. Exiting..." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($deploymentName)) {
    Write-Host "No deployment name provided. Exiting..." -ForegroundColor Red
    exit 1
}

# Set the configuration in user secrets
dotnet user-secrets set "AzureOpenAI:ApiKey" $apiKey
dotnet user-secrets set "AzureOpenAI:Endpoint" $endpoint
dotnet user-secrets set "AzureOpenAI:DeploymentName" $deploymentName

Write-Host ""
Write-Host "✅ Azure OpenAI configuration has been securely stored!" -ForegroundColor Green
Write-Host ""
Write-Host "Configuration set:" -ForegroundColor Cyan
Write-Host "- Endpoint: $endpoint" -ForegroundColor Gray
Write-Host "- Deployment: $deploymentName" -ForegroundColor Gray
Write-Host "- API Key: [HIDDEN]" -ForegroundColor Gray
Write-Host ""
Write-Host "You can now run the application with: dotnet run" -ForegroundColor Cyan
Write-Host ""
