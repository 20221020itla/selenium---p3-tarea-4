# Script para ejecutar el automatizador Selenium
Write-Host "=== Automatizador Selenium Login ===" -ForegroundColor Green
Write-Host ""

try {
    Write-Host "Restaurando paquetes NuGet..." -ForegroundColor Yellow
    $restoreResult = dotnet restore SeleniumLoginAutomation.csproj
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error al restaurar paquetes NuGet" -ForegroundColor Red
        Read-Host "Presione Enter para salir"
        exit 1
    }
    
    Write-Host "Paquetes restaurados exitosamente" -ForegroundColor Green
    Write-Host ""
    
    Write-Host "Ejecutando automatizador..." -ForegroundColor Yellow
    dotnet run --project SeleniumLoginAutomation.csproj
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
finally {
    Write-Host ""
    Read-Host "Presione Enter para salir"
}
