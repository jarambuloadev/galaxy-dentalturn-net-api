# DentalTurn — Sistema de Gestión de Turnos Odontológicos

Aplicación web para la gestión de turnos de un consultorio odontológico. El sistema permite a los pacientes solicitar turnos seleccionando servicio y odontólogo, mientras que el usuario administrador gestiona pacientes, servicios, odontólogos y turnos desde un entorno protegido.

El proyecto utiliza una arquitectura separada **Frontend + Backend API**, con **Angular 21** para la interfaz y **ASP.NET Core .NET 10 Minimal APIs** para el backend.

---

## Stack tecnológico

### Frontend

- **Angular 21**
- **TypeScript**
- **Bootstrap 5**
- Angular Router
- Reactive Forms
- `HttpClient` para consumo de API REST
- Interceptores HTTP para JWT
- Guards para protección de rutas

### Backend

- **ASP.NET Core .NET 10**
- **Minimal APIs**
- **Entity Framework Core**
- C#
- Inyección de dependencias
- JWT Bearer Authentication
- Autorización mediante roles/claims
- CORS
- Swagger / OpenAPI
- Gestión de transacciones

### Base de datos

- **PostgreSQL**
- Entity Framework Core como ORM
- Migraciones de EF Core

---

## Arquitectura de la solución

La solución aplica separación de responsabilidades entre presentación, API, lógica de negocio y persistencia.

```text
┌──────────────────────────────────────────────┐
│              PRESENTACIÓN                    │
│                                              │
│              Angular 21                     │
│          Bootstrap 5 / TypeScript            │
└──────────────────────┬───────────────────────┘
                       │ HTTPS / JSON / JWT
                       ▼
┌──────────────────────────────────────────────┐
│                    API                       │
│                                              │
│       ASP.NET Core .NET 10 Minimal APIs      │
│       Swagger / OpenAPI / CORS / JWT         │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│              CAPA DE NEGOCIO                 │
│                                              │
│      Reglas de disponibilidad y turnos       │
│      Validaciones y procesos de negocio     │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│                PERSISTENCIA                  │
│                                              │
│             Entity Framework Core            │
│                 DbContext                    │
└──────────────────────┬───────────────────────┘
                       │
                       ▼
┌──────────────────────────────────────────────┐
│                 PostgreSQL                   │
└──────────────────────────────────────────────┘
```

### Principios aplicados

- Separación de responsabilidades.
- Inyección de dependencias.
- Bajo acoplamiento entre capas.
- Reglas de negocio centralizadas en Backend.
- Persistencia mediante Entity Framework Core.
- Autenticación y autorización mediante JWT.
- Documentación mediante Swagger/OpenAPI.
- CORS configurado para la comunicación con Angular.
- Transacciones para operaciones que requieren integridad y consistencia.

---

## Caso de negocio

DentalTurn permite administrar el proceso de reserva y gestión de turnos odontológicos.

### Flujo principal

1. El paciente selecciona un servicio.
2. El sistema consulta los odontólogos que ofrecen dicho servicio.
3. Se verifica la disponibilidad del odontólogo.
4. El paciente selecciona fecha y hora.
5. El Backend valida las reglas de negocio.
6. Se registra el turno relacionando paciente, servicio y odontólogo.
7. La operación garantiza la consistencia de los datos.
8. El administrador puede consultar y gestionar los turnos.

Este proceso involucra múltiples entidades y reglas de negocio, superando las operaciones CRUD básicas.

---

## Modelo de datos

Entidades principales:

- `Patient`
- `Service`
- `Practitioner`
- `PractitionerService`
- `Appointment`
- `BusinessHours`
- `AdminUser`

### Relaciones

```text
Patient
   │
   │ 1:N
   ▼
Appointment
   │
   ├──────────────► Service
   │
   └──────────────► Practitioner
                         │
                         │ N:M
                         ▼
                  PractitionerService
                         │
                         ▼
                       Service
```

### Reglas principales

- `PractitionerService` es la tabla puente de la relación muchos-a-muchos entre odontólogos y servicios.
- `Appointment` referencia paciente, servicio y odontólogo.
- Un odontólogo no puede tener dos turnos con el mismo instante de inicio.
- No se permite eliminar entidades con turnos asociados cuando la integridad referencial lo impide.
- El estado del turno utiliza:
  - `PENDING`
  - `CONFIRMED`
  - `CANCELLED`
  - `COMPLETED`

---

## Seguridad

La API utiliza **JWT Bearer Authentication**.

