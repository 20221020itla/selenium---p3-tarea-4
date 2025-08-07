@echo off
echo Restaurando paquetes NuGet...
dotnet restore SeleniumLoginAutomation.csproj
if %errorlevel% neq 0 (
    echo Error al restaurar paquetes. Presione cualquier tecla para salir.
    pause
    exit /b 1
)

echo.
echo Ejecutando automatizador Selenium...
dotnet run --project SeleniumLoginAutomation.csproj
pause
