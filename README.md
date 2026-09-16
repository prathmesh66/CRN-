# Product RESTful API

A RESTful Backend API built using .NET 8, C#, ASP.NET Core Web API, SQL Server, and Entity Framework Core.

The application follows a layered architecture with separation of concerns and includes authentication, authorization, validation, error handling, testing, API documentation, and Docker support.

## Features

- Product CRUD operations
- Product and Item relationship
- JWT authentication
- Refresh token strategy with token rotation
- Role-based authorization
- Repository Pattern
- Unit of Work Pattern
- Service Layer
- DTO-based API contracts
- FluentValidation
- Global exception handling middleware
- API versioning
- Pagination
- Entity Framework Core
- AsNoTracking for read-only queries
- Async/Await
- Structured logging
- CORS configuration
- HTTPS configuration
- Security headers
- Response compression
- Swagger/OpenAPI documentation
- Unit testing with xUnit and Moq
- Integration testing support
- Docker and Docker Compose

## Technology Stack

| Technology | Usage |
|---|---|
| .NET 8 | Application Framework |
| C# | Programming Language |
| ASP.NET Core Web API | REST API |
| SQL Server | Database |
| Entity Framework Core | ORM |
| JWT | Authentication |
| FluentValidation | Request Validation |
| Swagger / OpenAPI | API Documentation |
| xUnit | Testing |
| Moq | Mocking |
| Docker | Containerization |

## Project Structure

```text
ProductApi/
│
├── src/
│   ├── API/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   └── ProductsController.cs
│   │   ├── Filters/
│   │   ├── Middleware/
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── Application/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Mapping/
│   │   ├── Services/
│   │   └── Validators/
│   │
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── Product.cs
│   │   │   └── Item.cs
│   │   ├── Enums/
│   │   ├── Events/
│   │   └── Exceptions/
│   │
│   └── Infrastructure/
│       ├── Data/
│       │   ├── Configurations/
│       │   ├── Repositories/
│       │   ├── ApplicationDbContext.cs
│       │   └── UnitOfWork.cs
│       ├── Identity/
│       ├── Logging/
│       └── Services/
│
├── tests/
│   ├── API.Tests/
│   ├── Application.Tests/
│   └── Infrastructure.Tests/
│
├── Dockerfile
├── docker-compose.yml
└── ProductApi.sln
