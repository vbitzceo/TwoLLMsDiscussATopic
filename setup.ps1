# Setup script for Two LLMs Discussion App

Write-Host "🔐 Setting up Azure Services for Two LLMs Discussion App" -ForegroundColor Green
Write-Host "========================================================" -ForegroundColor Green
Write-Host ""

Write-Host "This app uses:" -ForegroundColor Yellow
Write-Host "• TWO different Azure OpenAI models for Alice and Bob" -ForegroundColor Cyan
Write-Host "• Azure Speech Service for voice output (optional)" -ForegroundColor Cyan
Write-Host ""

# Azure OpenAI Configuration
Write-Host "🤖 Azure OpenAI Configuration" -ForegroundColor Green
Write-Host "-----------------------------" -ForegroundColor Green
$apiKey = Read-Host "Enter your Azure OpenAI API Key"
$endpoint = Read-Host "Enter your Azure OpenAI Endpoint (e.g., https://your-resource.openai.azure.com/)"

Write-Host ""
Write-Host "Now configure the models for each participant:" -ForegroundColor Cyan
$aliceModel = Read-Host "Enter deployment name for Alice (e.g., gpt-4o, gpt-4)"
$bobModel = Read-Host "Enter deployment name for Bob (e.g., phi-3, gpt-35-turbo)"

# Azure Speech Service Configuration
Write-Host ""
Write-Host "🔊 Azure Speech Service Configuration (Optional)" -ForegroundColor Green
Write-Host "-----------------------------------------------" -ForegroundColor Green
$configureSpeech = Read-Host "Configure Azure Speech Service for voice output? (y/N)"

$speechApiKey = ""
$speechRegion = ""
$aliceVoice = "en-US-JennyNeural"
$bobVoice = "en-US-BrianNeural"

if ($configureSpeech -eq "y" -or $configureSpeech -eq "Y") {
    $speechApiKey = Read-Host "Enter your Azure Speech Service API Key"
    $speechRegion = Read-Host "Enter your Azure Speech Service Region (e.g., eastus, westus2)"
    
    Write-Host ""
    Write-Host "Voice configuration (press Enter for defaults):" -ForegroundColor Cyan
    $aliceVoiceInput = Read-Host "Alice's voice [$aliceVoice]"
    if (![string]::IsNullOrWhiteSpace($aliceVoiceInput)) {
        $aliceVoice = $aliceVoiceInput
    }
    
    $bobVoiceInput = Read-Host "Bob's voice [$bobVoice]"
    if (![string]::IsNullOrWhiteSpace($bobVoiceInput)) {
        $bobVoice = $bobVoiceInput
    }
}

# Validation
if ([string]::IsNullOrWhiteSpace($apiKey)) {
    Write-Host "No OpenAI API key provided. Exiting..." -ForegroundColor Red
    exit 1
}

if ([string]::IsNullOrWhiteSpace($endpoint)) {
    Write-Host "No OpenAI endpoint provided. Exiting..." -ForegroundColor Red
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

# Set speech configuration if provided
if (![string]::IsNullOrWhiteSpace($speechApiKey)) {
    dotnet user-secrets set "AzureSpeech:ApiKey" $speechApiKey
    dotnet user-secrets set "AzureSpeech:Region" $speechRegion
    dotnet user-secrets set "AzureSpeech:Alice:VoiceName" $aliceVoice
    dotnet user-secrets set "AzureSpeech:Bob:VoiceName" $bobVoice
}

Write-Host ""
Write-Host "✅ Configuration has been securely stored!" -ForegroundColor Green
Write-Host ""
Write-Host "Azure OpenAI Configuration:" -ForegroundColor Cyan
Write-Host "- Endpoint: $endpoint" -ForegroundColor Gray
Write-Host "- Alice Model: $aliceModel" -ForegroundColor Gray
Write-Host "- Bob Model: $bobModel" -ForegroundColor Gray
Write-Host "- API Key: [HIDDEN]" -ForegroundColor Gray

if (![string]::IsNullOrWhiteSpace($speechApiKey)) {
    Write-Host ""
    Write-Host "Azure Speech Configuration:" -ForegroundColor Cyan
    Write-Host "- Region: $speechRegion" -ForegroundColor Gray
    Write-Host "- Alice Voice: $aliceVoice" -ForegroundColor Gray
    Write-Host "- Bob Voice: $bobVoice" -ForegroundColor Gray
    Write-Host "- API Key: [HIDDEN]" -ForegroundColor Gray
}

Write-Host ""
Write-Host "You can now run the application with: dotnet run" -ForegroundColor Cyan
Write-Host ""
Write-Host "💡 Tips:" -ForegroundColor Yellow
Write-Host "   • Try different model combinations for varied discussions!" -ForegroundColor Gray
Write-Host "     Examples: GPT-4o vs Phi-3, GPT-4 vs GPT-3.5-turbo" -ForegroundColor Gray
if (![string]::IsNullOrWhiteSpace($speechApiKey)) {
    Write-Host "   • Enable voice output when prompted for an immersive experience!" -ForegroundColor Gray
}
Write-Host ""
