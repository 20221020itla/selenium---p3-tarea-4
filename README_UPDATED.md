# Selenium Login y CRUD Automation con C#

## 📋 Descripción
Este proyecto utiliza **Selenium WebDriver** con **C#** para automatizar:
1. **Login** en una aplicación web
2. **Operaciones CRUD** completas para gestión de productos (Crear, Leer, Actualizar, Eliminar)
3. **Generación de reportes HTML** profesionales con capturas de pantalla

## ✨ Características Principales

### 🔐 Automatización de Login
- Soporte para múltiples selectores CSS
- Manejo robusto de errores
- Verificación automática de login exitoso
- Compatible con diferentes tipos de formularios

### 📦 Automatización CRUD de Productos
- **Navegación**: Acceso automático a la sección de productos
- **Crear Producto**: Formulario completo con validación
- **Editar Producto**: Modificación de datos existentes
- **Eliminar Producto**: Eliminación con confirmación
- **Validación**: Verificación de cada operación

### 📊 Sistema de Reportes Avanzado
- **Reportes HTML**: Diseño profesional y responsivo
- **Capturas de Pantalla**: Automáticas en cada paso
- **Modales de Imágenes**: Visualización ampliada de screenshots
- **CSS Moderno**: Interfaz atractiva con gradientes y animaciones
- **JavaScript Integrado**: Funcionalidades interactivas

## 🚀 Configuración y Uso

### Prerrequisitos
- .NET 6.0 o superior
- Google Chrome instalado
- Conexión a internet para descargar ChromeDriver

### Instalación
1. Clona o descarga este proyecto
2. Abre una terminal en la carpeta del proyecto
3. Ejecuta: `dotnet restore`
4. Compila: `dotnet build`

### Ejecución
```bash
dotnet run
```

### Configuración Personalizada
Edita el archivo `Config.cs` para personalizar:

#### Configuración Básica
```csharp
public const string LOGIN_URL = "http://localhost:5173/";
public const string USERNAME = "tu_usuario@email.com";
public const string PASSWORD = "tu_contraseña";
```

#### Datos de Prueba para CRUD
```csharp
// Producto a crear
public const string TEST_PRODUCT_NAME = "Producto Test Selenium";
public const string TEST_PRODUCT_DESCRIPTION = "Producto creado automáticamente";
public const string TEST_PRODUCT_PRICE = "99.99";
public const string TEST_PRODUCT_CATEGORY = "Electrónicos";
public const string TEST_PRODUCT_STOCK = "50";

// Datos para edición
public const string EDITED_PRODUCT_NAME = "Producto Test Editado";
public const string EDITED_PRODUCT_DESCRIPTION = "Producto editado automáticamente";
public const string EDITED_PRODUCT_PRICE = "149.99";
public const string EDITED_PRODUCT_CATEGORY = "Tecnología";
public const string EDITED_PRODUCT_STOCK = "75";
```

#### Timeouts y Configuración
```csharp
public const int IMPLICIT_WAIT_TIMEOUT = 10;    // segundos
public const int EXPLICIT_WAIT_TIMEOUT = 10;    // segundos
public const int VERIFICATION_DELAY = 2000;     // milisegundos
```

## 🛠️ Estructura del Proyecto

### Archivos Principales
- **`Program.cs`**: Lógica principal de automatización
- **`Config.cs`**: Configuración centralizada
- **`TestReport.cs`**: Sistema de reportes y capturas
- **`SeleniumLoginAutomation.csproj`**: Configuración del proyecto

### Flujo de Ejecución
1. **Inicialización**: Configuración del driver Chrome
2. **Login**: Autenticación en la aplicación
3. **Navegación**: Acceso a la sección de productos
4. **CRUD Operations**:
   - Crear nuevo producto
   - Editar producto creado
   - Eliminar producto
5. **Reporte**: Generación de documentación HTML

## 📁 Estructura de Reportes

```
TestReports/
├── TestReport_YYYYMMDD_HHMMSS.html    # Reporte principal
└── Screenshots/
    ├── inicio_YYYYMMDD_HHMMSS.png
    ├── login_success_YYYYMMDD_HHMMSS.png
    ├── productos_nav_YYYYMMDD_HHMMSS.png
    ├── crear_producto_YYYYMMDD_HHMMSS.png
    ├── editar_producto_YYYYMMDD_HHMMSS.png
    └── eliminar_producto_YYYYMMDD_HHMMSS.png
```

## 🎯 Selectores Soportados

### Login
- Campos de email/usuario con múltiples variaciones
- Campos de contraseña con diferentes nombres
- Botones de submit con diversos formatos

### Productos
- Pestañas de navegación con iconos y texto
- Botones de acción (Crear, Editar, Eliminar)
- Formularios con validación automática
- Modales de confirmación

## 🔧 Personalización Avanzada

### Agregar Nuevos Selectores
```csharp
public static readonly string[] MI_NUEVO_SELECTOR = 
{
    "selector1",
    "selector2",
    "selector3"
};
```

### Modificar Operaciones CRUD
Las operaciones CRUD están modularizadas en métodos independientes:
- `NavigateToProducts()`: Navegación a productos
- `CreateProduct()`: Creación de productos
- `EditProduct()`: Edición de productos
- `DeleteProduct()`: Eliminación de productos

## 📝 Logs y Debugging

### Console Output
El proyecto muestra progreso en tiempo real:
```
Iniciando el navegador Chrome...
Navegando a la página de login...
✓ Login verificado como exitoso!
Navegando a la sección de productos...
✓ Producto 'Producto Test Selenium' creado exitosamente
✓ Producto editado exitosamente
✓ Producto eliminado exitosamente
🎉 Login y operaciones CRUD exitosas!
```

### Reportes HTML
- Pasos detallados con timestamps
- Screenshots automáticos
- Estados de éxito/error claramente marcados
- Diseño responsivo para móviles y desktop

## 🚨 Manejo de Errores

### Errores Comunes y Soluciones
1. **ChromeDriver no encontrado**: Se descarga automáticamente
2. **Elementos no encontrados**: Múltiples selectores de respaldo
3. **Timeouts**: Configurables en `Config.cs`
4. **Credenciales inválidas**: Verificación automática con mensajes claros

### Recuperación Automática
- Reintentos con diferentes selectores
- Capturas de pantalla en caso de error
- Logs detallados para debugging

## 🏗️ Arquitectura

### Patrón de Diseño
- **Configuración centralizada**: Todos los parámetros en `Config.cs`
- **Separación de responsabilidades**: Login, CRUD y Reportes independientes
- **Manejo robusto de errores**: Try-catch en cada operación
- **Logging comprensivo**: Registro detallado de cada acción

### Tecnologías Utilizadas
- **Selenium WebDriver 4.15.0**: Automatización web
- **ChromeDriver**: Driver específico para Chrome
- **Selenium Support**: Funciones adicionales como `SelectElement`
- **.NET 6.0**: Framework de desarrollo
- **HTML5 + CSS3 + JavaScript**: Reportes interactivos

## 🤝 Contribuciones
Para contribuir al proyecto:
1. Fork del repositorio
2. Crear rama feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crear Pull Request

## 📄 Licencia
Este proyecto está bajo la Licencia MIT. Ver archivo `LICENSE` para más detalles.

---
**Desarrollado con ❤️ usando Selenium WebDriver y C#**