```text
Angular
   │
   │ POST /api/auth/login
   ▼
.NET 10 Minimal API
   │
   │ valida credenciales
   ▼
JWT
   │
   │ Authorization: Bearer <token>
   ▼
Endpoints protegidos
```

Se consideran:

- Autenticación mediante JWT.
- Autorización mediante roles/claims.
- Contraseñas almacenadas mediante hash.
- Guards para rutas administrativas en Angular.
- Validación de autorización en Backend.
- CORS configurado entre Frontend y API.

No publicar secretos, contraseñas ni tokens reales en el repositorio.

---

## Proceso transaccional

El registro de un turno requiere validar información relacionada con varias entidades.

```text
Inicio de transacción
        │
        ▼
Validar paciente
        │
        ▼
Validar servicio
        │
        ▼
Validar odontólogo
        │
        ▼
Validar relación odontólogo-servicio
        │
        ▼
Validar disponibilidad
        │
        ▼
Crear Appointment
        │
        ▼
Guardar cambios
        │
        ▼
Commit
```

Si una validación o persistencia falla, la operación debe revertirse para preservar la integridad de los datos.

---

## Requisitos

- **Node.js** compatible con Angular 21.
- **Angular CLI 21**.
- **.NET SDK 10**.
- **PostgreSQL 16** o compatible.
- Git.
- Navegador web moderno.

Verificar:

```bash
node --version
npm --version
ng version
dotnet --version
psql --version
```

---

# Ejecución

## 1. Clonar el repositorio

```bash
git clone <URL_DEL_REPOSITORIO>
cd DentalTurn
```

## 2. Configurar PostgreSQL

Crear la base de datos:

```sql
CREATE DATABASE dentalturndb;
```

Configurar usuario y permisos según el entorno.

> No subir credenciales reales al repositorio.

## 3. Configurar Backend

Ingresar al proyecto:

```bash
cd backend
```

Configurar la cadena de conexión en `appsettings.Development.json` o mediante variables de entorno.

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dentalturndb;Username=postgres;Password=TU_PASSWORD"
  }
}
```

Configurar JWT:

```json
{
  "Jwt": {
    "Key": "CLAVE_SECRETA_DE_DESARROLLO",
    "Issuer": "DentalTurn",
    "Audience": "DentalTurnFrontend",
    "ExpirationMinutes": 60
  }
}
```

Restaurar dependencias:

```bash
dotnet restore
```

Aplicar migraciones:

```bash
dotnet ef database update
```

Ejecutar:

```bash
dotnet run
```

## 4. Swagger / OpenAPI

Con la API ejecutándose, acceder a:

```text
/swagger
```

Swagger permite consultar, documentar y probar los endpoints implementados.

## 5. Configurar Angular

Desde el Frontend:

```bash
cd frontend
npm install
```

Configurar la URL de la API en el archivo de entorno correspondiente:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:xxxx/api'
};
```

## 6. Ejecutar Angular

```bash
ng serve
```

Por defecto:

```text
http://localhost:4200
```

---

# Módulos funcionales

## Público

- Consulta de servicios.
- Consulta de odontólogos.
- Consulta de horarios disponibles.
- Registro de paciente.
- Reserva de turno.

## Administrativo

Protegido mediante JWT:

- Login.
- Gestión de pacientes.
- Gestión de servicios.
- Gestión de odontólogos.
- Asociación odontólogo-servicio.
- Consulta y gestión de turnos.
- Cambio de estado de turnos.
- Gestión de horarios de atención.

---

# Endpoints principales

> Las rutas definitivas deben mantenerse sincronizadas con Swagger/OpenAPI.

### Autenticación

```text
POST /api/auth/login
```

### Pacientes

```text
GET    /api/patients
GET    /api/patients/{id}
POST   /api/patients
PUT    /api/patients/{id}
DELETE /api/patients/{id}
```

### Servicios

```text
GET    /api/services
GET    /api/services/{id}
POST   /api/services
PUT    /api/services/{id}
DELETE /api/services/{id}
```

### Odontólogos

```text
GET    /api/practitioners
GET    /api/practitioners/{id}
POST   /api/practitioners
PUT    /api/practitioners/{id}
DELETE /api/practitioners/{id}
```

### Turnos

```text
GET    /api/appointments
GET    /api/appointments/{id}
POST   /api/appointments
PUT    /api/appointments/{id}
DELETE /api/appointments/{id}
```

### Proceso de negocio

