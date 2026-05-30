# API Inteligente de Tareas y Análisis

Una API RESTful robusta para gestionar tareas internas enriquecidas con información externa e inteligencia artificial usando ML.NET.

## 🎯 Descripción

Esta API proporciona funcionalidades completas para:
- **Gestión CRUD de Tareas**: Crear, leer, actualizar y eliminar tareas
- **Entidad Tarea**: Con propiedades como título, descripción, estado, prioridad y fechas
- **Validaciones robustas**: Garantiza integridad de datos
- **Persistencia en SQLite**: Base de datos ligera y portable
- **Documentación OpenAPI**: Exploración interactiva de endpoints

## 📋 Requisitos

- .NET 10.0 o superior
- SQLite 3.x
- Visual Studio Code o Visual Studio 2022 (opcional)

## 🚀 Instalación y Ejecución

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd ApiInteligenteDeTareasYAnalisis
```

### 2. Restaurar dependencias

```bash
dotnet restore
```

### 3. Ejecutar migraciones (si aún no se han aplicado)

```bash
dotnet ef database update
```

O durante la ejecución, se aplicarán automáticamente en modo desarrollo.

### 4. Ejecutar la aplicación

```bash
dotnet run
```

La API estará disponible en: `http://localhost:5157`

## 📚 Endpoints Implementados

Todos los endpoints se encuentran bajo el prefijo `/api/tareas`

### Obtener todas las tareas

**GET** `/api/tareas`

```bash
curl -X GET "http://localhost:5157/api/tareas"
```

**Respuesta exitosa (200):**
```json
[
  {
    "id": 1,
    "titulo": "Implementar autenticación",
    "descripcion": "Agregar JWT al proyecto",
    "estado": "EnProceso",
    "prioridad": "Alta",
    "fechaCreacion": "2026-05-30T01:23:45Z",
    "fechaVencimiento": "2026-06-15T00:00:00Z"
  }
]
```

---

### Obtener una tarea por ID

**GET** `/api/tareas/{id}`

```bash
curl -X GET "http://localhost:5157/api/tareas/1"
```

**Respuesta exitosa (200):**
```json
{
  "id": 1,
  "titulo": "Implementar autenticación",
  "descripcion": "Agregar JWT al proyecto",
  "estado": "EnProceso",
  "prioridad": "Alta",
  "fechaCreacion": "2026-05-30T01:23:45Z",
  "fechaVencimiento": "2026-06-15T00:00:00Z"
}
```

**Error (404):**
```json
{
  "mensaje": "La tarea con ID 999 no existe"
}
```

---

### Crear una nueva tarea

**POST** `/api/tareas`

```bash
curl -X POST "http://localhost:5157/api/tareas" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Escribir documentación",
    "descripcion": "Crear README y guía de uso",
    "estado": "Pendiente",
    "prioridad": "Media",
    "fechaVencimiento": "2026-06-30T00:00:00Z"
  }'
```

**Respuesta exitosa (201):**
```json
{
  "id": 2,
  "titulo": "Escribir documentación",
  "descripcion": "Crear README y guía de uso",
  "estado": "Pendiente",
  "prioridad": "Media",
  "fechaCreacion": "2026-05-30T01:25:00Z",
  "fechaVencimiento": "2026-06-30T00:00:00Z"
}
```

**Validación fallida (400):**
```json
{
  "errores": [
    "El título es obligatorio y no puede estar vacío",
    "La fecha de vencimiento no puede ser una fecha pasada"
  ]
}
```

---

### Actualizar una tarea

**PUT** `/api/tareas/{id}`

```bash
curl -X PUT "http://localhost:5157/api/tareas/1" \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Implementar autenticación con OAuth",
    "descripcion": "Agregar JWT y OAuth2",
    "estado": "EnProceso",
    "prioridad": "Alta",
    "fechaVencimiento": "2026-06-20T00:00:00Z"
  }'
```

**Respuesta exitosa (200):**
```json
{
  "id": 1,
  "titulo": "Implementar autenticación con OAuth",
  "descripcion": "Agregar JWT y OAuth2",
  "estado": "EnProceso",
  "prioridad": "Alta",
  "fechaCreacion": "2026-05-30T01:23:45Z",
  "fechaVencimiento": "2026-06-20T00:00:00Z"
}
```

