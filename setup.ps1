# Setup script for Two LLMs Discussion App

Write-Host "🔐 Setting up Azure OpenAI for Two LLMs Discussion App" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Green
Write-Host ""

Write-Host "This app uses TWO different Azure OpenAI models for Alice and Bob." -ForegroundColor Yellow
Write-Host "You need to deploy both models in your Azure OpenAI resource." -ForegroundColor Cyan
Write-Host ""

$apiKey = Read-Host "Enter your Azure OpenAI API Key"
$endpoint = Read-Host "Enter your Azure OpenAI Endpoint (e.g., https://your-resource.openai.azure.com/)"

Write-Host ""
Write-Host "Now configure the models for each participant:" -ForegroundColor Cyan
$aliceModel = Read-Host "Enter deployment name for Alice (e.g., gpt-4o, gpt-4)"
$bobModel = Read-Host "Enter deployment name for Bob (e.g., phi-3, gpt-35-turbo)"

if ([string]::IsNullOrWhiteSpace($apiKey)) {
    Write-Host "No API key provided. Exiting..." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($endpoint)) {
    Write-Host "No endpoint provided. Exiting..." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($aliceModel)) {
    Write-Host "No Alice model provided. Exiting..." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($bobModel)) {
    Write-Host "No Bob model provided. Exiting..." -ForegroundColor Red
    exit 1
}

# Set the configuration in user secrets
dotnet user-secrets set "AzureOpenAI:ApiKey" $apiKey
dotnet user-secrets set "AzureOpenAI:Endpoint" $endpoint
dotnet user-secrets set "AzureOpenAI:Alice:DeploymentName" $aliceModel
dotnet user-secrets set "AzureOpenAI:Bob:DeploymentName" $bobModel

Write-Host ""
Write-Host "✅ Azure OpenAI configuration has been securely stored!" -ForegroundColor Green
Write-Host ""
Write-Host "Configuration set:" -ForegroundColor Cyan
Write-Host "- Endpoint: $endpoint" -ForegroundColor Gray
Write-Host "- Alice Model: $aliceModel" -ForegroundColor Gray
Write-Host "- Bob Model: $bobModel" -ForegroundColor Gray
Write-Host "- API Key: [HIDDEN]" -ForegroundColor Gray
Write-Host ""
Write-Host "You can now run the application with: dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "💡 Tip: Try different model combinations for varied discussions!" -ForegroundColor Yellow
Write-Host "   Examples: GPT-4o vs Phi-3, GPT-4 vs GPT-3.5-turbo" -ForegroundColor Gray
Write-Host ""