La creación de un turno debe validar:

- Existencia del paciente.
- Existencia del servicio.
- Existencia del odontólogo.
- Relación válida odontólogo-servicio.
- Disponibilidad del horario.
- Ausencia de conflicto.
- Persistencia consistente mediante transacción.

---

# Estructura propuesta

```text
DentalTurn/
│
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/
│   │   │   ├── shared/
│   │   │   ├── features/
│   │   │   └── ...
│   │   ├── environments/
│   │   └── ...
│   ├── angular.json
│   └── package.json
│
├── backend/
│   ├── DentalTurn.Api/
│   │   ├── Endpoints/
│   │   ├── Extensions/
│   │   └── Program.cs
│   │
│   ├── DentalTurn.Application/
│   │   ├── DTOs/
│   │   ├── Services/
│   │   ├── Interfaces/
│   │   └── Validators/
│   │
│   ├── DentalTurn.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   └── Interfaces/
│   │
│   └── DentalTurn.Infrastructure/
│       ├── Data/
│       ├── Repositories/
│       ├── Configurations/
│       └── Migrations/
│
├── README.md
└── .gitignore
```

La estructura definitiva puede variar según la implementación, manteniendo la separación de responsabilidades.

---

# Pruebas

## CRUD

- Crear, consultar, actualizar y eliminar pacientes.
- Crear, consultar, actualizar y eliminar servicios.
- Crear, consultar, actualizar y eliminar odontólogos.

## Seguridad

- Login correcto.
- Credenciales incorrectas.
- Acceso sin JWT.
- Acceso con JWT válido.
- Validación de roles/claims.

## Proceso de negocio

- Crear turno válido.
- Intentar reservar horario ocupado.
- Intentar reservar servicio no ofrecido por el odontólogo.
- Validar disponibilidad.
- Verificar rollback ante errores transaccionales.

## Documentación

Las APIs deben probarse mediante Swagger/OpenAPI y/o Postman.

---

# Despliegue

La solución está preparada para separar el despliegue del Frontend, Backend y base de datos:

```text
                    Internet
                       │
          ┌────────────┴────────────┐
          │                         │
          ▼                         ▼
     Angular 21                .NET 10 API
     Frontend                   Backend
          │                         │
          │       HTTPS / JSON      │
          └────────────►────────────┘
                                    │
                                    ▼
                               PostgreSQL
```

En producción se deben configurar:

- URL de la API.
- Variables de entorno.
- CORS.
- Cadena de conexión.
- JWT secret.
- HTTPS.
- Configuración específica del proveedor de despliegue.

---

# Cumplimiento de la rúbrica

| Criterio | Implementación |
|---|---|
| Arquitectura y estructura | Angular 21 + .NET 10 con separación de responsabilidades |
| Minimal APIs y proceso de negocio | .NET 10 Minimal APIs + gestión de turnos |
| Persistencia | Entity Framework Core + PostgreSQL |
| Seguridad | JWT + autorización + CORS |
| Transacciones | Registro de turno con validaciones y persistencia transaccional |
| Documentación y pruebas | Swagger/OpenAPI + pruebas funcionales |
| Despliegue | Frontend y Backend preparados para despliegue independiente |

---

# Control de versiones

El proyecto se gestiona mediante Git.

Ejemplos de commits:

```text
feat: crear estructura inicial Angular 21
feat: implementar Minimal APIs de pacientes
feat: agregar persistencia con EF Core
feat: implementar autenticación JWT
feat: implementar proceso de reserva de turnos
feat: agregar validación de disponibilidad
feat: configurar Swagger y CORS
fix: corregir validación de horarios
docs: actualizar README
```

No versionar:

```text
.env
appsettings.*.local.json
bin/
obj/
node_modules/
dist/
```

---

# Objetivo académico

DentalTurn se desarrolla como proyecto final para demostrar el dominio de:

- .NET 10 Minimal APIs.
- Diseño de APIs REST.
- Separación de responsabilidades.
- Inyección de dependencias.
- Entity Framework Core.
- PostgreSQL.
- JWT.
- CORS.
- Gestión transaccional.
- Swagger/OpenAPI.
- Angular 21.
- Bootstrap 5.
- Integración Frontend ↔ Backend mediante HTTP/REST.
- Control de versiones con Git.

La incorporación de Angular 21 permite separar el Frontend del Backend, mientras que el cumplimiento de los requisitos principales del proyecto se mantiene en la API desarrollada con .NET 10 Minimal APIs.
