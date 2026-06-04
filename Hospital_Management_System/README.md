# 🏥 Hospital Appointment & Patient Management System

> A backend REST API system built with **C#**, **ASP.NET Core Web API**, **ADO.NET**, and **SQL Server** — designed for City General Hospital to manage patients, doctors, and appointments efficiently.

---

## 📌 Table of Contents

- [Project Overview](#project-overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Database Design](#database-design)
- [Stored Procedures](#stored-procedures)
- [REST API Endpoints](#rest-api-endpoints)
- [Project / Execution Flow](#project--execution-flow)
- [HTTP Status Codes](#http-status-codes)
- [Technical Highlights](#technical-highlights)
- [Deliverables](#deliverables)
- [Setup & Running Locally](#setup--running-locally)

---

## Project Overview

The **Hospital Appointment & Patient Management System** is a mini project backend API that handles three core domains:

**Patient Management** — Register patients with unique codes, date of birth, gender, phone, and email. Supports full record updates, soft deactivation (no hard deletes), active listing, and live age calculation.

**Doctor Management** — Maintain doctor profiles with specialization, consultation fee, and availability status. Supports filtering by specialization and availability.

**Appointment Booking** — Book appointments between patients and doctors with date/time. Tracks status lifecycle: `Scheduled → Completed / Cancelled`. Cancelled appointments record a cancellation timestamp.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Language | C# / .NET |
| API Framework | ASP.NET Core Web API |
| Data Access | ADO.NET |
| Database | SQL Server |
| Stored Procedures | 14 SPs |
| API Testing | Postman / Swagger |

---

## Architecture

The system follows a clean **layered architecture**:

```
Presentation Layer
  └── Postman · API Clients · Swagger

API Layer
  └── ASP.NET Core Web API · Controllers · Middleware · Global Logger

Domain Layer
  └── Models · Interfaces · Typed Exceptions · Age/Status/Format Logic

Data Access Layer
  └── ADO.NET · Repository Pattern · Interfaces · Stored Procedures

Database
  └── SQL Server · Transactions · Indexes · Referential Integrity
```

**Key design choices:**
- Repository interfaces are injected into controllers via Dependency Injection — no tight coupling.
- Domain logic (age calculation, status checks, schedule formatting) lives in the domain model — not in the DB or API layer.
- A global middleware logger captures method, path, and response time without touching individual controllers.

---

## Database Design

Three core tables with enforced referential integrity:

### `Patients`
| Column | Notes |
|--------|-------|
| `PatientId` 🔑 | Primary Key |
| `PatientCode` | UNIQUE |
| `FullName` | |
| `DateOfBirth` | |
| `Gender` | |
| `Phone` | UNIQUE |
| `Email` | UNIQUE, nullable |
| `IsActive` | Soft-delete flag |

### `Doctors`
| Column | Notes |
|--------|-------|
| `DoctorId` 🔑 | Primary Key |
| `DoctorCode` | UNIQUE |
| `FullName` | |
| `Specialization` | |
| `Phone` | UNIQUE |
| `ConsultationFee` | |
| `IsAvailable` | Toggle flag |

### `Appointments`
| Column | Notes |
|--------|-------|
| `AppointmentId` 🔑 | Primary Key |
| `PatientId` | FK → Patients |
| `DoctorId` | FK → Doctors |
| `AppointmentDateTime` | |
| `Status` | ENUM: Scheduled / Completed / Cancelled |
| `CancelledAt` | Nullable, set on cancellation |

**Database rules:**
- All operations go through Stored Procedures.
- Appointment booking is fully transactional.
- Soft-delete only — `IsActive` flag, never `DELETE`.
- Indexes on Doctor + Date for query performance.

---

## Stored Procedures

The system uses **14 Stored Procedures** grouped by domain:

### Patient SPs
- `sp_AddPatient` — Register a new patient
- `sp_UpdatePatient` — Update patient details
- `sp_DeactivatePatient` — Soft-deactivate a patient
- `sp_GetActivePatients` — List all active patients with calculated age

### Doctor SPs
- `sp_GetDoctorsByFilter` — Filter doctors by specialization and/or availability

### Appointment SPs
- `sp_AddAppointment` — Book an appointment (transactional; rejects unavailable doctors)
- `sp_CancelAppointment` — Cancel a scheduled appointment with timestamp
- `sp_GetUpcomingAppointments` — Retrieve all upcoming appointments
- `sp_GetAppointmentsByDoctor` — Get appointments for a specific doctor

### Reporting SPs
- `sp_GetAppointmentDetails` — Consolidated appointment report view
- `sp_GetDoctorsWithMoreAppointments` — Doctors with more than 2 appointments
- `sp_GetRevenueBySpecialization` — Revenue breakdown by medical specialization
- `sp_GetDuplicateAppointments` — Detect duplicate bookings
- `sp_GetNext7DaysAppointments` — Appointments scheduled in the next 7 days

---

## REST API Endpoints

### Patient Endpoints
| Method | Route | Description | Status |
|--------|-------|-------------|--------|
| `POST` | `/api/patients` | Register new patient | `201 Created` |
| `PUT` | `/api/patients/{id}` | Update patient details | `200 OK` |
| `DELETE` | `/api/patients/{id}/deactivate` | Soft-deactivate patient | `204 No Content` |
| `GET` | `/api/patients` | Active patients with age | `200 OK` |

### Doctor Endpoints
| Method | Route | Description | Status |
|--------|-------|-------------|--------|
| `GET` | `/api/doctors?spec=&available=` | Filter doctors | `200 OK` |

### Appointment Endpoints
| Method | Route | Description | Status |
|--------|-------|-------------|--------|
| `POST` | `/api/appointments` | Book appointment (transactional) | `201 Created` |
| `PUT` | `/api/appointments/{id}/cancel` | Cancel scheduled appointment | `200 OK` |
| `GET` | `/api/appointments/upcoming` | All upcoming appointments | `200 OK` |
| `GET` | `/api/appointments/doctor/{id}` | Appointments by doctor | `200 OK` |

### Reporting Endpoints
| Method | Route | Description | Status |
|--------|-------|-------------|--------|
| `GET` | `/api/reports/consolidated` | Full appointment report view | `200 OK` |

---

## Project / Execution Flow

This section describes how a request flows through the system end-to-end.

### 1. Client Request
A client (Postman, Swagger, or any API consumer) sends an HTTP request to an endpoint (e.g., `POST /api/appointments`).

### 2. API Layer — Controller
The ASP.NET Core Web API controller receives the request. Global middleware logs the HTTP method, path, and starts a response timer. Input payload is validated before any processing begins.

### 3. Domain Layer — Business Logic
The controller delegates to domain models/services. Domain logic runs here — age calculation, status checks, schedule formatting, and typed exception handling. If a domain rule is violated (e.g., booking a past date, duplicate patient code), a typed domain exception is thrown.

### 4. Data Access Layer — Repository
The repository (injected via DI) receives a validated domain object. ADO.NET executes the relevant Stored Procedure against SQL Server.

### 5. Database — SQL Server
The Stored Procedure runs within a transaction (for write operations). For appointment booking specifically:
- The DB checks doctor availability.
- If unavailable → transaction is rolled back → error propagates back.
- If available → appointment is inserted with `Status = Scheduled`.

### 6. Response Path
The result travels back up through the repository → domain → controller. The controller returns the appropriate HTTP response (201, 200, 204, 4xx, 500). Global middleware logs the final response time. Errors return a structured JSON response with zero internal detail exposed.

### Appointment Lifecycle Flow

```
POST /api/appointments
        │
        ▼
  Validate payload
        │
        ▼
  Check doctor availability (SP)
        │
   ┌────┴────┐
Unavailable  Available
   │            │
Rollback     Insert with
+ 400        Status = Scheduled
                │
                ▼
        PUT /api/appointments/{id}/cancel
                │
                ▼
        Status = Cancelled
        CancelledAt = timestamp
```

### Patient Lifecycle Flow

```
POST /api/patients  →  Register (IsActive = true)
PUT  /api/patients/{id}  →  Update details
DELETE /api/patients/{id}/deactivate  →  IsActive = false (soft delete)
GET  /api/patients  →  Returns only IsActive = true with live age
```

---

## HTTP Status Codes

| Code | Meaning | When |
|------|---------|------|
| `201 Created` | Resource created | Patient or Appointment successfully registered |
| `204 No Content` | Action done, no body | Deactivation or cancellation |
| `400 Bad Request` | Invalid input | Invalid payload, past date, domain rule violation |
| `404 Not Found` | Resource missing | Patient / Doctor / Appointment not found |
| `409 Conflict` | Duplicate detected | Duplicate phone, email, or entity code |
| `500 Server Error` | Internal error | Structured error response, zero internal detail exposed |

---

## Technical Highlights

**Dependency Injection** — All services are resolved via DI. Repository interfaces are injected into controllers, ensuring loose coupling and easy testability.

**Transaction Safety** — Appointment booking executes inside a SQL transaction. If the doctor is unavailable, the transaction rolls back with a descriptive error.

**Domain Logic Isolation** — Age calculation, status checks, and schedule formatting are handled in the domain model — keeping the DB and API layers clean.

**Global Middleware Logging** — Method, path, and response time are logged globally without modifying any individual controller.

**Structured Error Handling** — Typed domain exceptions are used throughout. All unhandled errors return a structured JSON response — no stack traces or internal details are leaked to the client.

**Input Validation** — All request payloads are validated before hitting the database. Unique constraints are enforced at both the application layer and the database layer.

---

## Deliverables

- **Working application** running on local machine — live demo ready
- **Full source code** — solution folder zipped
- **SQL script** — schema, all 14 stored procedures, and sample data
- **Postman collection** — pre-built requests for every endpoint
- **Project document** — structure, setup steps, and assumptions

---

## Setup & Running Locally

1. **Clone / unzip** the solution folder.
2. **Restore the database** by running the provided SQL script in SQL Server Management Studio (SSMS). This creates the schema, all 14 stored procedures, and loads sample data.
3. **Update the connection string** in `appsettings.json` to point to your SQL Server instance.
4. **Build and run** the ASP.NET Core Web API project:
   ```bash
   dotnet build
   dotnet run
   ```
5. **Test endpoints** using the provided Postman collection or via Swagger UI at `https://localhost:{port}/swagger`.

---

*Mini Project — City General Hospital Backend System*
