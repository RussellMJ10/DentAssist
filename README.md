# 🦷 DentAssist – Sistema de Gestión Clínica para "Sonrisa Plena"

DentAssist es una aplicación web desarrollada para digitalizar la gestión de la clínica dental "Sonrisa Plena", ubicada en una ciudad de tamaño medio. Este sistema permite gestionar de forma eficiente pacientes, turnos, tratamientos y planes clínicos personalizados, con acceso diferenciado por roles.

## 📌 Descripción General

La clínica enfrentaba problemas derivados del manejo manual de información: turnos mal registrados, historiales incompletos y dificultad para generar reportes. DentAssist fue creado con el objetivo de centralizar y optimizar los procesos administrativos y clínicos mediante una interfaz amigable y moderna.

## 🧰 Tecnologías Utilizadas

- **ASP.NET Core MVC (.NET 8)**
- **Entity Framework Core** (con migraciones)
- **SQL Server** como base de datos relacional
- **Razor Pages + Bootstrap 5** para la interfaz responsiva
- **ASP.NET Identity** para autenticación y autorización
- **Git + GitHub** como control de versiones

## 🗂️ Estructura del Proyecto

```
DentAssistProject/
├── Connected Services/
├── Dependencies/
├── Properties/
├── wwwroot/
├── Controllers/
├── Migrations/
├── Models/
│ ├── Attributes/
│ ├── Data/
│ ├── Entities/
│ ├── Enums/
│ ├── ErrorViewModels.cs
│ ├── LoginViewModels.cs
│ ├── RegisterViewModels.cs
├── Views/
│ ├── Account/
│ ├── Home/
│ ├── Odontologos/
│ ├── Pacientes/
│ ├── PasoPlanes/
│ ├── PlanTratamientos/
│ ├── Shared/
│ ├── Tratamientos/
│ ├── Turnos/
│ ├── _ViewImports.cshtml
│ ├── _ViewStart.cshtml
├── appsettings.json
├── Program.cs
└── ScaffoldingReadMe.txt
```


### Descripción de carpetas principales:

- **Controllers**: Contiene los controladores MVC
- **Models**: 
  - `Entities/`: Modelos de entidades de la base de datos
  - `ViewModels/`: Modelos para vistas (login, registro, etc.)
- **Views**: Vistas organizadas por funcionalidad
- **wwwroot**: Archivos estáticos (CSS, JS, imágenes)
- **Migrations**: Migraciones de Entity Framework

## 🚀 Instalación y Ejecución

1. Clona el repositorio:
```bash
git clone https://github.com/RussellMJ10/DentAssist.git
cd dentassist
```

2. Configura la conexión a base de datos en `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DentAssistDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

3. Aplica las migraciones de la base de datos:
```bash
Update-Database
```

4. Ejecuta la aplicación desde visualStudio

Accede a la aplicación desde tu navegador en: `https://localhost:7176`

## 👥 Roles y Permisos

| Rol           | Funcionalidades Principales |
|---------------|-----------------------------|
| **Administrador** | ABM de odontólogos, tratamientos, configuración general |
| **Recepcionista** | ABM de pacientes, asignación de turnos, visualización de agenda |
| **Odontólogo**    | Consulta de su agenda, historial clínico de pacientes, creación de planes de tratamiento |

## ✅ Funcionalidades Implementadas

### Pacientes
- Alta, baja y modificación de pacientes
- Historial de tratamientos
- Solicitud de turnos

### Turnos
- Registro de turnos con fecha, hora y duración
- Asociación con pacientes y odontólogos
- Estados: Pendiente / Confirmado / Realizado / Cancelado
- Visualización de agenda semanal por profesional

### Odontólogos
- Registro y edición de odontólogos
- Visualización de su agenda
- Acceso a fichas de pacientes

### Tratamientos
- ABM de tratamientos ofrecidos
- Asociación con planes clínicos y registro en historial

### Planes de Tratamiento
- Creación de planes personalizados por paciente
- Secuencia de pasos, fechas, estados y observaciones
- Seguimiento visual del avance
- Exportación en PDF para el paciente

## 💻 Validaciones y Seguridad

- Validaciones en cliente y servidor
- Manejo de errores y feedback visual
- Restricción de vistas según el rol autenticado
- Autenticación mediante ASP.NET Identity

## 📄 Manual de Usuario

- Los **administradores** gestionan los odontólogos, tratamientos y parámetros generales desde el panel de configuración.
- Los **recepcionistas** crean y editan pacientes, asignan turnos desde una interfaz simplificada y acceden a la agenda semanal.
- Los **odontólogos** acceden a su agenda, historial clínico y pueden crear planes detallados para cada paciente, incluyendo exportación PDF.

## 📁 Migraciones y Base de Datos

El proyecto utiliza EF Core con migraciones. Para generar una nueva migración:

```bash
Add-Migration NombreMigracion
Update-Database
```

## 📖 Créditos

Este proyecto fue desarrollado por el equipo de estudiantes de Ingeniería Informática para la asignatura de Desarrollo de Aplicaciones Web en el año 2025.

## 📚 Licencia

Este proyecto es de uso educativo.

---

**DentAssist** – Transformando la gestión clínica con tecnología confiable y accesible.
