# Product Management REST API

A production-ready RESTful API built using **.NET 8**, following **Clean Architecture**, **Repository Pattern**, **Unit of Work**, **JWT Authentication**, and **Entity Framework Core**.

This project was developed as part of a technical assessment and demonstrates modern backend development practices.

---

# Features

- Product CRUD Operations
- Item Management
- Clean Architecture
- Repository Pattern
- Unit of Work Pattern
- Entity Framework Core
- SQL Server
- JWT Authentication
- Refresh Token Support
- FluentValidation
- Global Exception Handling Middleware
- Structured Logging
- Swagger / OpenAPI Documentation
- Docker Support
- xUnit & Moq Unit Tests
- Integration Tests using WebApplicationFactory

---

# Technology Stack

| Technology | Version |
|------------|----------|
| .NET | 8 |
| ASP.NET Core Web API | 8 |
| Entity Framework Core | 8 |
| SQL Server | 2022 |
| JWT Authentication | Yes |
| FluentValidation | Latest |
| AutoMapper | Latest |
| Serilog | Latest |
| xUnit | Latest |
| Moq | Latest |
| Docker | Latest |

---

# Environment Setup

## Prerequisites

Before running the application, ensure the following software is installed:

- .NET 8 SDK
- SQL Server 2022 (or SQL Server Express)
- SQL Server Management Studio (SSMS)
- Visual Studio 2022
- Docker Desktop (Optional)
- Git

## Clone Repository

```bash
git clone <your-github-repository-url>
cd ProductManagement
```

## Restore Packages

```bash
dotnet restore
```

## Apply Database Migrations

```bash
dotnet ef database update --startup-project ProductManagement.API
```

## Run the Application

```bash
dotnet run --project ProductManagement.API
```

Swagger will be available at:

```
https://localhost:xxxx/swagger
```



# Project Structure

```text
ProductManagement
│
├── ProductManagement.API
│   ├── Controllers
│   ├── Middleware
│   ├── Extensions
│   ├── Filters
│   └── Program.cs
│
├── ProductManagement.Application
│   ├── DTOs
│   ├── Services
│   ├── Interfaces
│   ├── Validators
│   └── Mapping
│
├── ProductManagement.Domain
│   ├── Entities
│   ├── Exceptions
│   └── Enums
│
├── ProductManagement.Infrastructure
│   ├── Data
│   ├── Repositories
│   ├── JWT
│   ├── Logging
│   └── DependencyInjection
│
├── ProductManagement.API.Tests
├── ProductManagement.Application.Tests
└── ProductManagement.Infrastructure.Tests
```

---

# Architecture

The solution follows **Clean Architecture**.

```text
Presentation (API)
        │
        ▼
Application
        │
        ▼
Domain
        ▲
        │
Infrastructure
```

Dependencies always point inward.

# High-Level Architecture

```text
                        Client (Swagger / Postman)
                                   │
                                   ▼
                    ASP.NET Core Web API (Presentation Layer)
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        │                          │                          │
        ▼                          ▼                          ▼
 Authentication            Product Controller          Global Middleware
       │                          │                          │
       └──────────────────────────┼──────────────────────────┘
                                  ▼
                      Application Layer (Business Logic)
                                  │
      ┌───────────────────────────┼───────────────────────────┐
      │                           │                           │
      ▼                           ▼                           ▼
 Product Service         Authentication Service       Validators
                                  │
                                  ▼
                        Repository Interfaces
                                  │
                                  ▼
                       Infrastructure Layer
      ┌───────────────────────────┼───────────────────────────┐
      │                           │                           │
      ▼                           ▼                           ▼
 Product Repository       User Repository            Unit of Work
                                  │
                                  ▼
                        Entity Framework Core
                                  │
                                  ▼
                             SQL Server
```

# Request Flow

```text
HTTP Request
      │
      ▼
Controller
      │
      ▼
Validation (FluentValidation)
      │
      ▼
Application Service
      │
      ▼
Repository
      │
      ▼
Entity Framework Core
      │
      ▼
SQL Server
      │
      ▼
API Response
```

# Exception Flow

```text
Controller
      │
      ▼
Application Service
      │
      ▼
Exception Thrown
      │
      ▼
Global Exception Middleware
      │
      ▼
Standard API Response
```

# Logging Flow

```text
Request
   │
   ▼
Serilog Request Logging
   │
   ▼
Controller
   │
   ▼
Application Service
   │
   ▼
Repository
   │
   ▼
SQL Server
```


