# Transacciones API

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512bd4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)

API para el manejo de transacciones financieras, construida con .NET 8 siguiendo principios SOLID y Clean Architecture.

---

## Tecnologías Principales

*   **Runtime:** .NET 8 SDK
*   **Base de Datos:** PostgreSQL (Hospedado en [NEON](https://neon.tech/))
*   **Arquitectura:** Clean Architecture + UseCase Pattern
*   **Endpoints:** [Ardalis.Endpoints](https://github.com/ardalis/ApiEndpoints) (REPR Pattern: Request-Endpoint-Response)
*   **Autenticación:** API Key simple

---

## Configuración del Proyecto

### Requisitos Previos

1.  **.NET SDK 8.0** instalado.
2.  Archivo `appsettings.json` (se compartirá por correo).

### Pasos para iniciar

1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com/DruGuerrero/transaccionesapi.git
    cd transaccionesapi
    ```
2.  **Configurar secretos:**
    Colocar el archivo `appsettings.json` en la raíz del proyecto `Transacciones.API`.
3.  **Restaurar y Compilar:**
    ```bash
    dotnet build
    ```

---

## Comandos Útiles

| Acción | Comando |
| :--- | :--- |
| **Ejecutar API** | `dotnet run --project Transacciones.API` |
| **Correr Tests** | `dotnet test` |
| **Nueva Migración** | `dotnet ef migrations add <Nombre> --project Transacciones.Infrastructure --startup-project Transacciones.API` |
| **Actualizar DB** | `dotnet ef database update --project Transacciones.Infrastructure --startup-project Transacciones.API` |

---

## Decisiones Técnicas

*   **NEON (Postgres):** Elegido por su capacidad de escalado serverless y rapidez en la configuración inicial.
*   **Ardalis Endpoints:** Usando el patrón **REPR** para evitar controladores "gordos" y cumplir estrictamente con el Principio de Responsabilidad Única (SRP).
*   **UseCase Pattern:** Desacoplamiento de la lógica de negocio de la infraestructura y de la capa de API, facilitando el mantenimiento y las pruebas unitarias.
*   **API Key Auth:** Implementado como un middleware ligero para asegurar los endpoints de manera rápida.

---

## Endpoints

### Cuentas
*   `POST /api/cuentas` - Crear una nueva cuenta financiera.
*   `GET /api/cuentas/{id}` - Obtener los detalles y balance de una cuenta específica.

### Transacciones
*   `GET /api/transacciones/cuenta/{id}` - Consultar el historial de movimientos de una cuenta.
*   `POST /api/transacciones/deposito` - Registrar un ingreso de fondos.
*   `POST /api/transacciones/retiro` - Registrar un egreso de fondos.


> [!NOTE]
> La documentación interactiva está disponible vía Swagger al ejecutar el proyecto en modo desarrollo.


