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

        static void Main(string[] args)
        {
            try
            {
                // Configurar el driver de Chrome
                SetupDriver();
                
                // Realizar el login
                PerformLogin();
                
                Console.WriteLine("Login exitoso!");
                
                // Esperar un poco para ver el resultado
                Thread.Sleep(Config.FINAL_DELAY);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error durante la automatización: {ex.Message}");
            }
            finally
            {
                // Cerrar el navegador
                CleanupDriver();
            }
            
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }

        private static void SetupDriver()
        {
            Console.WriteLine("Iniciando el navegador Chrome...");
            
            // Configurar opciones de Chrome usando configuración
            var chromeOptions = new ChromeOptions();
            foreach (string option in Config.CHROME_OPTIONS)
            {
                chromeOptions.AddArgument(option);
            }
            
            // Crear instancia del driver
            driver = new ChromeDriver(chromeOptions);
            
            // Configurar timeout implícito usando configuración
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(Config.IMPLICIT_WAIT_TIMEOUT);
            
            // Crear WebDriverWait para esperas explícitas usando configuración
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Config.EXPLICIT_WAIT_TIMEOUT));
        }

        private static void PerformLogin()
        {
            try
            {
                Console.WriteLine("Navegando a la página de login...");
                
                // Navegar a la URL especificada usando configuración
                driver.Navigate().GoToUrl(Config.LOGIN_URL);
                
                Console.WriteLine("Página cargada. Buscando elementos de login...");
                
                // Buscar y completar el campo de email usando selectores de configuración
                IWebElement? emailField = FindLoginElement(Config.USERNAME_SELECTORS);
                
                if (emailField != null)
                {
                    Console.WriteLine("Campo de email encontrado. Ingresando datos...");
                    emailField.Clear();
                    emailField.SendKeys(Config.USERNAME);
                }
                else
                {
                    throw new Exception("No se pudo encontrar el campo de email");
                }
                
                // Buscar y completar el campo de password usando selectores de configuración
                IWebElement? passwordField = FindLoginElement(Config.PASSWORD_SELECTORS);
                
                if (passwordField != null)
                {
                    Console.WriteLine("Campo de contraseña encontrado. Ingresando datos...");
                    passwordField.Clear();
                    passwordField.SendKeys(Config.PASSWORD);
                }
                else
                {
                    throw new Exception("No se pudo encontrar el campo de contraseña");
                }
                
                // Buscar y hacer clic en el botón de login usando selectores de configuración
                IWebElement? loginButton = FindLoginElement(Config.LOGIN_BUTTON_SELECTORS);
                
                if (loginButton != null)
                {
                    Console.WriteLine("Botón de login encontrado. Haciendo clic...");
                    loginButton.Click();
                }
                else
                {
                    // Si no encontramos un botón, intentamos enviar Enter en el campo de contraseña
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
                Console.WriteLine($"Error durante el proceso de login: {ex.Message}");
                throw;
            }
        }

        private static void CheckForLoadingState()
        {
            try
            {
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
                
                Console.WriteLine("Proceso de login completado.");
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Timeout esperando que termine el proceso de login.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verificando estado de carga: {ex.Message}");
            }
        }

        private static void CheckForErrors()
        {
            try
            {
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
                            Console.WriteLine($"Error detectado: {errorElement.Text}");
                            throw new Exception($"Login falló: {errorElement.Text}");
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }
                
                Console.WriteLine("No se detectaron errores de login.");
            }
            catch (Exception ex) when (!(ex is NoSuchElementException))
            {
                throw; // Re-lanzar excepciones que no sean NoSuchElementException
            }
        }

        private static IWebElement? FindLoginElement(string[] selectors)
        {
            foreach (string selector in selectors)
            {
                try
                {
                    var element = driver.FindElement(By.CssSelector(selector));
                    if (element.Displayed && element.Enabled)
                    {
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
            Console.WriteLine("No se pudo encontrar el elemento con ningún selector.");
            return null;
        }

        private static void VerifyLoginSuccess()
        {
            try
            {
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
                    Console.WriteLine($"URL actual: {currentUrl}");
                    
                    if (!currentUrl.Contains("login") && !currentUrl.EndsWith("/"))
                    {
                        Console.WriteLine("Login exitoso - URL cambió, ya no está en la página de login");
                        loginSuccessful = true;
                    }
                    else if (currentUrl.Contains("dashboard") || currentUrl.Contains("home") || currentUrl.Contains("app"))
                    {
                        Console.WriteLine("Login exitoso - Redirigido a página principal");
                        loginSuccessful = true;
                    }
                }

                if (!loginSuccessful)
                {
                    Console.WriteLine("Advertencia: No se pudo verificar automáticamente si el login fue exitoso.");
                    Console.WriteLine("Por favor, revise manualmente el resultado en el navegador.");
                }
                else
                {
                    Console.WriteLine("✓ Login verificado como exitoso!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar el login: {ex.Message}");
            }
        }

        private static void CleanupDriver()
        {
            try
            {
                if (driver != null)
                {
                    Console.WriteLine("Cerrando el navegador...");
                    driver.Quit();
                    driver.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar el navegador: {ex.Message}");
            }
        }
    }
}
