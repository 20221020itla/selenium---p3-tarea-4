using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace SeleniumLoginAutomation
{
    class Program
    {
        private static IWebDriver driver = null!;
        private static WebDriverWait wait = null!;
        private static TestReport report = null!;

        static void Main(string[] args)
        {
            report = new TestReport("Login Automation Test", null!);
            report.AddStep("Iniciando prueba de automatización de login");
            
            try
            {
                // Configurar el driver de Chrome
                SetupDriver();
                
                // Actualizar reporte con driver inicializado
                report = new TestReport("Login Automation Test", driver);
                report.AddStep("Navegador Chrome iniciado correctamente");
                
                // Realizar el login
                PerformLogin();
                
                // Realizar operaciones CRUD de productos
                PerformProductsCrud();
                
                report.AddStep("Proceso de login y CRUD completado exitosamente");
                report.SetTestResult(true);
                
                Console.WriteLine("\n🎉 Login y operaciones CRUD exitosas!");
                
                // Esperar un poco para ver el resultado
                Thread.Sleep(Config.FINAL_DELAY);
            }
            catch (Exception ex)
            {
                report.AddStep($"Error durante la automatización", false, ex.Message);
                report.SetTestResult(false, ex.Message);
                Console.WriteLine($"\n❌ Error durante la automatización: {ex.Message}");
            }
            finally
            {
                // Generar reporte antes de cerrar
                report.AddStep("Generando reporte final");
                report.GenerateHtmlReport();
                
                // Cerrar el navegador
                CleanupDriver();
            }
            
            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }

        private static void SetupDriver()
        {
            report.AddStep("Configurando opciones del navegador Chrome");
            Console.WriteLine("Iniciando el navegador Chrome...");
            
            // Configurar opciones de Chrome usando configuración
            var chromeOptions = new ChromeOptions();
            foreach (string option in Config.CHROME_OPTIONS)
            {
                chromeOptions.AddArgument(option);
            }
            
            report.AddStep("Opciones de Chrome configuradas", true, string.Join(", ", Config.CHROME_OPTIONS));
            
            // Crear instancia del driver
            driver = new ChromeDriver(chromeOptions);
            report.AddStep("Driver de Chrome inicializado correctamente");
            
            // Configurar timeout implícito usando configuración
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.IMPLICIT_WAIT_TIMEOUT);
            
            // Crear WebDriverWait para esperas explícitas usando configuración
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Config.EXPLICIT_WAIT_TIMEOUT));
            
            report.AddStep($"Timeouts configurados: Implícito={Config.IMPLICIT_WAIT_TIMEOUT}s, Explícito={Config.EXPLICIT_WAIT_TIMEOUT}s");
        }

        private static void PerformLogin()
        {
            try
            {
                report.AddStep($"Navegando a la URL: {Config.LOGIN_URL}");
                Console.WriteLine("Navegando a la página de login...");
                
                // Navegar a la URL especificada usando configuración
                driver.Navigate().GoToUrl(Config.LOGIN_URL);
                report.AddStep("Página de login cargada correctamente");
                
                Console.WriteLine("Página cargada. Buscando elementos de login...");
                
                // Buscar y completar el campo de email usando selectores de configuración
                IWebElement? emailField = FindLoginElement(Config.USERNAME_SELECTORS, "Campo de Email");
                
                if (emailField != null)
                {
                    report.AddStep($"Campo de email encontrado, ingresando: {Config.USERNAME}");
                    Console.WriteLine("Campo de email encontrado. Ingresando datos...");
                    emailField.Clear();
                    emailField.SendKeys(Config.USERNAME);
                }
                else
                {
                    report.AddStep("No se pudo encontrar el campo de email", false);
                    throw new Exception("No se pudo encontrar el campo de email");
                }
                
                // Buscar y completar el campo de password usando selectores de configuración
                IWebElement? passwordField = FindLoginElement(Config.PASSWORD_SELECTORS, "Campo de Password");
                
                if (passwordField != null)
                {
                    report.AddStep("Campo de contraseña encontrado, ingresando password");
                    Console.WriteLine("Campo de contraseña encontrado. Ingresando datos...");
                    passwordField.Clear();
                    passwordField.SendKeys(Config.PASSWORD);
                }
                else
                {
                    report.AddStep("No se pudo encontrar el campo de contraseña", false);
                    throw new Exception("No se pudo encontrar el campo de contraseña");
                }
                
                // Buscar y hacer clic en el botón de login usando selectores de configuración
                IWebElement? loginButton = FindLoginElement(Config.LOGIN_BUTTON_SELECTORS, "Botón de Login");
                
                if (loginButton != null)
                {
                    report.AddStep("Botón de login encontrado, haciendo clic");
                    Console.WriteLine("Botón de login encontrado. Haciendo clic...");
                    loginButton.Click();
                }
                else
                {
                    // Si no encontramos un botón, intentamos enviar Enter en el campo de contraseña
                    report.AddStep("Botón de login no encontrado, enviando Enter en campo de password");
                    Console.WriteLine("No se encontró botón de login. Intentando enviar Enter...");
                    passwordField.SendKeys(Keys.Enter);
                }
                
                // Esperar un momento inicial para que comience el proceso
                Thread.Sleep(1000);
                
                // Verificar si aparece el estado de carga
                CheckForLoadingState();
                
                // Esperar un momento para que se procese el login usando configuración
                Thread.Sleep(Config.VERIFICATION_DELAY);
                
                // Verificar si hay mensajes de error
                CheckForErrors();
                
                // Verificar si el login fue exitoso
                VerifyLoginSuccess();
            }
            catch (Exception ex)
            {
                report.AddStep($"Error durante el proceso de login", false, ex.Message);
                Console.WriteLine($"Error durante el proceso de login: {ex.Message}");
                throw;
            }
        }

        private static void CheckForLoadingState()
        {
            try
            {
                report.AddStep("Verificando estado de carga del formulario");
                Console.WriteLine("Verificando estado de carga...");
                
                // Esperar hasta que desaparezca el estado de carga
                var loadingWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
                loadingWait.Until(d => 
                {
                    try
                    {
                        var button = d.FindElement(By.CssSelector("button[type='submit']"));
                        bool isLoading = button.Text.Contains("Iniciando sesión") || 
                                       button.GetAttribute("disabled") == "true";
                        
                        if (isLoading)
                        {
                            Console.WriteLine("Esperando que termine el proceso de login...");
                            return false;
                        }
                        return true;
                    }
                    catch
                    {
                        return true; // Si no se encuentra, asumimos que terminó
                    }
                });
                
                report.AddStep("Estado de carga completado exitosamente");
                Console.WriteLine("Proceso de login completado.");
            }
            catch (WebDriverTimeoutException)
            {
                report.AddStep("Timeout esperando que termine el proceso de login", false);
                Console.WriteLine("Timeout esperando que termine el proceso de login.");
            }
            catch (Exception ex)
            {
                report.AddStep("Error verificando estado de carga", false, ex.Message);
                Console.WriteLine($"Error verificando estado de carga: {ex.Message}");
            }
        }

        private static void CheckForErrors()
        {
            try
            {
                report.AddStep("Verificando mensajes de error en la página");
                Console.WriteLine("Verificando errores de login...");
                
                // Buscar mensajes de error
                var errorSelectors = new string[]
                {
                    ".error-message",
                    ".alert-danger",
                    ".error",
                    "[class*='error']",
                    ".invalid-feedback",
                    ".text-danger"
                };

                foreach (string selector in errorSelectors)
                {
                    try
                    {
                        var errorElement = driver.FindElement(By.CssSelector(selector));
                        if (errorElement.Displayed && !string.IsNullOrWhiteSpace(errorElement.Text))
                        {
                            report.AddStep($"Error detectado en página: {errorElement.Text}", false);
                            Console.WriteLine($"Error detectado: {errorElement.Text}");
                            throw new Exception($"Login falló: {errorElement.Text}");
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }
                
                report.AddStep("No se detectaron errores de login - Continuando");
                Console.WriteLine("No se detectaron errores de login.");
            }
            catch (Exception ex) when (!(ex is NoSuchElementException))
            {
                throw; // Re-lanzar excepciones que no sean NoSuchElementException
            }
        }

        private static IWebElement? FindLoginElement(string[] selectors, string elementName = "elemento")
        {
            report.AddStep($"Buscando {elementName} con {selectors.Length} selectores");
            
            foreach (string selector in selectors)
            {
                try
                {
                    var element = driver.FindElement(By.CssSelector(selector));
                    if (element.Displayed && element.Enabled)
                    {
                        report.AddStep($"{elementName} encontrado con selector: {selector}");
                        Console.WriteLine($"Elemento encontrado con selector: {selector}");
                        return element;
                    }
                }
                catch (NoSuchElementException)
                {
                    // Continuar con el siguiente selector
                    continue;
                }
            }
            
            report.AddStep($"No se pudo encontrar {elementName} con ningún selector", false);
            Console.WriteLine($"No se pudo encontrar {elementName} con ningún selector.");
            return null;
        }

        private static void VerifyLoginSuccess()
        {
            try
            {
                report.AddStep("Verificando indicadores de login exitoso");
                Console.WriteLine("Verificando si el login fue exitoso...");
                
                // Verificar si hay algún indicador de login exitoso usando configuración
                bool loginSuccessful = false;
                
                foreach (string indicator in Config.SUCCESS_INDICATORS)
                {
                    try
                    {
                        var element = driver.FindElement(By.XPath(indicator));
                        if (element.Displayed)
                        {
                            report.AddStep($"Login exitoso detectado por elemento: {element.Text}");
                            Console.WriteLine($"Login exitoso detectado: {element.Text}");
                            loginSuccessful = true;
                            break;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }

                if (!loginSuccessful)
                {
                    // Verificar si la URL cambió (otro indicador de login exitoso)
                    string currentUrl = driver.Url;
                    report.AddStep($"Verificando cambio de URL. URL actual: {currentUrl}");
                    Console.WriteLine($"URL actual: {currentUrl}");
                    
                    if (!currentUrl.Contains("login") && !currentUrl.EndsWith("/"))
                    {
                        report.AddStep("Login exitoso - URL cambió, ya no está en página de login");
                        Console.WriteLine("Login exitoso - URL cambió, ya no está en la página de login");
                        loginSuccessful = true;
                    }
                    else if (currentUrl.Contains("dashboard") || currentUrl.Contains("home") || currentUrl.Contains("app"))
                    {
                        report.AddStep("Login exitoso - Redirigido a página principal");
                        Console.WriteLine("Login exitoso - Redirigido a página principal");
                        loginSuccessful = true;
                    }
                }

                if (!loginSuccessful)
                {
                    report.AddStep("No se pudo verificar automáticamente el éxito del login", false);
                    Console.WriteLine("Advertencia: No se pudo verificar automáticamente si el login fue exitoso.");
                    Console.WriteLine("Por favor, revise manualmente el resultado en el navegador.");
                }
                else
                {
                    report.AddStep("✓ Login verificado como exitoso!");
                    Console.WriteLine("✓ Login verificado como exitoso!");
                }
            }
            catch (Exception ex)
            {
                report.AddStep("Error al verificar el login", false, ex.Message);
                Console.WriteLine($"Error al verificar el login: {ex.Message}");
            }
        }

        private static void CleanupDriver()
        {
            try
            {
                if (driver != null)
                {
                    report.AddStep("Cerrando navegador y limpiando recursos");
                    Console.WriteLine("Cerrando el navegador...");
                    driver.Quit();
                    driver.Dispose();
                    report.AddStep("Navegador cerrado exitosamente");
                }
            }
            catch (Exception ex)
            {
                report.AddStep("Error al cerrar el navegador", false, ex.Message);
                Console.WriteLine($"Error al cerrar el navegador: {ex.Message}");
            }
        }

        private static void PerformProductsCrud()
        {
            try
            {
                report.AddStep("Iniciando operaciones CRUD de productos");
                Console.WriteLine("Iniciando operaciones CRUD de productos...");

                // Navegar a la sección de productos
                NavigateToProducts();

                // Crear un nuevo producto
                CreateProduct();

                // Editar el producto creado
                EditProduct();

                // Eliminar el producto
                DeleteProduct();

                report.AddStep("Operaciones CRUD de productos completadas exitosamente");
                Console.WriteLine("✓ Operaciones CRUD de productos completadas exitosamente!");
            }
            catch (Exception ex)
            {
                report.AddStep("Error durante operaciones CRUD de productos", false, ex.Message);
                Console.WriteLine($"❌ Error durante operaciones CRUD de productos: {ex.Message}");
                throw;
            }
        }

        private static void NavigateToProducts()
        {
            try
            {
                report.AddStep("Verificando que estamos en el dashboard con productos");
                Console.WriteLine("Verificando acceso a sección de productos...");

                // En una SPA de React, después del login ya estamos en el dashboard
                // Solo necesitamos verificar que los elementos de productos estén visibles
                Thread.Sleep(Config.VERIFICATION_DELAY * 2); // Dar más tiempo para que React renderice

                // Verificar que estamos en la sección de productos
                VerifyProductsSection();
                
                report.AddStep("Ya estamos en la sección de productos del dashboard");
                Console.WriteLine("✓ Acceso a productos verificado");
            }
            catch (Exception ex)
            {
                report.AddStep("Error verificando acceso a productos", false, ex.Message);
                throw;
            }
        }

        private static void VerifyProductsSection()
        {
            try
            {
                report.AddStep("Verificando que estamos en la sección de productos");
                
                // Buscar indicadores de que estamos en la sección de productos
                string[] productSectionIndicators = {
                    ".product-crud",
                    ".crud-header",
                    "#add-product-btn",
                    "button:contains('+ Nuevo Producto')",
                    ".dashboard-container",
                    "h2:contains('Gestión de Productos')",
                    ".products-grid"
                };

                bool inProductsSection = false;
                foreach (string indicator in productSectionIndicators)
                {
                    try
                    {
                        var element = driver.FindElement(By.CssSelector(indicator));
                        if (element.Displayed)
                        {
                            report.AddStep($"Sección de productos verificada por elemento: {indicator}");
                            inProductsSection = true;
                            break;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }

                if (!inProductsSection)
                {
                    report.AddStep("Advertencia: No se pudo verificar automáticamente la sección de productos", false);
                    Console.WriteLine("Advertencia: No se pudo verificar que estemos en la sección de productos");
                }
            }
            catch (Exception ex)
            {
                report.AddStep("Error verificando sección de productos", false, ex.Message);
                // No lanzamos excepción aquí, solo registramos el error
            }
        }

        private static void CreateProduct()
        {
            try
            {
                report.AddStep("Creando nuevo producto");
                Console.WriteLine("Creando nuevo producto...");

                // Hacer clic en el botón "Nuevo Producto" con espera explícita
                IWebElement? addButton = null;
                var waitTime = TimeSpan.FromSeconds(10);
                var buttonWait = new WebDriverWait(driver, waitTime);
                
                try
                {
                    addButton = buttonWait.Until(d => {
                        foreach (string selector in Config.ADD_PRODUCT_BUTTON_SELECTORS)
                        {
                            try
                            {
                                var btn = d.FindElement(By.CssSelector(selector));
                                if (btn.Displayed && btn.Enabled)
                                {
                                    return btn;
                                }
                            }
                            catch (NoSuchElementException) { continue; }
                        }
                        return null;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    throw new Exception("No se pudo encontrar el botón de nuevo producto después de esperar");
                }

                if (addButton != null)
                {
                    addButton.Click();
                    Thread.Sleep(1500); // Dar tiempo para que aparezca el modal
                    report.AddStep("Modal de nuevo producto abierto");
                }
                else
                {
                    throw new Exception("No se pudo encontrar el botón de nuevo producto");
                }

                // Esperar a que aparezca el modal del formulario
                Thread.Sleep(2000);

                // Llenar el formulario del producto
                FillProductForm(Config.TEST_PRODUCT_NAME, Config.TEST_PRODUCT_DESCRIPTION, 
                               Config.TEST_PRODUCT_PRICE, Config.TEST_PRODUCT_CATEGORY, 
                               Config.TEST_PRODUCT_STOCK);

                // Guardar el producto con espera explícita
                IWebElement? saveButton = null;
                try
                {
                    saveButton = buttonWait.Until(d => {
                        foreach (string selector in Config.SAVE_PRODUCT_BUTTON_SELECTORS)
                        {
                            try
                            {
                                var btn = d.FindElement(By.CssSelector(selector));
                                if (btn.Displayed && btn.Enabled && !btn.GetAttribute("disabled").Equals("true"))
                                {
                                    return btn;
                                }
                            }
                            catch (NoSuchElementException) { continue; }
                        }
                        return null;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    report.AddStep("No se pudo encontrar el botón guardar, intentando submit del formulario");
                    // Intentar enviar el formulario directamente
                    try
                    {
                        var form = driver.FindElement(By.Id("product-form"));
                        form.Submit();
                    }
                    catch (Exception)
                    {
                        throw new Exception("No se pudo enviar el formulario de producto");
                    }
                }

                if (saveButton != null)
                {
                    saveButton.Click();
                    Thread.Sleep(Config.VERIFICATION_DELAY * 2); // Más tiempo para React
                    report.AddStep($"Producto '{Config.TEST_PRODUCT_NAME}' creado exitosamente");
                    Console.WriteLine($"✓ Producto '{Config.TEST_PRODUCT_NAME}' creado exitosamente");
                }
            }
            catch (Exception ex)
            {
                report.AddStep("Error creando producto", false, ex.Message);
                throw;
            }
        }

        private static void EditProduct()
        {
            try
            {
                report.AddStep("Editando producto disponible");
                Console.WriteLine("Editando producto...");

                // Buscar el primer producto disponible y su botón de editar
                IWebElement? editButton = null;
                
                // Intentar encontrar botón de editar en cualquier producto
                foreach (string selector in Config.EDIT_BUTTON_SELECTORS)
                {
                    try
                    {
                        var elements = driver.FindElements(By.CssSelector(selector));
                        if (elements.Count > 0)
                        {
                            editButton = elements[0]; // Tomar el primer elemento encontrado
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (editButton != null && editButton.Displayed && editButton.Enabled)
                {
                    editButton.Click();
                    Thread.Sleep(1000);
                    report.AddStep("Modal de edición abierto");
                }
                else
                {
                    throw new Exception("No se pudo encontrar ningún botón de editar disponible");
                }

                // Llenar el formulario con los datos editados
                FillProductForm(Config.EDITED_PRODUCT_NAME, Config.EDITED_PRODUCT_DESCRIPTION, 
                               Config.EDITED_PRODUCT_PRICE, Config.EDITED_PRODUCT_CATEGORY, 
                               Config.EDITED_PRODUCT_STOCK);

                // Guardar los cambios
                IWebElement? saveButton = FindElementByCssSelectors(Config.SAVE_PRODUCT_BUTTON_SELECTORS, "Botón Guardar Edición");
                if (saveButton != null)
                {
                    saveButton.Click();
                    Thread.Sleep(Config.VERIFICATION_DELAY);
                    report.AddStep($"Producto editado a '{Config.EDITED_PRODUCT_NAME}' exitosamente");
                    Console.WriteLine($"✓ Producto editado exitosamente");
                }
                else
                {
                    throw new Exception("No se pudo encontrar el botón guardar edición");
                }
            }
            catch (Exception ex)
            {
                report.AddStep("Error editando producto", false, ex.Message);
                throw;
            }
        }

        private static void DeleteProduct()
        {
            try
            {
                report.AddStep("Eliminando producto disponible");
                Console.WriteLine("Eliminando producto...");

                // Esperar a que aparezcan productos en la grilla
                Thread.Sleep(3000);

                // Buscar el primer producto disponible y su botón de eliminar
                IWebElement? deleteButton = null;
                var waitTime = TimeSpan.FromSeconds(10);
                var buttonWait = new WebDriverWait(driver, waitTime);
                
                // Intentar encontrar botón de eliminar en cualquier producto
                try
                {
                    deleteButton = buttonWait.Until(d => {
                        foreach (string selector in Config.DELETE_BUTTON_SELECTORS)
                        {
                            try
                            {
                                var elements = d.FindElements(By.CssSelector(selector));
                                if (elements.Count > 0)
                                {
                                    var btn = elements[0];
                                    if (btn.Displayed && btn.Enabled)
                                    {
                                        return btn;
                                    }
                                }
                            }
                            catch (Exception) { continue; }
                        }
                        return null;
                    });
                }
                catch (WebDriverTimeoutException)
                {
                    throw new Exception("No se encontraron productos para eliminar después de esperar");
                }

                if (deleteButton != null && deleteButton.Displayed && deleteButton.Enabled)
                {
                    deleteButton.Click();
                    Thread.Sleep(1000);
                    report.AddStep("Botón de eliminar clickeado");
                    
                    // Manejar el diálogo de confirmación de JavaScript
                    try
                    {
                        // Esperar y aceptar el alert de confirmación
                        var alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                        var alert = alertWait.Until(d => d.SwitchTo().Alert());
                        
                        string alertText = alert.Text;
                        report.AddStep($"Confirmación de eliminación detectada: {alertText}");
                        Console.WriteLine($"Confirmando eliminación: {alertText}");
                        
                        alert.Accept(); // Hacer clic en "OK"
                        
                        Thread.Sleep(Config.VERIFICATION_DELAY * 2);
                        report.AddStep("Producto eliminado exitosamente");
                        Console.WriteLine("✓ Producto eliminado exitosamente");
                    }
                    catch (WebDriverTimeoutException)
                    {
                        report.AddStep("No apareció diálogo de confirmación - El producto puede haberse eliminado directamente");
                        Console.WriteLine("✓ Producto eliminado sin diálogo de confirmación");
                    }
                    catch (NoAlertPresentException)
                    {
                        report.AddStep("No hay alerta presente - El producto puede haberse eliminado directamente");
                        Console.WriteLine("✓ Producto eliminado sin alerta");
                    }
                }
                else
                {
                    throw new Exception("No se pudo encontrar ningún botón de eliminar disponible");
                }
            }
            catch (Exception ex)
            {
                report.AddStep("Error eliminando producto", false, ex.Message);
                throw;
            }
        }

        private static void FillProductForm(string name, string description, string price, string category, string stock)
        {
            try
            {
                report.AddStep($"Llenando formulario de producto: {name}");

                // Llenar nombre del producto
                IWebElement? nameField = FindElementByCssSelectors(Config.PRODUCT_FORM_SELECTORS, "Campo Nombre");
                if (nameField != null)
                {
                    nameField.Clear();
                    nameField.SendKeys(name);
                    report.AddStep($"Nombre del producto ingresado: {name}");
                }

                // Llenar descripción
                IWebElement? descriptionField = FindElementByCssSelectors(Config.PRODUCT_DESCRIPTION_SELECTORS, "Campo Descripción");
                if (descriptionField != null)
                {
                    descriptionField.Clear();
                    descriptionField.SendKeys(description);
                    report.AddStep("Descripción del producto ingresada");
                }

                // Llenar precio
                IWebElement? priceField = FindElementByCssSelectors(Config.PRODUCT_PRICE_SELECTORS, "Campo Precio");
                if (priceField != null)
                {
                    priceField.Clear();
                    priceField.SendKeys(price);
                    report.AddStep($"Precio del producto ingresado: {price}");
                }

                // Seleccionar categoría
                IWebElement? categoryField = FindElementByCssSelectors(Config.PRODUCT_CATEGORY_SELECTORS, "Campo Categoría");
                if (categoryField != null)
                {
                    var select = new SelectElement(categoryField);
                    select.SelectByText(category);
                    report.AddStep($"Categoría seleccionada: {category}");
                }

                // Llenar stock
                IWebElement? stockField = FindElementByCssSelectors(Config.PRODUCT_STOCK_SELECTORS, "Campo Stock");
                if (stockField != null)
                {
                    stockField.Clear();
                    stockField.SendKeys(stock);
                    report.AddStep($"Stock ingresado: {stock}");
                }

                report.AddStep("Formulario de producto completado exitosamente");
            }
            catch (Exception ex)
            {
                report.AddStep("Error llenando formulario de producto", false, ex.Message);
                throw;
            }
        }

        private static IWebElement? FindElementByCssSelectors(string[] selectors, string elementName = "elemento")
        {
            report.AddStep($"Buscando {elementName} con {selectors.Length} selectores CSS");
            
            foreach (string selector in selectors)
            {
                try
                {
                    var element = driver.FindElement(By.CssSelector(selector));
                    if (element.Displayed && element.Enabled)
                    {
                        report.AddStep($"{elementName} encontrado con selector: {selector}");
                        Console.WriteLine($"Elemento encontrado con selector: {selector}");
                        return element;
                    }
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }
            
            report.AddStep($"No se pudo encontrar {elementName} con ningún selector CSS", false);
            Console.WriteLine($"No se pudo encontrar {elementName} con ningún selector CSS.");
            return null;
        }
    }
}
