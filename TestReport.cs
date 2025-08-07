using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SeleniumLoginAutomation
{
    public class TestReport
    {
        private List<TestStep> steps;
        private string testName;
        private DateTime startTime;
        private DateTime endTime;
        private bool testPassed;
        private string errorMessage;
        private IWebDriver driver;

        public TestReport(string testName, IWebDriver driver)
        {
            this.testName = testName;
            this.driver = driver;
            this.steps = new List<TestStep>();
            this.startTime = DateTime.Now;
            this.testPassed = false;
            this.errorMessage = string.Empty;
            
            // Crear directorios si no existen
            CreateDirectories();
        }

        private void CreateDirectories()
        {
            if (!Directory.Exists(Config.REPORT_FOLDER))
                Directory.CreateDirectory(Config.REPORT_FOLDER);
                
            if (!Directory.Exists(Path.Combine(Config.REPORT_FOLDER, Config.SCREENSHOTS_FOLDER)))
                Directory.CreateDirectory(Path.Combine(Config.REPORT_FOLDER, Config.SCREENSHOTS_FOLDER));
        }

        public void AddStep(string description, bool success = true, string details = "")
        {
            string screenshotPath = "";
            
            if (Config.TAKE_SCREENSHOTS && driver != null)
            {
                screenshotPath = TakeScreenshot(description);
            }
            
            var step = new TestStep
            {
                StepNumber = steps.Count + 1,
                Description = description,
                Success = success,
                Details = details,
                Timestamp = DateTime.Now,
                ScreenshotPath = screenshotPath
            };
            
            steps.Add(step);
            
            // Log a consola también
            string status = success ? "✓" : "✗";
            Console.WriteLine($"{status} Paso {step.StepNumber}: {description}");
            if (!string.IsNullOrEmpty(details))
            {
                Console.WriteLine($"   Detalles: {details}");
            }
        }

        private string TakeScreenshot(string stepDescription)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string fileName = $"step_{steps.Count + 1:00}_{SanitizeFileName(stepDescription)}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                string fullPath = Path.Combine(Config.REPORT_FOLDER, Config.SCREENSHOTS_FOLDER, fileName);
                
                screenshot.SaveAsFile(fullPath);
                return Path.Combine(Config.SCREENSHOTS_FOLDER, fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error tomando captura de pantalla: {ex.Message}");
                return "";
            }
        }

        private string SanitizeFileName(string fileName)
        {
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            foreach (char c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName.Replace(" ", "_").Substring(0, Math.Min(fileName.Length, 50));
        }

        public void SetTestResult(bool passed, string errorMessage = "")
        {
            this.endTime = DateTime.Now;
            this.testPassed = passed;
            this.errorMessage = errorMessage;
        }

        public void GenerateHtmlReport()
        {
            if (!Config.GENERATE_HTML_REPORT) 
                return;

            string reportPath = Path.Combine(Config.REPORT_FOLDER, $"test_report_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang='es'>");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset='UTF-8'>");
            html.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            html.AppendLine($"    <title>Reporte de Prueba - {testName}</title>");
            html.AppendLine("    <style>");
            html.AppendLine(GetCssStyles());
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Header
            html.AppendLine("    <div class='header'>");
            html.AppendLine("        <h1>🤖 Reporte de Automatización Selenium</h1>");
            html.AppendLine($"        <h2>{testName}</h2>");
            html.AppendLine("    </div>");
            
            // Summary
            html.AppendLine("    <div class='summary'>");
            html.AppendLine("        <h3>📊 Resumen de Ejecución</h3>");
            html.AppendLine("        <div class='summary-grid'>");
            html.AppendLine($"            <div class='summary-item'><strong>Inicio:</strong> {startTime:dd/MM/yyyy HH:mm:ss}</div>");
            html.AppendLine($"            <div class='summary-item'><strong>Fin:</strong> {endTime:dd/MM/yyyy HH:mm:ss}</div>");
            html.AppendLine($"            <div class='summary-item'><strong>Duración:</strong> {(endTime - startTime).TotalSeconds:F1} segundos</div>");
            html.AppendLine($"            <div class='summary-item status-{(testPassed ? "success" : "failure")}'><strong>Resultado:</strong> {(testPassed ? "✓ EXITOSO" : "✗ FALLIDO")}</div>");
            html.AppendLine($"            <div class='summary-item'><strong>Total de Pasos:</strong> {steps.Count}</div>");
            html.AppendLine($"            <div class='summary-item'><strong>Pasos Exitosos:</strong> {steps.Count(s => s.Success)}</div>");
            html.AppendLine("        </div>");
            
            if (!string.IsNullOrEmpty(errorMessage))
            {
                html.AppendLine("        <div class='error-summary'>");
                html.AppendLine($"            <h4>❌ Error Principal:</h4>");
                html.AppendLine($"            <p>{errorMessage}</p>");
                html.AppendLine("        </div>");
            }
            html.AppendLine("    </div>");
            
            // Steps
            html.AppendLine("    <div class='steps-container'>");
            html.AppendLine("        <h3>📝 Pasos de Ejecución</h3>");
            
            foreach (var step in steps)
            {
                string statusClass = step.Success ? "success" : "failure";
                string statusIcon = step.Success ? "✓" : "✗";
                
                html.AppendLine("        <div class='step'>");
                html.AppendLine($"            <div class='step-header status-{statusClass}'>");
                html.AppendLine($"                <span class='step-number'>{step.StepNumber}</span>");
                html.AppendLine($"                <span class='step-status'>{statusIcon}</span>");
                html.AppendLine($"                <span class='step-description'>{step.Description}</span>");
                html.AppendLine($"                <span class='step-time'>{step.Timestamp:HH:mm:ss}</span>");
                html.AppendLine("            </div>");
                
                if (!string.IsNullOrEmpty(step.Details))
                {
                    html.AppendLine("            <div class='step-details'>");
                    html.AppendLine($"                <p><strong>Detalles:</strong> {step.Details}</p>");
                    html.AppendLine("            </div>");
                }
                
                if (!string.IsNullOrEmpty(step.ScreenshotPath))
                {
                    html.AppendLine("            <div class='step-screenshot'>");
                    html.AppendLine($"                <img src='{step.ScreenshotPath}' alt='Captura paso {step.StepNumber}' onclick='openModal(this.src)' />");
                    html.AppendLine("            </div>");
                }
                
                html.AppendLine("        </div>");
            }
            
            html.AppendLine("    </div>");
            
            // Modal for images
            html.AppendLine("    <div id='imageModal' class='modal' onclick='closeModal()'>");
            html.AppendLine("        <span class='close'>&times;</span>");
            html.AppendLine("        <img class='modal-content' id='modalImg'>");
            html.AppendLine("    </div>");
            
            // Footer
            html.AppendLine("    <div class='footer'>");
            html.AppendLine($"        <p>Reporte generado el {DateTime.Now:dd/MM/yyyy HH:mm:ss} por Selenium WebDriver</p>");
            html.AppendLine("        <p>🚀 Automatización desarrollada con C# y Selenium</p>");
            html.AppendLine("    </div>");
            
            html.AppendLine("    <script>");
            html.AppendLine(GetJavaScript());
            html.AppendLine("    </script>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            File.WriteAllText(reportPath, html.ToString());
            Console.WriteLine($"\n📄 Reporte HTML generado: {Path.GetFullPath(reportPath)}");
        }

        private string GetCssStyles()
        {
            return @"
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f5f5f5;
        }

        .header {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 2rem;
            text-align: center;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

        .header h1 {
            font-size: 2.5rem;
            margin-bottom: 0.5rem;
        }

        .header h2 {
            font-size: 1.5rem;
            opacity: 0.9;
        }

        .summary {
            background: white;
            margin: 2rem;
            padding: 2rem;
            border-radius: 10px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }

        .summary h3 {
            color: #667eea;
            margin-bottom: 1rem;
            font-size: 1.5rem;
        }

        .summary-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 1rem;
            margin-bottom: 1rem;
        }

        .summary-item {
            background: #f8f9fa;
            padding: 1rem;
            border-radius: 8px;
            border-left: 4px solid #667eea;
        }

        .status-success {
            border-left-color: #28a745 !important;
            background-color: #d4edda !important;
        }

        .status-failure {
            border-left-color: #dc3545 !important;
            background-color: #f8d7da !important;
        }

        .error-summary {
            background: #f8d7da;
            border: 1px solid #f5c6cb;
            color: #721c24;
            padding: 1rem;
            border-radius: 8px;
            margin-top: 1rem;
        }

        .steps-container {
            margin: 2rem;
        }

        .steps-container h3 {
            color: #667eea;
            margin-bottom: 1rem;
            font-size: 1.5rem;
        }

        .step {
            background: white;
            margin-bottom: 1rem;
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            overflow: hidden;
        }

        .step-header {
            padding: 1rem;
            display: flex;
            align-items: center;
            gap: 1rem;
        }

        .step-header.status-success {
            background: #d4edda;
            border-left: 5px solid #28a745;
        }

        .step-header.status-failure {
            background: #f8d7da;
            border-left: 5px solid #dc3545;
        }

        .step-number {
            background: #667eea;
            color: white;
            width: 30px;
            height: 30px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            flex-shrink: 0;
        }

        .step-status {
            font-size: 1.2rem;
            font-weight: bold;
            flex-shrink: 0;
        }

        .step-description {
            flex-grow: 1;
            font-weight: 500;
        }

        .step-time {
            color: #666;
            font-size: 0.9rem;
            flex-shrink: 0;
        }

        .step-details {
            padding: 1rem;
            background: #f8f9fa;
            border-top: 1px solid #dee2e6;
        }

        .step-screenshot {
            padding: 1rem;
            text-align: center;
        }

        .step-screenshot img {
            max-width: 100%;
            height: auto;
            border: 2px solid #dee2e6;
            border-radius: 8px;
            cursor: pointer;
            transition: transform 0.3s ease;
        }

        .step-screenshot img:hover {
            transform: scale(1.02);
            box-shadow: 0 4px 15px rgba(0,0,0,0.2);
        }

        .footer {
            background: #343a40;
            color: white;
            text-align: center;
            padding: 2rem;
            margin-top: 2rem;
        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0,0,0,0.9);
        }

        .modal-content {
            margin: auto;
            display: block;
            width: 90%;
            max-width: 1200px;
            max-height: 90vh;
            object-fit: contain;
        }

        .close {
            position: absolute;
            top: 15px;
            right: 35px;
            color: #f1f1f1;
            font-size: 40px;
            font-weight: bold;
            cursor: pointer;
        }

        .close:hover {
            color: #bbb;
        }

        @media (max-width: 768px) {
            .header h1 {
                font-size: 2rem;
            }
            
            .summary {
                margin: 1rem;
                padding: 1rem;
            }
            
            .steps-container {
                margin: 1rem;
            }
            
            .summary-grid {
                grid-template-columns: 1fr;
            }
        }";
        }

        private string GetJavaScript()
        {
            return @"
        function openModal(src) {
            document.getElementById('imageModal').style.display = 'block';
            document.getElementById('modalImg').src = src;
        }

        function closeModal() {
            document.getElementById('imageModal').style.display = 'none';
        }

        document.addEventListener('keydown', function(event) {
            if (event.key === 'Escape') {
                closeModal();
            }
        });";
        }
    }

    public class TestStep
    {
        public int StepNumber { get; set; }
        public string Description { get; set; } = "";
        public bool Success { get; set; }
        public string Details { get; set; } = "";
        public DateTime Timestamp { get; set; }
        public string ScreenshotPath { get; set; } = "";
    }
}