---

# Authentication

- JWT Authentication
- Refresh Token
- Role-based Authorization
- Password Hashing

# Authentication Flow

The application uses **JWT (JSON Web Token)** based authentication with Refresh Token support.

```text
                Register
                    │
                    ▼
          Password Hashed
                    │
                    ▼
             User Saved
                    │
                    ▼
                 Login
                    │
                    ▼
      Validate Email & Password
                    │
                    ▼
         Generate JWT Access Token
                    │
                    ▼
      Generate Refresh Token
                    │
                    ▼
         Return Tokens to Client
                    │
                    ▼
Client sends JWT in Authorization Header
                    │
                    ▼
JWT Middleware validates Token
                    │
                    ▼
Protected API Endpoint
```

## JWT Claims

The access token contains the following claims:

| Claim | Description |
|--------|-------------|
| NameIdentifier | User Id |
| Name | Username |
| Email | User Email |
| Role | User Role |
| Jti | Unique Token Identifier |

## Authentication Sequence

1. User registers using the Register endpoint.
2. Password is securely hashed before storing in the database.
3. User logs in with valid credentials.
4. JWT Access Token is generated.
5. Refresh Token is generated and stored.
6. Client stores the tokens.
7. Client sends the Access Token in the `Authorization` header.
8. JWT Middleware validates the token before accessing protected endpoints.
9. Expired Access Tokens can be refreshed using the Refresh Token endpoint.

# Security Features

- JWT Authentication
- Password Hashing
- Refresh Token Support
- Role-Based Authorization
- HTTPS Support
- Global Exception Handling
- FluentValidation

# Protected Endpoints

| Endpoint | Authorization |
|----------|---------------|
| GET Products | Required |
| GET Product By Id | Required |
| POST Product | Required |
| PUT Product | Required |
| DELETE Product | Required |

Authentication endpoints:

- POST /api/v1/auth/register
- POST /api/v1/auth/login



---

# Logging

Structured logging using Serilog.

---

# Validation

FluentValidation is used for validating all incoming requests.

---

# Testing

- Unit Tests using xUnit and Moq
- Integration Tests using WebApplicationFactory

---


# API Endpoints

## Authentication

| Method | Endpoint | Description |
|---------|----------|-------------|
| POST | /api/v1/auth/register | Register a new user |
| POST | /api/v1/auth/login | Login user |
| POST | /api/v1/auth/refresh | Refresh access token |

---

## Products

| Method | Endpoint | Description |
|---------|----------|-------------|
| GET | /api/v1/products | Get all products |
| GET | /api/v1/products/{id} | Get product by Id |
| POST | /api/v1/products | Create product |
| PUT | /api/v1/products/{id} | Update product |
| DELETE | /api/v1/products/{id} | Delete product |

# HTTP Status Codes

| Status Code | Meaning |
|-------------|---------|
| 200 | Success |
| 201 | Resource Created |
| 204 | No Content |
| 400 | Bad Request |
| 401 | Unauthorized |
| 404 | Not Found |
| 500 | Internal Server Error |

# Deployment

The application supports deployment using Docker.

## Build Docker Image

```bash
docker build -f ProductManagement.API/Dockerfile -t productmanagement-api .
```

## Run Using Docker Compose

```bash
docker compose up
```

The API will be available at:

```
http://localhost:5000
```

SQL Server will be available on:

```
localhost:1433
```

# Docker

Run using:

```bash
docker compose up
```

---

# Swagger

```
https://localhost:xxxx/swagger
```

---

# Future Improvements

- Redis Caching
- API Versioning Expansion
- CQRS with MediatR
- Background Jobs using Hangfire
- Rate Limiting

---

# Known Limitations

- Docker execution depends on Docker Desktop and WSL2 being configured on the host machine.
- API Versioning is implemented with URL versioning (`v1`) and can be extended further.
- Refresh token persistence can be enhanced with additional security policies such as token reuse detection.

# Assessment Highlights

✔ Clean Architecture

✔ Repository Pattern

✔ Unit of Work

✔ Entity Framework Core

✔ SQL Server

✔ JWT Authentication

✔ Refresh Token Strategy

✔ FluentValidation

✔ Global Exception Handling

✔ Serilog Logging

✔ Swagger Documentation

✔ Docker Support

✔ xUnit & Moq Testing

✔ Integration Testing using WebApplicationFactory

# Author

Kaustubh Chaudhari