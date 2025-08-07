using System;

namespace SeleniumLoginAutomation
{
    public static class Config
    {
        public const string LOGIN_URL = "http://localhost:5173/";
        
        public const string USERNAME = "davila@gmail.com";
        public const string PASSWORD = "12345";
        
        // Timeouts (en segundos)
        public const int IMPLICIT_WAIT_TIMEOUT = 10;
        public const int EXPLICIT_WAIT_TIMEOUT = 10;
        public const int VERIFICATION_DELAY = 2000; // milisegundos
        public const int FINAL_DELAY = 3000; // milisegundos
        
        // Configuración de reportes
        public const string REPORT_FOLDER = "TestReports";
        public const string SCREENSHOTS_FOLDER = "Screenshots";
        public const bool TAKE_SCREENSHOTS = true;
        public const bool GENERATE_HTML_REPORT = true;
        
        // Selectores CSS para campos de login (en orden de prioridad)
        public static readonly string[] USERNAME_SELECTORS = 
        {
            "input[name='email']",
            "input[id='email']",
            "#email",
            "input[type='email']",
            "input[placeholder='davila@gmail.com']",
            "input[placeholder*='davila']",
            "input[placeholder*='gmail']",
            ".login-form input[type='email']",
            ".form-group input[type='email']",
            "input[name='username']",
            "input[id='username']",
            "input[type='text']"
        };
        
        public static readonly string[] PASSWORD_SELECTORS = 
        {
            "input[name='password']",
            "input[id='password']",
            "#password",
            "input[type='password']",
            "input[placeholder='••••••••']",
            "input[placeholder*='••••']",
            ".login-form input[type='password']",
            ".form-group input[type='password']",
            ".password",
            "input[placeholder*='pass']",
            "input[placeholder*='Pass']",
            "input[placeholder*='contraseña']"
        };
        
        public static readonly string[] LOGIN_BUTTON_SELECTORS = 
        {
            "button[type='submit']",
            ".login-button",
            "button.login-button",
            ".login-form button[type='submit']",
            "form.login-form button",
            "button:contains('Iniciar Sesión')",
            "button:contains('Iniciando sesión')",
            "input[type='submit']",
            "button[id*='login']",
            "button[class*='login']",
            ".login-btn",
            "#login-btn",
            ".btn-login",
            "#btn-login"
        };
        
        // XPath selectors para verificar login exitoso
        public static readonly string[] SUCCESS_INDICATORS = 
        {
            "//div[contains(@class, 'success')]",
            "//div[contains(@class, 'dashboard')]",
            "//div[contains(@class, 'welcome')]",
            "//div[contains(@class, 'home')]",
            "//div[contains(@class, 'main')]",
            "//nav[contains(@class, 'navbar')]",
            "//header[contains(@class, 'header')]",
            "//div[contains(text(), 'Welcome')]",
            "//div[contains(text(), 'Bienvenido')]",
            "//div[contains(text(), 'Dashboard')]",
            "//a[contains(text(), 'Logout')]",
            "//a[contains(text(), 'Cerrar')]",
            "//a[contains(text(), 'Salir')]",
            "//button[contains(text(), 'Logout')]",
            "//button[contains(text(), 'Cerrar')]",
            "//div[contains(@class, 'user-info')]",
            "//div[contains(@class, 'profile')]",
            "//*[contains(@class, 'error-message') and not(contains(@style, 'display: none'))]"
        };
        
        // Opciones de Chrome
        public static readonly string[] CHROME_OPTIONS = 
        {
            "--start-maximized",
            "--disable-web-security",
            "--allow-running-insecure-content",
            "--disable-features=VizDisplayCompositor",
            "--no-sandbox",
            "--disable-dev-shm-usage"
        };
    }
}
