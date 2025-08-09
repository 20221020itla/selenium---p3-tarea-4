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
        
        // Configuración de productos CRUD
        public const string TEST_PRODUCT_NAME = "Producto Test Selenium";
        public const string TEST_PRODUCT_DESCRIPTION = "Producto creado automáticamente para pruebas";
        public const string TEST_PRODUCT_PRICE = "99.99";
        public const string TEST_PRODUCT_CATEGORY = "Electrónicos";
        public const string TEST_PRODUCT_STOCK = "50";
        
        // Datos editados del producto
        public const string EDITED_PRODUCT_NAME = "Producto Test Editado";
        public const string EDITED_PRODUCT_DESCRIPTION = "Producto editado automáticamente";
        public const string EDITED_PRODUCT_PRICE = "149.99";
        public const string EDITED_PRODUCT_CATEGORY = "Tecnología";
        public const string EDITED_PRODUCT_STOCK = "75";
        
        // Selectores CSS para campos de login (en orden de prioridad)
        public static readonly string[] USERNAME_SELECTORS = 
        {
            "input[name='email']",
            "input[id='email']",
            "#email",
            "input[type='email']",
            "input[name='username']",
            "input[id='username']",
            "#username",
            "input[type='text']",
            ".login-form input[type='email']",
            ".form-group input[type='email']",
            ".login-form input[type='text']",
            ".form-group input[type='text']"
        };
        
        public static readonly string[] PASSWORD_SELECTORS = 
        {
            "input[name='password']",
            "input[id='password']",
            "#password",
            "input[type='password']",
            ".login-form input[type='password']",
            ".form-group input[type='password']",
            ".password"
        };
        
        public static readonly string[] LOGIN_BUTTON_SELECTORS = 
        {
            "button[type='submit']",
            ".login-btn",
            "button.login-btn",
            ".login-form button[type='submit']",
            "form button[type='submit']",
            "button:contains('Iniciar Sesión')",
            "input[type='submit']",
            ".btn-login"
        };
        
        // XPath selectors para verificar login exitoso
        public static readonly string[] SUCCESS_INDICATORS = 
        {
            "//div[contains(@class, 'success-message')]",
            "//div[contains(@class, 'dashboard')]",
            "//div[contains(@class, 'dashboard-container')]",
            "//h1[contains(text(), 'Bienvenido')]",
            "//div[contains(text(), 'Bienvenido')]",
            "//div[contains(text(), 'Dashboard')]",
            "//h1[contains(text(), 'Dashboard')]",
            "//button[contains(text(), 'Cerrar Sesión')]",
            "//button[contains(@class, 'logout-button')]",
            "//div[contains(@class, 'welcome-section')]",
            "//div[contains(@class, 'dashboard-header')]",
            "//div[contains(@class, 'product-crud')]",
            "//button[contains(text(), 'Nuevo Producto')]",
            "//button[contains(@id, 'add-product-btn')]"
        };
        
        // Selectores para navegación y CRUD de productos
        public static readonly string[] PRODUCTS_TAB_SELECTORS = 
        {
            ".dashboard-container",
            ".product-crud",
            ".crud-header",
            "h2:contains('Gestión de Productos')",
            ".dashboard-header"
        };
        
        public static readonly string[] ADD_PRODUCT_BUTTON_SELECTORS = 
        {
            "#add-product-btn",
            "button:contains('+ Nuevo Producto')",
            ".btn-primary:contains('Nuevo')",
            ".crud-header .btn-primary",
            "button:contains('Nuevo Producto')"
        };
        
        public static readonly string[] PRODUCT_FORM_SELECTORS = 
        {
            "#product-name",
            "input[name='name']",
            "input[id='product-name']",
            ".product-form input[name='name']"
        };
        
        public static readonly string[] PRODUCT_DESCRIPTION_SELECTORS = 
        {
            "#product-description",
            "textarea[name='description']",
            "textarea[id='product-description']",
            ".product-form textarea[name='description']"
        };
        
        public static readonly string[] PRODUCT_PRICE_SELECTORS = 
        {
            "#product-price",
            "input[name='price']",
            "input[id='product-price']",
            ".product-form input[name='price']"
        };
        
        public static readonly string[] PRODUCT_CATEGORY_SELECTORS = 
        {
            "#product-category",
            "select[name='category']",
            "select[id='product-category']",
            ".product-form select[name='category']"
        };
        
        public static readonly string[] PRODUCT_STOCK_SELECTORS = 
        {
            "#product-stock",
            "input[name='stock']",
            "input[id='product-stock']",
            ".product-form input[name='stock']"
        };
        
        public static readonly string[] SAVE_PRODUCT_BUTTON_SELECTORS = 
        {
            "#submit-btn",
            "button[type='submit']",
            ".form-actions .btn-primary",
            ".modal-content button[type='submit']",
            "button:contains('Crear')",
            "button:contains('Actualizar')"
        };
        
        public static readonly string[] EDIT_BUTTON_SELECTORS = 
        {
            ".btn-edit[data-action='edit']",
            ".btn-edit-text[data-action='edit']",
            "button[data-action='edit']",
            ".product-actions .btn-edit-text",
            ".product-overlay .btn-edit",
            "button:contains('Editar')",
            ".btn-edit",
            "[data-action='edit']"
        };
        
        public static readonly string[] DELETE_BUTTON_SELECTORS = 
        {
            ".btn-delete[data-action='delete']",
            ".btn-delete-text[data-action='delete']",
            "button[data-action='delete']",
            ".product-actions .btn-delete-text",
            ".product-overlay .btn-delete",
            "button:contains('Eliminar')",
            ".btn-delete",
            "[data-action='delete']"
        };
        
        public static readonly string[] CONFIRM_DELETE_SELECTORS = 
        {
            "button:contains('Confirmar')",
            "button:contains('Sí')",
            "button:contains('Eliminar')",
            ".btn-danger",
            ".modal-confirm button:first-child"
        };
        
        // Selectores para encontrar productos específicos
        public static readonly string[] FIRST_PRODUCT_CARD_SELECTORS = 
        {
            ".product-card:first-child",
            ".product-card:first-of-type",
            ".product-card[data-product-id]",
            ".product-card"
        };
        
        // Selectores para verificar que el producto fue creado
        public static readonly string[] PRODUCT_CREATED_INDICATORS = 
        {
            ".product-card:contains('Producto Test Selenium')",
            ".product-name:contains('Producto Test Selenium')",
            "h4:contains('Producto Test Selenium')",
            ".product-card .product-name"
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
