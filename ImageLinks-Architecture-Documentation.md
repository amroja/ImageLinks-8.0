# ImageLinks® - System Architecture Documentation

## Table of Contents
1. [Overview](#overview)
2. [Architectural Pattern](#architectural-pattern)
3. [Project Structure](#project-structure)
4. [Layer Responsibilities](#layer-responsibilities)
5. [Design Patterns](#design-patterns)
6. [Development Guidelines](#development-guidelines)
7. [Database Strategy](#database-strategy)
8. [Authentication & Authorization](#authentication--authorization)
9. [Error Handling Strategy](#error-handling-strategy)
10.[Coding Standards](#coding-standards)
11.[Testing Strategy](#testing-strategy)
12.[Deployment Considerations](#deployment-considerations)

---

## 1. Overview

**ImageLinks®** is a document management system built using .NET 9.0 with a clean architecture approach. The system supports multi-database environments (SQL Server and Oracle) and provides secure, scalable APIs for document and user management.

### Technology Stack
- **Framework**: .NET 9.0
- **API**: ASP.NET Core Web API
- **ORM**: Entity Framework Core 9.0.9
- **Micro-ORM**: Dapper 2.1.66
- **Databases**: SQL Server, Oracle
- **Authentication**: JWT (JSON Web Tokens)
- **Documentation**: OpenAPI/Swagger

---

## 2. Architectural Pattern

The system follows **Clean Architecture** (also known as Onion Architecture), which emphasizes:
- **Separation of Concerns**: Each layer has distinct responsibilities
- **Dependency Inversion**: Dependencies flow inward toward the domain
- **Testability**: Business logic is independent of external concerns
- **Maintainability**: Changes in one layer have minimal impact on others

### Architectural Diagram

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                   │
│                  (ImageLinks®.API)                      │
│  • Controllers                                          │
│  • Middleware (Exception Handling, Auth)                │
│  • Extensions                                           │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                   Application Layer                     │
│               (ImageLinks®.Application)                 │
│  • Services (Business Logic)                            │
│  • DTOs (Data Transfer Objects)                         │
│  • Interfaces (Repository Abstractions)                 │
│  • Mappers                                              │
│  • Common Models (Pagination, Requests)                 │
│  • Helpers & Utilities                                  │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                  │
│              (ImageLinks®.Infrastructure)               │
│  • Repository Implementations                           │
│  • Database Context (EF Core)                           │
│  • Data Access (Dapper)                                 │
│  • Entity Mappers                                       │
│  • External Service Integrations                        │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                      Domain Layer                       │
│                 (ImageLinks®.Domain)                    │
│  • Entities/Models                                      │
│  • Enums                                                │
│  • Result Types                                         │
│  • Domain Logic (if any)                                │
└─────────────────────────────────────────────────────────┘
```

### Dependency Flow
```
API → Application → Infrastructure → Domain
  ↓        ↓              ↓            ↑
  └────────┴──────────────┴────────────┘
           (All depend on Domain)
```

---

## 3. Project Structure

### 3.1 ImageLinks®.Domain

**Purpose**: Contains core business entities, enums, and domain logic.

**Key Components**:
- `Models/`: Entity classes representing database tables
  - `User.cs`: User entity with authentication properties
  - `SysSettingDetail.cs`: System settings configuration
  - `Tree.cs`, `Doc.cs`, `Group.cs`, etc.: Domain entities
- `Results/`: Result pattern implementation for error handling
  - `Result.cs`: Generic result wrapper
  - `Error.cs`: Error representation
  - `ErrorKind.cs`: Error type enumeration
- `Enums/`: Application-wide enumerations
  - `DatabaseProvider.cs`: Database type enumeration

**Dependencies**: 
- Microsoft.EntityFrameworkCore.Abstractions (for annotations only)

**Rules**:
- ✅ No dependencies on other project layers
- ✅ Contains only pure domain logic
- ✅ Entity classes should have data annotations
- ❌ No external service dependencies
- ❌ No infrastructure concerns

---

### 3.2 ImageLinks®.Application

**Purpose**: Contains business logic, service interfaces, DTOs, and application workflows.

**Key Components**:

#### Features Structure (Feature Folders)
```
Features/
├── Identity/
│   ├── DTO/
│   │   └── TokenDto.cs
│   ├── Requests/
│   │   ├── LoginRequest.cs
│   │   └── TokenRequest.cs
│   └── Services/
│       ├── Interface/
│       │   └── IIdentityService.cs
│       └── Implementation/
│           └── IdentityService.cs
│
├── Users/
│   ├── DTO/
│   │   └── UserDto.cs
│   ├── IRepository/
│   │   └── IUserRepository.cs
│   ├── Mappers/
│   │   └── UserDtoMapper.cs
│   ├── Requests/
│   │   └── UserRequest.cs
│   └── Services/
│       ├── Interface/
│       │   └── IUserService.cs
│       └── Implementation/
│           └── UserService.cs
│
└── SysSettingDetail/
    ├── DTO/
    │   └── SysSettingDetailDto.cs
    └── IRepository/
        └── ISysSettingDetailRepository.cs
```

#### Common Components
```
Common/
├── Models/
│   ├── PaginatedList.cs      # Generic pagination wrapper
│   ├── PageRequest.cs         # Pagination request model
│   └── FilterFields.cs        # Dynamic filtering support
├── Helpers/
│   ├── Encryption.cs          # AES encryption utilities
│   └── GeneralFunction.cs     # SQL generation, date helpers
└── Enums/                     # (Reserved for future use)
```

#### Core Interfaces
```
IRepository/
├── IRepository.cs             # Generic repository interface
└── IGenericRepository.cs      # Dapper-based data access
```

**Dependencies**:
- Dapper
- Microsoft.Extensions.Configuration.Abstractions
- System.IdentityModel.Tokens.Jwt
- ImageLinks®.Domain (project reference)

**Rules**:
- ✅ Organize by features (vertical slicing)
- ✅ Use DTOs for data transfer
- ✅ Define service interfaces here
- ✅ Business logic belongs in services
- ❌ No direct database access (use interfaces)
- ❌ No HTTP concerns (controllers, routing)

---

### 3.3 ImageLinks®.Infrastructure

**Purpose**: Implements data access, external services, and infrastructure concerns.

**Key Components**:

#### Repository Implementations
```
Features/
├── Users/
│   ├── Repository/
│   │   └── UserRepository.cs
│   └── Mappers/
│       └── UserEntityMapper.cs
│
└── SysSettingDetail/
    ├── Repository/
    │   └── SysSettingDetailRepository.cs
    └── Mappers/
        └── SysSettingDetailEntityMapper.cs
```

#### Core Infrastructure
```
Repository/
├── Repository.cs              # EF Core generic repository
└── GenericRepository.cs       # Dapper implementation

Data/
└── ApplicationDbContext.cs    # EF Core DbContext
```

**Dependencies**:
- Dapper
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Oracle.EntityFrameworkCore
- Oracle.ManagedDataAccess.Core
- ImageLinks®.Application (project reference)

**Rules**:
- ✅ Implement repository interfaces from Application layer
- ✅ Use both EF Core and Dapper as needed
- ✅ Handle database-specific logic here
- ✅ Map DataTable/DataRow to entities
- ❌ No business logic
- ❌ No direct controller dependencies

---

### 3.4 ImageLinks®.API

**Purpose**: Entry point for the application, handling HTTP requests and responses.

**Key Components**:

#### Controllers
```
Controllers/
├── ApiController.cs           # Base controller with error handling
├── IdentityController.cs      # Authentication endpoints
└── UserController.cs          # User management endpoints
```

#### Middleware & Extensions
```
Middleware/
└── GlobalExceptionHandler.cs  # Centralized exception handling

Extensions/
└── ProblemExtensions.cs       # Error to ProblemDetails conversion
```

#### Configuration
```
appsettings.json               # Production settings
appsettings.Development.json   # Development overrides
Program.cs                     # Application startup & DI configuration
```

**Dependencies**:
- Microsoft.AspNetCore.OpenApi
- Microsoft.AspNetCore.Authentication.JwtBearer
- ImageLinks®.Infrastructure (project reference)

**Rules**:
- ✅ Controllers should be thin (delegate to services)
- ✅ Use DTOs for request/response
- ✅ Apply authorization attributes
- ✅ Document endpoints with attributes
- ❌ No business logic in controllers
- ❌ No direct database access

---

## 4. Layer Responsibilities

### 4.1 Domain Layer Responsibilities
- **What it does**:
  - Defines core business entities
  - Contains domain enumerations
  - Provides result/error types
  - Houses value objects (if needed)

- **What it DOES NOT do**:
  - Database access
  - HTTP concerns
  - External service calls
  - UI/presentation logic

### 4.2 Application Layer Responsibilities
- **What it does**:
  - Implements business logic in services
  - Defines contracts (interfaces) for repositories
  - Provides DTOs for data transfer
  - Contains validation logic
  - Orchestrates workflows
  - Maps between DTOs and entities

- **What it DOES NOT do**:
  - Direct database access
  - HTTP request/response handling
  - Infrastructure implementations

### 4.3 Infrastructure Layer Responsibilities
- **What it does**:
  - Implements repository interfaces
  - Manages database connections
  - Executes queries (EF Core & Dapper)
  - Handles database-specific logic
  - Maps database results to entities
  - Integrates with external services

- **What it DOES NOT do**:
  - Business logic
  - DTO definitions
  - HTTP handling

### 4.4 API Layer Responsibilities
- **What it does**:
  - Handles HTTP requests/responses
  - Routes requests to services
  - Manages authentication/authorization
  - Handles exceptions globally
  - Configures dependency injection
  - Provides API documentation

- **What it DOES NOT do**:
  - Business logic
  - Direct database access
  - Complex data transformations

---

## 5. Design Patterns

### 5.1 Repository Pattern

**Purpose**: Abstraction over data access logic.

**Implementation**:
```csharp
// Interface in Application Layer
public interface IRepository<T> where T : class
{
    Task<List<T>> GetAll(CancellationToken ct = default, 
                         Expression<Func<T, bool>>? filter = null, 
                         string? includeProperties = null, 
                         bool tracked = false);
    Task<T> Get(Expression<Func<T, bool>> filter, 
                string? includeProperties = null, 
                bool tracked = false);
    void Add(T entity);
    bool Any(Expression<Func<T, bool>> filter);
    void Remove(T entity);
}

// Implementation in Infrastructure Layer
public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    // Implementation...
}
```

**Usage Guidelines**:
- Use generic repository for simple CRUD operations
- Create specific repositories (e.g., `IUserRepository`) for complex queries
- Leverage both EF Core and Dapper based on needs:
  - **EF Core**: For change tracking, navigation properties, complex relationships
  - **Dapper**: For performance-critical queries, dynamic SQL, reporting

---

### 5.2 Result Pattern

**Purpose**: Encapsulate operation results with success/failure information.

**Implementation**:
```csharp
public sealed class Result<TValue>
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public TValue Value { get; }
    public List<Error> Errors { get; }
    public Error TopError { get; }
    
    public TNextValue Match<TNextValue>(
        Func<TValue, TNextValue> onValue, 
        Func<List<Error>, TNextValue> onError)
    {
        return IsSuccess ? onValue(Value!) : onError(Errors);
    }
}
```

**Usage Guidelines**:
- Return `Result<T>` from service methods
- Use implicit conversion from T or Error
- Handle results in controllers using `Match` method

**Example**:
```csharp
// Service Layer
public async Task<Result<TokenDto>> Login(LoginRequest login, CancellationToken ct)
{
    if (string.IsNullOrWhiteSpace(login.UserName))
        return Error.Validation("InvalidCredentials", "Username is required.");
    
    var user = await _userRepository.SelectAsync(filter, ct);
    if (user == null)
        return Error.NotFound("UserNotFound", "Invalid credentials.");
    
    return CreateToken(user); // Implicit conversion
}

// Controller Layer
public async Task<IActionResult> GenerateToken([FromBody] LoginRequest request, CancellationToken ct)
{
    var result = await _identity.Login(request, ct);
    return result.Match(
        response => Ok(response),
        Problem  // Converts errors to ProblemDetails
    );
}
```

---

### 5.3 Dependency Injection Pattern

**Purpose**: Inversion of control for loose coupling.

**Configuration** (`Program.cs`):
```csharp
// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (provider == "SQL")
        options.UseSqlServer(connStr);
    else if (provider == "ORACLE")
        options.UseOracle(connStr);
});

// Repositories
builder.Services.AddScoped<IGenericRepository, GenericRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISysSettingDetailRepository, SysSettingDetailRepository>();

// Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
```

**Guidelines**:
- Use `AddScoped` for database-related services (per-request lifetime)
- Use `AddSingleton` for stateless, thread-safe services
- Use `AddTransient` for lightweight, stateless services
- Always inject interfaces, not concrete classes

---

### 5.4 Service Layer Pattern

**Purpose**: Encapsulate business logic and orchestrate workflows.

**Structure**:
```
Feature/
└── Services/
    ├── Interface/
    │   └── IFeatureService.cs
    └── Implementation/
        └── FeatureService.cs
```

**Example**:
```csharp
public interface IUserService
{
    Task<Result<List<UserDto>>> GetAllUsersEF(CancellationToken ct);
    Task<Result<PaginatedList<UserDto>>> GetAllUsersEfFilter(
        PageRequest pageRequest, CancellationToken ct);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<List<UserDto>>> GetAllUsersEF(CancellationToken ct)
    {
        var users = await _userRepository.GetAll(ct);
        return users.ToDtos(); // Implicit conversion
    }
}
```

---

### 5.5 Mapper Pattern

**Purpose**: Convert between entities and DTOs.

**Implementation**:
```csharp
public static class UserDtoMapper
{
    public static UserDto ToDto(this User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        return new UserDto(user.UserId.ToString(), user.UserName);
    }

    public static List<UserDto> ToDtos(this IEnumerable<User> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
```

**Guidelines**:
- Create extension methods for clean syntax
- Keep mappers in the Application layer
- Create separate mappers for:
  - Entity → DTO (Application layer)
  - DataRow → Entity (Infrastructure layer)
- Use null checks (ArgumentNullException.ThrowIfNull)

---

## 6. Development Guidelines

### 6.1 Feature Development Workflow

#### Step 1: Define Domain Entity (Domain Layer)
```csharp
// ImageLinks®.Domain/Models/Feature.cs
[Table("FEATURES")]
public class Feature
{
    [Key]
    [Column("FEATURE_ID")]
    public decimal FeatureId { get; set; }
    
    [Column("FEATURE_NAME")]
    [MaxLength(100)]
    public string? FeatureName { get; set; }
}
```

#### Step 2: Create DTO (Application Layer)
```csharp
// ImageLinks®.Application/Features/FeatureName/DTO/FeatureDto.cs
namespace ImageLinks_.Application.Features.FeatureName.DTO;

public record FeatureDto(string FeatureId, string FeatureName);
```

#### Step 3: Create Request Models (Application Layer)
```csharp
// ImageLinks®.Application/Features/FeatureName/Requests/CreateFeatureRequest.cs
namespace ImageLinks_.Application.Features.FeatureName.Requests;

public record CreateFeatureRequest(string FeatureName);
```

#### Step 4: Define Repository Interface (Application Layer)
```csharp
// ImageLinks®.Application/Features/FeatureName/IRepository/IFeatureRepository.cs
namespace ImageLinks_.Application.Features.FeatureName.IRepository;

public interface IFeatureRepository : IRepository<Feature>
{
    Task<Feature?> SelectAsync(Feature filter, CancellationToken ct = default);
    Task<List<Feature>> GetAllFeatures(CancellationToken ct = default);
}
```

#### Step 5: Implement Repository (Infrastructure Layer)
```csharp
// ImageLinks®.Infrastructure/Features/FeatureName/Repository/FeatureRepository.cs
namespace ImageLinks_.Infrastructure.Features.FeatureName.Repository;

public class FeatureRepository : Repository<Feature>, IFeatureRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;
    
    public FeatureRepository(ApplicationDbContext db, IGenericRepository genericService) 
        : base(db)
    {
        _db = db;
        _genericService = genericService;
    }
    
    public async Task<Feature?> SelectAsync(Feature filter, CancellationToken ct = default)
    {
        // Dapper implementation with dynamic SQL
        var dbType = _genericService.GetDatabaseType();
        var sql = new StringBuilder(@"
            SELECT 
                FEATURE_ID AS FeatureId,
                FEATURE_NAME AS FeatureName
            FROM FEATURES 
            WHERE 1 = 1
        ");
        
        var parameters = new DynamicParameters();
        
        if (!string.IsNullOrEmpty(filter.FeatureName))
        {
            sql.Append($" AND LOWER(FEATURE_NAME) = LOWER({GeneralFunction.GetParam("FeatureName", dbType)})");
            parameters.Add("FeatureName", filter.FeatureName);
        }
        
        var list = await _genericService.GetListAsync<Feature>(sql.ToString(), parameters, null, ct);
        return list.FirstOrDefault();
    }
    
    public async Task<List<Feature>> GetAllFeatures(CancellationToken ct = default)
    {
        string sql = @"
            SELECT 
                FEATURE_ID AS FeatureId,
                FEATURE_NAME AS FeatureName
            FROM FEATURES";
        
        return await _genericService.GetListAsync<Feature>(sql, null, null, ct);
    }
}
```

#### Step 6: Create Mapper (Application Layer)
```csharp
// ImageLinks®.Application/Features/FeatureName/Mappers/FeatureDtoMapper.cs
namespace ImageLinks_.Application.Features.FeatureName.Mappers;

public static class FeatureDtoMapper
{
    public static FeatureDto ToDto(this Feature feature)
    {
        ArgumentNullException.ThrowIfNull(feature);
        return new FeatureDto(
            feature.FeatureId.ToString(), 
            feature.FeatureName ?? string.Empty
        );
    }

    public static List<FeatureDto> ToDtos(this IEnumerable<Feature> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
```

#### Step 7: Create Service Interface and Implementation (Application Layer)
```csharp
// ImageLinks®.Application/Features/FeatureName/Services/Interface/IFeatureService.cs
namespace ImageLinks_.Application.Features.FeatureName.Services.Interface;

public interface IFeatureService
{
    Task<Result<List<FeatureDto>>> GetAllFeatures(CancellationToken ct);
    Task<Result<FeatureDto>> CreateFeature(CreateFeatureRequest request, CancellationToken ct);
}

// ImageLinks®.Application/Features/FeatureName/Services/Implementation/FeatureService.cs
namespace ImageLinks_.Application.Features.FeatureName.Services.Implementation;

public class FeatureService : IFeatureService
{
    private readonly IFeatureRepository _featureRepository;
    
    public FeatureService(IFeatureRepository featureRepository)
    {
        _featureRepository = featureRepository;
    }
    
    public async Task<Result<List<FeatureDto>>> GetAllFeatures(CancellationToken ct)
    {
        var features = await _featureRepository.GetAllFeatures(ct);
        return features.ToDtos();
    }
    
    public async Task<Result<FeatureDto>> CreateFeature(
        CreateFeatureRequest request, CancellationToken ct)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.FeatureName))
            return Error.Validation("InvalidName", "Feature name is required.");
        
        // Business logic
        var feature = new Feature
        {
            FeatureName = request.FeatureName
        };
        
        _featureRepository.Add(feature);
        await _db.SaveChangesAsync(ct);
        
        return feature.ToDto();
    }
}
```

#### Step 8: Create Controller (API Layer)
```csharp
// ImageLinks®.API/Controllers/FeatureController.cs
namespace ImageLinks_.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FeatureController : ApiController
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<FeatureDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Retrieves all features")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _featureService.GetAllFeatures(ct);
        return result.Match(
            response => Ok(response),
            Problem
        );
    }

    [HttpPost]
    [ProducesResponseType(typeof(FeatureDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Creates a new feature")]
    public async Task<IActionResult> Create(
        [FromBody] CreateFeatureRequest request, 
        CancellationToken ct)
    {
        var result = await _featureService.CreateFeature(request, ct);
        return result.Match(
            response => CreatedAtAction(nameof(GetAll), new { id = response.FeatureId }, response),
            Problem
        );
    }
}
```

#### Step 9: Register Dependencies (API Layer)
```csharp
// ImageLinks®.API/Program.cs
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
```

#### Step 10: Update DbContext (Infrastructure Layer)
```csharp
// ImageLinks®.Infrastructure/Data/ApplicationDbContext.cs
public class ApplicationDbContext : DbContext
{
    // ... existing DbSets ...
    
    public DbSet<Feature> FEATURES { get; set; }
}
```

---

### 6.2 Naming Conventions

#### Projects
- `ImageLinks®.[LayerName]` (e.g., `ImageLinks®.API`, `ImageLinks®.Domain`)

#### Namespaces
```csharp
// Pattern: ImageLinks_.{Layer}.{Feature}.{Category}
namespace ImageLinks_.Application.Features.Users.Services.Interface;
namespace ImageLinks_.Infrastructure.Features.Users.Repository;
```

#### Files and Classes
- **Entities**: PascalCase, singular (e.g., `User.cs`, `Document.cs`)
- **DTOs**: PascalCase with "Dto" suffix (e.g., `UserDto`, `TokenDto`)
- **Interfaces**: PascalCase with "I" prefix (e.g., `IUserService`, `IUserRepository`)
- **Services**: PascalCase with "Service" suffix (e.g., `UserService`, `IdentityService`)
- **Repositories**: PascalCase with "Repository" suffix (e.g., `UserRepository`)
- **Controllers**: PascalCase with "Controller" suffix (e.g., `UserController`)
- **Requests**: PascalCase with "Request" suffix (e.g., `LoginRequest`, `CreateUserRequest`)

#### Methods
- **Async methods**: Use "Async" suffix (e.g., `GetUserAsync`, `SaveChangesAsync`)
- **Query methods**: Start with "Get", "Find", "Select" (e.g., `GetAllUsers`, `FindUserById`)
- **Command methods**: Use verbs (e.g., `Create`, `Update`, `Delete`, `Add`, `Remove`)

#### Variables and Parameters
- **Private fields**: Start with underscore (e.g., `_db`, `_userRepository`)
- **Parameters**: camelCase (e.g., `userName`, `cancellationToken`, `ct`)
- **Local variables**: camelCase (e.g., `user`, `result`, `parameters`)

---

### 6.3 Folder Organization Rules

#### Feature Folder Structure
```
Features/
└── {FeatureName}/
    ├── DTO/
    │   └── {Feature}Dto.cs
    ├── Requests/
    │   ├── Create{Feature}Request.cs
    │   ├── Update{Feature}Request.cs
    │   └── {Feature}FilterRequest.cs
    ├── IRepository/                    (Application layer only)
    │   └── I{Feature}Repository.cs
    ├── Mappers/
    │   ├── {Feature}DtoMapper.cs       (Application layer)
    │   └── {Feature}EntityMapper.cs    (Infrastructure layer)
    ├── Services/                       (Application layer only)
    │   ├── Interface/
    │   │   └── I{Feature}Service.cs
    │   └── Implementation/
    │       └── {Feature}Service.cs
    └── Repository/                     (Infrastructure layer only)
        └── {Feature}Repository.cs
```

**Rules**:
- One feature per folder
- Use vertical slicing (group by feature, not technical layer)
- Keep related files together
- Avoid deep nesting (max 3-4 levels)

---

## 7. Database Strategy

### 7.1 Multi-Database Support

The system supports both SQL Server and Oracle through:

#### Configuration
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;",
    "DefaultConnection_ProviderName": "SQL",
    
    // Or for Oracle:
    // "DefaultConnection": "User ID=...;Data Source=...",
    // "DefaultConnection_ProviderName": "ORACLE",
    
    "MasterConnection": "...",
    "MasterConnection_ProviderName": "SQL"
  }
}
```

#### Provider Detection
```csharp
public DatabaseProvider GetDatabaseType(string? nameOrConn = null)
{
    var providerName = _configuration.GetConnectionString($"{nameOrConn}_ProviderName");
    
    return providerName?.ToUpperInvariant() switch
    {
        "SQL" or "SYSTEM.DATA.SQLCLIENT" => DatabaseProvider.SqlServer,
        "ORACLE" or "ORACLE.MANAGEDDATAACCESS.CLIENT" => DatabaseProvider.Oracle,
        _ => throw new NotSupportedException($"Provider not supported")
    };
}
```

### 7.2 Dual Data Access Approach

#### When to use Entity Framework Core
- **CRUD operations** with related entities
- **Change tracking** is needed
- **Navigation properties** are required
- Working with **complex relationships**
- Need **migration support**

**Example**:
```csharp
public async Task<List<User>> GetAll(CancellationToken ct, 
    Expression<Func<User, bool>>? filter = null)
{
    IQueryable<User> query = dbSet.AsNoTracking();
    
    if (filter != null)
        query = query.Where(filter);
    
    return await query.ToListAsync(ct);
}
```

#### When to use Dapper
- **Performance-critical queries**
- **Dynamic SQL** generation
- **Reporting queries** with complex joins
- **Stored procedures**
- **Bulk operations**
- **Database-agnostic queries**

**Example**:
```csharp
public async Task<User?> SelectAsync(User filter, CancellationToken ct = default)
{
    var dbType = _genericService.GetDatabaseType();
    var sql = new StringBuilder("SELECT ... FROM USERS WHERE 1 = 1");
    var parameters = new DynamicParameters();
    
    if (!string.IsNullOrEmpty(filter.UserName))
    {
        sql.Append($" AND LOWER(USER_NAME) = LOWER({GeneralFunction.GetParam("UserName", dbType)})");
        parameters.Add("UserName", filter.UserName);
    }
    
    var list = await _genericService.GetListAsync<User>(sql.ToString(), parameters, null, ct);
    return list.FirstOrDefault();
}
```

### 7.3 Database-Agnostic SQL

Use helper methods to generate database-specific parameter syntax:

```csharp
public static string GetParam(string name, DatabaseProvider provider)
{
    return provider == DatabaseProvider.SqlServer 
        ? $"@{name}"    // SQL Server: @parameter
        : $":{name}";   // Oracle: :parameter
}
```

**Usage**:
```csharp
var dbType = _genericService.GetDatabaseType();
sql.Append($" AND USER_ID = {GeneralFunction.GetParam("UserId", dbType)}");
parameters.Add("UserId", userId);
```

### 7.4 Pagination Strategy

The system provides database-agnostic pagination:

```csharp
public static (string Sql, string CountSql, DynamicParameters Parameters) BuildPagedSql(
    string sqlStarter,
    string targetedSql,
    PageRequest pageRequest,
    List<FilterFields> filterFieldsList,
    Type modelType,
    DatabaseProvider provider)
{
    var (whereClause, parameters) = BuildWhereClause(...);
    string orderByClause = BuildOrderByClause(...);
    
    string sql = provider switch
    {
        DatabaseProvider.SqlServer =>
            $@"{baseSql}
               OFFSET (@Page - 1) * @PageSize ROWS
               FETCH NEXT @PageSize ROWS ONLY;",
               
        DatabaseProvider.Oracle =>
            $@"{baseSql}
               OFFSET (:Page - 1) * :PageSize ROWS
               FETCH NEXT :PageSize ROWS ONLY",
               
        _ => throw new NotSupportedException("Database provider not supported")
    };
    
    return (sql, countSql, parameters);
}
```

---

## 8. Authentication & Authorization

### 8.1 JWT Implementation

#### Configuration
```json
// appsettings.json
{
  "JwtSettings": {
    "Secret": "your-secret-key-here",
    "TokenExpirationInMinutes": 60,
    "Issuer": "localhost",
    "Audience": "localhost"
  }
}
```

#### Service Registration
```csharp
// Program.cs
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localhost",
        ValidAudience = "localhost",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();
```

#### Token Generation
```csharp
private TokenDto CreateAsync(User user)
{
    var jwtSettings = _configuration.GetSection("JwtSettings");
    var expires = DateTime.UtcNow.AddMinutes(
        int.Parse(jwtSettings["TokenExpirationInMinutes"]!));

    var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
        new(JwtRegisteredClaimNames.Name, user.UserName!),
    };

    var descriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = expires,
        Issuer = jwtSettings["Issuer"],
        Audience = jwtSettings["Audience"],
        SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
            SecurityAlgorithms.HmacSha256Signature)
    };

    var tokenHandler = new JwtSecurityTokenHandler();
    var securityToken = tokenHandler.CreateToken(descriptor);

    return new TokenDto
    {
        AccessToken = tokenHandler.WriteToken(securityToken),
        ExpiresOnUtc = expires
    };
}
```

### 8.2 Securing Endpoints

#### Apply Authorization
```csharp
[HttpGet("da")]
[Authorize]  // Requires authentication
public async Task<IActionResult> GetAllDa(CancellationToken ct)
{
    var result = await _userService.GetAllUsersDP(ct);
    return result.Match(response => Ok(response), Problem);
}
```

#### Role-Based Authorization (Future Enhancement)
```csharp
[Authorize(Roles = "Admin")]
public async Task<IActionResult> DeleteUser(int userId)
{
    // Only admins can access
}

[Authorize(Policy = "RequireManagerRole")]
public async Task<IActionResult> ApproveDocument(int documentId)
{
    // Custom policy
}
```

### 8.3 Password Encryption

The system uses AES encryption for passwords:

```csharp
public class Encryption
{
    private const string AES_KEY = "YNe8YwuIn1zxt3FPWTZFOr==";
    private const string AES_IV = "asxnWolsAyn7kCfWutrnqg==";
    
    public static string EncryptAES(string plainText)
    {
        using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
        {
            aes.Key = Convert.FromBase64String(AES_KEY);
            aes.IV = Convert.FromBase64String(AES_IV);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            // ... encryption logic
        }
    }
    
    public static string DecryptAES(string encryptedText)
    {
        // ... decryption logic
    }
}
```

**Usage**:
```csharp
var encryptedPassword = Encryption.EncryptAES(login.Password);
var user = await _userRepository.SelectAsync(new User 
{ 
    UserName = login.UserName, 
    UserPass = encryptedPassword 
});
```

---

## 9. Error Handling Strategy

### 9.1 Global Exception Handler

```csharp
public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "Application error",
                Detail = exception.Message,
            }
        });
    }
}
```

**Registration**:
```csharp
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// In middleware pipeline
app.UseExceptionHandler();
```

### 9.2 Result Pattern Error Handling

#### Error Types
```csharp
public enum ErrorKind
{
    Failure,        // General failure
    Unexpected,     // Unexpected error (500)
    Validation,     // Validation error (400)
    Conflict,       // Conflict error (409)
    NotFound,       // Not found (404)
    Unauthorized,   // Unauthorized (401)
    Forbidden,      // Forbidden (403)
}
```

#### Creating Errors
```csharp
// In service layer
if (string.IsNullOrWhiteSpace(login.UserName))
    return Error.Validation("InvalidCredentials", "Username is required.");

if (user == null)
    return Error.NotFound("UserNotFound", "Invalid username or password.");

// Multiple errors
var errors = new List<Error>
{
    Error.Validation("Field1", "Field1 is required"),
    Error.Validation("Field2", "Field2 must be positive")
};
return errors;
```

#### Handling Errors in Controllers
```csharp
protected ActionResult Problem(List<Error> errors)
{
    if (errors.Count is 0)
        return Problem();

    // All validation errors → 400 Bad Request with details
    if (errors.All(error => error.Type == ErrorKind.Validation))
        return ValidationProblem(errors);

    // Other errors → appropriate status code
    return Problem(errors[0]);
}

private ObjectResult Problem(Error error)
{
    var statusCode = error.Type switch
    {
        ErrorKind.Conflict => StatusCodes.Status409Conflict,
        ErrorKind.Validation => StatusCodes.Status400BadRequest,
        ErrorKind.NotFound => StatusCodes.Status404NotFound,
        ErrorKind.Unauthorized => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError,
    };

    return Problem(statusCode: statusCode, title: error.Description);
}
```

### 9.3 API Error Responses

#### Standard Format (ProblemDetails)
```json
{
  "type": "NotFound",
  "title": "Invalid username or password",
  "status": 404,
  "traceId": "00-abc123..."
}
```

#### Validation Errors
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "InvalidCredentials": ["Username is required"],
    "PasswordTooShort": ["Password must be at least 8 characters"]
  },
  "traceId": "00-abc123..."
}
```

---

## 10. Coding Standards

### 10.1 General Principles

#### SOLID Principles
- **S**ingle Responsibility: Each class should have one reason to change
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Subtypes must be substitutable for base types
- **I**nterface Segregation: Many specific interfaces are better than one general
- **D**ependency Inversion: Depend on abstractions, not concretions

#### DRY (Don't Repeat Yourself)
- Extract common logic into helper methods
- Use base classes for shared functionality
- Create reusable components

#### KISS (Keep It Simple, Stupid)
- Prefer simple solutions over complex ones
- Avoid premature optimization
- Write self-documenting code

### 10.2 Code Quality Standards

#### Null Safety
```csharp
// Use nullable reference types (enabled by default in .NET 9)
public string? UserName { get; set; }  // Can be null
public string UserEmail { get; set; } = string.Empty;  // Cannot be null

// Use null checks
ArgumentNullException.ThrowIfNull(user);

// Use null-conditional operators
var name = user?.UserName ?? "Unknown";

// Pattern matching
if (result is { IsSuccess: true })
{
    // Handle success
}
```

#### Async/Await Best Practices
```csharp
// ✅ Good: Pass cancellation token
public async Task<User> GetUserAsync(int id, CancellationToken ct)
{
    return await _db.Users.FirstOrDefaultAsync(u => u.UserId == id, ct);
}

// ✅ Good: Use ConfigureAwait(false) for library code
await SomeOperationAsync().ConfigureAwait(false);

// ❌ Bad: Blocking on async code
var user = GetUserAsync(id, ct).Result;  // Can cause deadlocks

// ❌ Bad: async void (except for event handlers)
public async void ProcessData() { }  // Should return Task
```

#### Exception Handling
```csharp
// ✅ Good: Catch specific exceptions
try
{
    await _db.SaveChangesAsync(ct);
}
catch (DbUpdateConcurrencyException ex)
{
    _logger.LogError(ex, "Concurrency error saving user");
    return Error.Conflict("ConcurrencyError", "Record was modified by another user");
}

// ✅ Good: Use Result pattern instead of throwing
public async Task<Result<User>> GetUser(int id)
{
    var user = await _repository.GetAsync(id);
    if (user == null)
        return Error.NotFound("UserNotFound", $"User {id} not found");
    
    return user;
}

// ❌ Bad: Catching and swallowing exceptions
try
{
    // code
}
catch { }  // Never do this
```

#### Dependency Injection
```csharp
// ✅ Good: Constructor injection
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    
    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
}

// ❌ Bad: Service locator pattern
public class UserService
{
    private IUserRepository _userRepository;
    
    public UserService()
    {
        _userRepository = ServiceLocator.Get<IUserRepository>();  // Anti-pattern
    }
}
```

### 10.3 Documentation Standards

#### XML Documentation Comments
```csharp
/// <summary>
/// Authenticates a user and generates JWT tokens.
/// </summary>
/// <param name="login">Login credentials containing username and password</param>
/// <param name="ct">Cancellation token</param>
/// <returns>
/// A Result containing TokenDto on success, or Error on failure.
/// </returns>
/// <exception cref="ArgumentNullException">Thrown when login is null</exception>
public async Task<Result<TokenDto>> Login(LoginRequest login, CancellationToken ct)
{
    // Implementation
}
```

#### Endpoint Documentation
```csharp
[HttpPost("login")]
[ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[EndpointSummary("Generates an access and refresh token for a valid user.")]
[EndpointDescription("Authenticates a user using provided credentials and returns a JWT token pair.")]
[EndpointName("GenerateToken")]
public async Task<IActionResult> GenerateToken([FromBody] LoginRequest request, CancellationToken ct)
{
    // Implementation
}
```

### 10.4 Performance Best Practices

#### Database Queries
```csharp
// ✅ Good: Use AsNoTracking for read-only queries
var users = await _db.Users.AsNoTracking().ToListAsync(ct);

// ✅ Good: Use projection to select only needed fields
var userNames = await _db.Users
    .Select(u => new { u.UserId, u.UserName })
    .ToListAsync(ct);

// ✅ Good: Use pagination for large datasets
var pagedUsers = await _db.Users
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync(ct);

// ❌ Bad: Loading entire collections
var allUsers = await _db.Users.ToListAsync();  // If there are millions
var filtered = allUsers.Where(u => u.IsActive);  // Filter in memory
```

#### String Operations
```csharp
// ✅ Good: Use StringBuilder for concatenation in loops
var sql = new StringBuilder("SELECT * FROM Users WHERE 1=1");
foreach (var filter in filters)
{
    sql.Append($" AND {filter.Field} = {filter.Value}");
}

// ❌ Bad: String concatenation in loops
string sql = "SELECT * FROM Users WHERE 1=1";
foreach (var filter in filters)
{
    sql += $" AND {filter.Field} = {filter.Value}";  // Creates new string each time
}
```

---

## 11. Testing Strategy

### 11.1 Testing Pyramid

```
       /\
      /  \      Unit Tests (70%)
     /____\     - Fast, isolated
    /      \    - Test business logic
   / Integ. \   Integration Tests (20%)
  /__________\  - Test layer interactions
 /            \ End-to-End Tests (10%)
/______________\- Test full workflows
```

### 11.2 Unit Testing Guidelines

#### Test Structure (AAA Pattern)  (Arrange-Act-Assert)
```csharp
[Fact]
public async Task Login_WithValidCredentials_ReturnsToken()
{
    // Arrange
    var mockRepo = new Mock<IUserRepository>();
    var mockConfig = new Mock<IConfiguration>();
    
    var user = new User 
    { 
        UserId = 1, 
        UserName = "testuser",
        UserPass = "encrypted_password" 
    };
    
    mockRepo.Setup(r => r.SelectAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
    
    var service = new IdentityService(mockRepo.Object, mockConfig.Object);
    var request = new LoginRequest("testuser", "testuser", "password");
    
    // Act
    var result = await service.Login(request, CancellationToken.None);
    
    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value.AccessToken);
    Assert.True(result.Value.ExpiresOnUtc > DateTime.UtcNow);
}
```

#### What to Test
- ✅ Business logic in services
- ✅ Validation rules
- ✅ Error handling
- ✅ Mappers
- ✅ Helper methods
- ❌ Don't test framework code (EF Core, ASP.NET)
- ❌ Don't test trivial getters/setters

### 11.3 Integration Testing

```csharp
public class UserRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    
    public UserRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
    }
    
    [Fact]
    public async Task GetAllUsers_ReturnsUsersFromDatabase()
    {
        // Arrange
        var repository = new UserRepository(_context, _genericService);
        
        // Act
        var users = await repository.GetAllUsers(CancellationToken.None);
        
        // Assert
        Assert.NotEmpty(users);
    }
}
```

### 11.4 Test Project Structure

```
ImageLinks®.Tests/
├── Unit/
│   ├── Services/
│   │   ├── IdentityServiceTests.cs
│   │   └── UserServiceTests.cs
│   ├── Mappers/
│   │   └── UserDtoMapperTests.cs
│   └── Helpers/
│       └── EncryptionTests.cs
│
├── Integration/
│   ├── Repositories/
│   │   └── UserRepositoryTests.cs
│   └── API/
│       └── UserControllerTests.cs
│
└── Fixtures/
    ├── DatabaseFixture.cs
    └── TestDataBuilder.cs
```

---

## 12. Deployment Considerations

### 12.1 Environment Configuration

#### Development (appsettings.Development.json)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### Production (appsettings.json)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "#{ConnectionString}#",  // Token replacement
    "DefaultConnection_ProviderName": "#{DatabaseProvider}#"
  },
  "JwtSettings": {
    "Secret": "#{JwtSecret}#",
    "TokenExpirationInMinutes": 60,
    "Issuer": "#{JwtIssuer}#",
    "Audience": "#{JwtAudience}#"
  }
}
```

### 12.2 Security Checklist

- [ ] Use strong JWT secret (minimum 256 bits)
- [ ] Enable HTTPS in production
- [ ] Implement rate limiting
- [ ] Use secure connection strings (Azure Key Vault, etc.)
- [ ] Enable CORS with specific origins
- [ ] Implement request validation
- [ ] Use parameterized queries (prevent SQL injection)
- [ ] Encrypt sensitive data at rest
- [ ] Log security events
- [ ] Regular dependency updates

### 12.3 Performance Optimization

#### API Level
- Enable response compression
- Use response caching where appropriate
- Implement ETag support
- Configure connection pooling

```csharp
// Program.cs
builder.Services.AddResponseCompression();
builder.Services.AddResponseCaching();

app.UseResponseCompression();
app.UseResponseCaching();
```

#### Database Level
- Add appropriate indexes
- Use compiled queries for frequently executed queries
- Implement connection pooling
- Monitor slow queries

```csharp
// Example: Compiled query
private static readonly Func<ApplicationDbContext, int, Task<User?>> GetUserByIdQuery =
    EF.CompileAsyncQuery((ApplicationDbContext context, int id) =>
        context.Users.FirstOrDefault(u => u.UserId == id));
```

### 12.4 Monitoring and Logging

#### Logging Levels
- **Trace**: Very detailed logs (disabled in production)
- **Debug**: Detailed flow of application (disabled in production)
- **Information**: General informational messages
- **Warning**: Indicates potential issues
- **Error**: Errors and exceptions
- **Critical**: Critical failures

#### What to Log
```csharp
// Log important business events
_logger.LogInformation("User {UserId} logged in successfully", user.UserId);

// Log errors with context
_logger.LogError(ex, "Failed to process document {DocumentId}", documentId);

// Log performance issues
using (_logger.BeginScope("Processing batch {BatchId}", batchId))
{
    _logger.LogWarning("Batch processing took {Duration}ms", duration);
}
```

#### Structured Logging
```csharp
_logger.LogInformation(
    "User {UserId} performed {Action} on {EntityType} {EntityId}",
    userId,
    "Update",
    "Document",
    documentId
);
```

---

## Summary

This architecture provides:

1. **Clear Separation of Concerns**: Each layer has distinct responsibilities
2. **Testability**: Business logic is isolated and easy to test
3. **Maintainability**: Changes are localized and predictable
4. **Scalability**: Services can be scaled independently
5. **Flexibility**: Easy to swap implementations (e.g., databases, external services)
6. **Database Agnosticism**: Support for multiple database providers
7. **Security**: JWT authentication and encrypted passwords
8. **Error Handling**: Consistent error handling using Result pattern
9. **Performance**: Dual approach (EF Core + Dapper) for optimal performance

---

## Quick Reference Checklist

When adding a new feature, follow this checklist:

- [ ] Create domain entity in Domain layer
- [ ] Create DTO in Application layer
- [ ] Create request/response models
- [ ] Define repository interface
- [ ] Implement repository in Infrastructure layer
- [ ] Create entity mapper (if using Dapper)
- [ ] Create DTO mapper
- [ ] Define service interface
- [ ] Implement service with business logic
- [ ] Create controller
- [ ] Register dependencies in Program.cs
- [ ] Add DbSet to ApplicationDbContext (if using EF Core)
- [ ] Write unit tests
- [ ] Document endpoints
- [ ] Test manually with Swagger

---

**Document Version**: 1.0  
**Last Updated**: 2025  
**Maintained By**: ImageLinks® Development Team
