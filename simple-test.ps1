# Simple Build Test
Write-Host "Starting build test..." -ForegroundColor Green

# Clean previous builds
if (Test-Path "./release") {
    Remove-Item "./release" -Recurse -Force
}

# Restore dependencies
Write-Host "Restoring dependencies..." -ForegroundColor Blue
dotnet restore ReturnOrderGenerator.csproj

# Build project
Write-Host "Building project..." -ForegroundColor Blue
dotnet build ReturnOrderGenerator.csproj --configuration Release --no-restore

# Publish application
Write-Host "Publishing application..." -ForegroundColor Blue
dotnet publish ReturnOrderGenerator.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -o ./release

# Check results
if (Test-Path "./release/ReturnOrderGenerator.exe") {
    $exeSize = (Get-Item "./release/ReturnOrderGenerator.exe").Length / 1MB
    Write-Host "Success! Main executable created: $([math]::Round($exeSize, 2)) MB" -ForegroundColor Green
    
    # List release contents
    Write-Host "Release contents:" -ForegroundColor Blue
    Get-ChildItem ./release | Format-Table Name, Length
    
    Write-Host "Build test completed successfully!" -ForegroundColor Green
} else {
    Write-Host "Error: Main executable not found!" -ForegroundColor Red
}