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
                
                report.AddStep("Proceso de login completado exitosamente");
                report.SetTestResult(true);
                
                Console.WriteLine("\n🎉 Login exitoso!");
                
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
    }
}
