# Selenium Login Automation

Este proyecto automatiza el proceso de login en una página web usando Selenium WebDriver con C# y genera reportes HTML detallados con capturas de pantalla.

## 🆕 Nuevas Características

- **📄 Reportes HTML profesionales** con diseño responsivo
- **📸 Capturas de pantalla automáticas** en cada paso
- **📊 Métricas de rendimiento** y estadísticas
- **🎯 Detección automática de errores** 
- **⏱️ Timeline detallado** de ejecución

## Requisitos

- .NET 6.0 o superior
- Google Chrome instalado
- La página web debe estar ejecutándose en `http://127.0.0.1:5500/index.html`

## Configuración

1. **Instalar .NET 6.0**: Si no lo tienes instalado, descárgalo desde [Microsoft .NET](https://dotnet.microsoft.com/download)

2. **Restaurar paquetes NuGet**: El proyecto incluye las siguientes dependencias:
   - Selenium.WebDriver
   - Selenium.WebDriver.ChromeDriver
   - Selenium.Support

3. **Asegurarse de que Chrome esté instalado**: El script usa ChromeDriver, por lo que necesitas tener Google Chrome instalado.

## Credenciales de Login

El script está configurado para usar las siguientes credenciales:
- **Email**: davila@gmail.com
- **Password**: 12345
- **URL**: http://localhost:5173/

## Estructura del formulario detectado

El automatizador está optimizado para el siguiente formulario HTML:
- Campo de email: `input[name='email']` con ID `email`
- Campo de contraseña: `input[name='password']` con ID `password`
- Botón de submit: `button[type='submit']` con clase `login-button`
- Mensajes de error: `.error-message`
- Estados de carga: Detecta cuando el botón muestra "Iniciando sesión..."

## Cómo ejecutar

### Desde la línea de comandos:

1. Abre PowerShell o Command Prompt
2. Navega al directorio del proyecto:
   ```
   cd "c:\Users\Emilio\Desktop\selenium"
   ```
3. Restaura los paquetes:
   ```
   dotnet restore
   ```
4. Ejecuta el proyecto:
   ```
   dotnet run
   ```

### Desde Visual Studio:

1. Abre Visual Studio
2. Abre el archivo `SeleniumLoginAutomation.csproj`
3. Presiona F5 o haz clic en "Start" para ejecutar

## Funcionalidades

El script realiza las siguientes acciones:

1. **📊 Inicia el sistema de reportes** y crea las carpetas necesarias
2. **🚀 Inicializa el navegador Chrome** con configuraciones optimizadas
3. **🌐 Navega a la URL especificada** (`http://localhost:5173/`)
4. **🔍 Busca automáticamente los campos de login** usando múltiples selectores CSS
5. **✍️ Completa el formulario** con las credenciales especificadas
6. **🖱️ Envía el formulario** haciendo clic en el botón de submit o presionando Enter
7. **⏳ Maneja estados de carga** esperando que desaparezca "Iniciando sesión..."
8. **🚨 Detecta errores automáticamente** buscando mensajes de error en la página
9. **✅ Verifica el éxito del login** buscando indicadores comunes
10. **📸 Toma capturas de pantalla** en cada paso importante
11. **📄 Genera reporte HTML** con timeline completo y métricas
12. **🔒 Cierra el navegador** al finalizar

## 📁 Archivos Generados

Después de cada ejecución se crean:

```
TestReports/
├── test_report_YYYYMMDD_HHMMSS.html    # Reporte principal
└── Screenshots/                         # Capturas de pantalla
    ├── step_01_Configurando_opciones_YYYYMMDD_HHMMSS.png
    ├── step_02_Navegando_a_URL_YYYYMMDD_HHMMSS.png
    └── ... (una por cada paso)
```

## 🎨 Características del Reporte

- **Diseño responsivo** que funciona en móviles y desktop
- **Timeline visual** con iconos de éxito/error
- **Capturas clickeables** que se abren en modal
- **Métricas de tiempo** para cada paso
- **Resumen ejecutivo** con estadísticas
- **Detección automática** de errores y warnings

## Características destacadas

- **Búsqueda inteligente de elementos**: Usa múltiples selectores para encontrar campos de login
- **Manejo robusto de errores**: Incluye try-catch para manejar excepciones
- **Verificación de login**: Detecta automáticamente si el login fue exitoso
- **Configuración optimizada**: Chrome se configura con opciones para mejor compatibilidad
- **Timeouts configurables**: Esperas implícitas y explícitas para elementos

## Solución de problemas

### Error: "ChromeDriver not found"
- Asegúrate de que el paquete `Selenium.WebDriver.ChromeDriver` esté instalado
- Verifica que Chrome esté instalado en tu sistema

### Error: "Cannot connect to localhost"
- Asegúrate de que el servidor local esté ejecutándose en `http://127.0.0.1:5500/index.html`
- Verifica que la página esté accesible desde tu navegador

### Error: "Element not found"
- El script buscará automáticamente elementos comunes de login
- Si tu página usa selectores diferentes, puedes modificar los arrays de selectores en el código

## Personalización

Para adaptar el script a una página diferente:

1. Cambia la URL en el método `PerformLogin()`
2. Modifica las credenciales si es necesario
3. Ajusta los selectores en los arrays `selectors` si tu página usa elementos diferentes

## Estructura del proyecto

```
SeleniumLoginAutomation/
├── Program.cs                          # Código principal con lógica de automatización
├── Config.cs                           # Configuración y selectores CSS/XPath  
├── TestReport.cs                       # Sistema de reportes y capturas
├── SeleniumLoginAutomation.csproj      # Configuración del proyecto
├── README.md                           # Documentación principal
├── NUEVAS_CARACTERISTICAS.md           # Guía de nuevas funciones
├── run.bat                             # Script para Windows
├── run.ps1                             # Script PowerShell
└── TestReports/                        # Reportes generados (se crea automáticamente)
    ├── test_report_*.html              # Reportes HTML
    └── Screenshots/                    # Capturas de pantalla
        └── step_*.png
```

## ⚙️ Configuración Avanzada

En `Config.cs` puedes personalizar:

```csharp
// Reportes y capturas
public const bool TAKE_SCREENSHOTS = true;         // Activar capturas
public const bool GENERATE_HTML_REPORT = true;     // Generar reporte HTML
public const string REPORT_FOLDER = "TestReports"; // Carpeta de reportes

// Credenciales y URL
public const string LOGIN_URL = "http://localhost:5173/";
public const string USERNAME = "davila@gmail.com";
public const string PASSWORD = "12345";

// Timeouts
public const int IMPLICIT_WAIT_TIMEOUT = 10;       // Espera implícita
public const int EXPLICIT_WAIT_TIMEOUT = 10;       # Espera explícita
```