---

### Eliminar una tarea

**DELETE** `/api/tareas/{id}`

```bash
curl -X DELETE "http://localhost:5157/api/tareas/1"
```

**Respuesta exitosa (200):**
```json
{
  "mensaje": "Tarea eliminada correctamente"
}
```

## ✅ Validaciones Implementadas

### Validación de Título
- ✅ Obligatorio
- ✅ No puede estar vacío o contener solo espacios
- ✅ Máximo 500 caracteres

### Validación de Prioridad
- ✅ Obligatoria
- ✅ Valores permitidos: `Baja`, `Media`, `Alta`

### Validación de Estado
- ✅ Obligatorio
- ✅ Valores permitidos: `Pendiente`, `EnProceso`, `Completada`

### Validación de Fecha de Vencimiento
- ✅ Obligatoria
- ✅ No puede ser una fecha pasada (menor a `DateTime.UtcNow.Date`)

## 🗄️ Estructura de la Base de Datos

### Tabla: Tareas

```sql
CREATE TABLE "Tareas" (
    "Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    "Titulo" TEXT NOT NULL,
    "Descripcion" TEXT NULL,
    "Estado" TEXT NOT NULL,
    "Prioridad" TEXT NOT NULL,
    "FechaCreacion" TEXT NOT NULL,
    "FechaVencimiento" TEXT NOT NULL
);
```

### Modelo de Entidad

```csharp
public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string Estado { get; set; }
    public string Prioridad { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaVencimiento { get; set; }
}
```

## 📁 Estructura del Proyecto

```
ApiInteligenteDeTareasYAnalisis/
├── Controllers/
│   └── TareasController.cs          # Endpoints de tareas
├── Models/
│   └── Tarea.cs                     # Entidad Tarea
├── Data/
│   ├── AppDbContext.cs              # DbContext de EF Core
│   └── AppDbContextFactory.cs       # Factory para diseño-tiempo
├── Program.cs                        # Configuración de la aplicación
├── appsettings.json                 # Configuración
├── appsettings.Development.json     # Configuración de desarrollo
├── ApiInteligenteDeTareasYAnalisis.csproj  # Proyecto
└── tareas.db                        # Base de datos SQLite
```

## 🔧 Tecnologías Utilizadas

- **ASP.NET Core 10.0**: Framework web
- **Entity Framework Core 10.0.8**: ORM
- **SQLite**: Base de datos
- **OpenAPI**: Documentación de API
- **DataAnnotations**: Validaciones

## 📝 Migraciones de Base de Datos

### Crear una nueva migración

```bash
dotnet ef migrations add <NombreMigracion>
```

Ejemplo:
```bash
dotnet ef migrations add AgregarCampoNuevo
```

### Aplicar migraciones

```bash
dotnet ef database update
```

### Revertir a una migración anterior

```bash
dotnet ef database update <NombreMigracionAnterior>
```

## 🐛 Troubleshooting

### Error: "Database is locked"

Asegúrate de que no hay otra instancia de la aplicación usando la base de datos.

### Error al crear migraciones

Verifica que el `AppDbContext` esté correctamente configurado en `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
```

### Puerto en uso

Si el puerto 5157 está en uso, modifica en `Properties/launchSettings.json`:

```json
"applicationUrl": "http://localhost:PUERTO"
```

## 📖 Documentación Adicional

### Explorar la API con OpenAPI

Durante el desarrollo, accede a:
- `http://localhost:5157/openapi/v1.json`

### Comandos útiles

```bash
# Ver todas las migraciones
dotnet ef migrations list

# Ver modelo de datos
dotnet ef dbcontext scaffold

# Limpiar base de datos
dotnet ef database drop
```

## 🎓 Próximos Pasos (Futuro)

- [ ] Integración de ML.NET para clasificación de comentarios
- [ ] Consumo de API externa
- [ ] Sistema de autenticación JWT
- [ ] Búsqueda y filtrado avanzado
- [ ] Caché distribuido
- [ ] Logging persistente

## 📄 Licencia

Este proyecto es parte de una evaluación académica.

## 👤 Autor

Santiago - Universidad

---

**Última actualización**: 29 de mayo de 2026
