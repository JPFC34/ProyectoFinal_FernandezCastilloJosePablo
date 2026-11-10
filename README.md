# Sistema de Matrícula Universitaria - UHCR

Proyecto final de Programación Avanzada (Universidad Fidélitas). Sistema web de matrícula para la **Universidad Horizonte de Costa Rica (UHCR)**, una universidad ficticia, desarrollado con **ASP.NET Core MVC 8.0**.

**Autor:** José Pablo Fernández Castillo

## Tecnologías

- ASP.NET Core MVC 8.0 (.NET 8)
- Entity Framework Core 8.0
- SQL Server (LocalDB o SQL Server 2022)
- ASP.NET Core Identity (roles Administrador y Estudiante)
- Bootstrap 5 y vistas Razor, todo en español

## Funcionalidades del Avance 2

**Estudiante**
- Registro con selección de carrera (E-01, E-03)
- Inicio de sesión (E-02)
- Catálogo de cursos de su carrera, con créditos, docente, horario y cupos (E-04)

**Administrador**
- Inicio de sesión con acceso protegido por rol (A-01)
- Panel principal con indicadores (A-02)
- CRUD de carreras, cursos y docentes, con asignación de docente a cada curso (A-04, A-05, A-06, A-07)
- Búsqueda y filtros (A-09)

## Base de datos

Tablas principales: Estudiantes, Carreras, Cursos, Docentes y Matrículas, más las tablas de ASP.NET Core Identity. Al iniciar, la aplicación aplica las migraciones y carga datos de ejemplo (carreras, docentes y cursos).

## Cómo ejecutar

1. Instalar Visual Studio con el SDK de .NET 8 y SQL Server LocalDB (viene con Visual Studio).
2. Clonar el repositorio y abrir el archivo `.slnx` en Visual Studio.
3. Revisar la cadena de conexión en `appsettings.json`:
   ```json
   "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MatriculaUHCR;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   ```
4. Pulsar **F5**. La base de datos se crea sola con las migraciones.

## Usuario administrador de prueba

- Correo: `admin@uhcr.ac.cr`
- Contraseña: `Admin123!`

Los estudiantes se registran desde la opción **Registrarse**.

## Flujo de trabajo en GitHub

El desarrollo se hizo con ramas y Pull Requests:
- PR #1: Identity, base de datos y registro de estudiantes
- PR #2: CRUD de carreras, cursos y docentes
- PR #3: README del proyecto
