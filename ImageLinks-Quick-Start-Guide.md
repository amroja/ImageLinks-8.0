# ImageLinks® - Developer Quick Start Guide

## Welcome to the Team! 🚀

This guide will help you get up and running with the ImageLinks® project quickly. For detailed architectural information, refer to the **Architecture Documentation**.

---

## Prerequisites

Before you begin, ensure you have the following installed:

- [ ] **.NET 9.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/9.0)
- [ ] **Visual Studio 2022** (v17.14+) or **Visual Studio Code** with C# extension
- [ ] **SQL Server** (2019+) or **Oracle Database** (19c+)
- [ ] **Postman** or similar tool for API testing (optional)

---

## Getting Started


### 1. Configure Database Connection

Update `appsettings.json` in the **ImageLinks®.API** project:

**For SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ImageLinks_YourDB;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;",
    "DefaultConnection_ProviderName": "SQL"
  }
}
```

**For Oracle:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User ID=YOUR_USER;Password=YOUR_PASSWORD;Data Source=YOUR_DATASOURCE",
    "DefaultConnection_ProviderName": "ORACLE"
  }
}
```

### 2. Build and Run

```bash
cd ImageLinks®.API
dotnet run
```

The API should now be running at:
- HTTP: `http://localhost:5059`
- HTTPS: `https://localhost:7249`

### 6. Test the API

Open your browser and navigate to:
```
https://localhost:7249/openapi/v1.json
```

Or use Swagger UI (if configured).

---

## Project Structure Overview

```
ImageLinks® Solution
│
├── ImageLinks®.API                 ← Start here (entry point)
│   ├── Controllers/               ← API endpoints
│   ├── Middleware/                ← Exception handling, etc.
│   └── Program.cs                 ← Startup configuration
│
├── ImageLinks®.Application        ← Business logic
│   ├── Features/                  ← Organized by feature
│   │   ├── Users/
│   │   ├── Identity/
│   │   └── [YourFeature]/
│   ├── Common/                    ← Shared utilities
│   └── IRepository/               ← Repository interfaces
│
├── ImageLinks®.Infrastructure     ← Data access
│   ├── Features/                  ← Repository implementations
│   ├── Data/                      ← Database context
│   └── Repository/                ← Generic repositories
│
└── ImageLinks®.Domain             ← Core entities
    ├── Models/                    ← Database entities
    ├── Results/                   ← Result pattern
    └── Enums/                     ← Enumerations
```

---

## Your First Feature: Adding a "Product" Entity

Let's walk through adding a complete feature from start to finish.

### Step 1: Domain Entity

**File**: `ImageLinks®.Domain/Models/Product.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImageLinks_.Domain.Models;

[Table("PRODUCTS")]
public class Product
{
    [Key]
    [Column("PRODUCT_ID")]
    public decimal ProductId { get; set; }

    [Column("PRODUCT_NAME")]
    [MaxLength(100)]
    public string? ProductName { get; set; }

    [Column("PRODUCT_PRICE")]
    public decimal? ProductPrice { get; set; }

    [Column("IS_ACTIVE")]
    public bool IsActive { get; set; }
}
```

### Step 2: DTO

**File**: `ImageLinks®.Application/Features/Products/DTO/ProductDto.cs`

```csharp
namespace ImageLinks_.Application.Features.Products.DTO;

public record ProductDto(
    string ProductId,
    string ProductName,
    decimal Price,
    bool IsActive
);
```

### Step 3: Request Models

**File**: `ImageLinks®.Application/Features/Products/Requests/CreateProductRequest.cs`

```csharp
namespace ImageLinks_.Application.Features.Products.Requests;

public record CreateProductRequest(
    string ProductName,
    decimal Price
);
```

### Step 4: Repository Interface

**File**: `ImageLinks®.Application/Features/Products/IRepository/IProductRepository.cs`

```csharp
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Products.IRepository;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAllProducts(CancellationToken ct = default);
    Task<Product?> GetProductById(decimal id, CancellationToken ct = default);
}
```

### Step 5: Repository Implementation

**File**: `ImageLinks®.Infrastructure/Features/Products/Repository/ProductRepository.cs`

```csharp
using ImageLinks_.Application.Features.Products.IRepository;
using ImageLinks_.Application.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Infrastructure.Data;
using ImageLinks_.Infrastructure.Repository;

namespace ImageLinks_.Infrastructure.Features.Products.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IGenericRepository _genericService;

    public ProductRepository(ApplicationDbContext db, IGenericRepository genericService) 
        : base(db)
    {
        _db = db;
        _genericService = genericService;
    }

    public async Task<List<Product>> GetAllProducts(CancellationToken ct = default)
    {
        string sql = @"
            SELECT 
                PRODUCT_ID AS ProductId,
                PRODUCT_NAME AS ProductName,
                PRODUCT_PRICE AS ProductPrice,
                IS_ACTIVE AS IsActive
            FROM PRODUCTS
            WHERE IS_ACTIVE = 1";

        return await _genericService.GetListAsync<Product>(sql, null, null, ct);
    }

    public async Task<Product?> GetProductById(decimal id, CancellationToken ct = default)
    {
        return await Get(p => p.ProductId == id);
    }
}
```

### Step 6: Mapper

**File**: `ImageLinks®.Application/Features/Products/Mappers/ProductDtoMapper.cs`

```csharp
using ImageLinks_.Application.Features.Products.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.Products.Mappers;

public static class ProductDtoMapper
{
    public static ProductDto ToDto(this Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        return new ProductDto(
            product.ProductId.ToString(),
            product.ProductName ?? string.Empty,
            product.ProductPrice ?? 0,
            product.IsActive
        );
    }

    public static List<ProductDto> ToDtos(this IEnumerable<Product> products)
    {
        return [.. products.Select(p => p.ToDto())];
    }
}
```

### Step 7: Service Interface

**File**: `ImageLinks®.Application/Features/Products/Services/Interface/IProductService.cs`

```csharp
using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.Features.Products.DTO;
using ImageLinks_.Application.Features.Products.Requests;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Products.Services.Interface;

public interface IProductService
{
    Task<Result<List<ProductDto>>> GetAllProducts(CancellationToken ct);
    Task<Result<ProductDto>> GetProductById(decimal id, CancellationToken ct);
    Task<Result<ProductDto>> CreateProduct(CreateProductRequest request, CancellationToken ct);
}
```

### Step 8: Service Implementation

**File**: `ImageLinks®.Application/Features/Products/Services/Implementation/ProductService.cs`

```csharp
using ImageLinks_.Application.Features.Products.DTO;
using ImageLinks_.Application.Features.Products.IRepository;
using ImageLinks_.Application.Features.Products.Mappers;
using ImageLinks_.Application.Features.Products.Requests;
using ImageLinks_.Application.Features.Products.Services.Interface;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;
using ImageLinks_.Infrastructure.Data;

namespace ImageLinks_.Application.Features.Products.Services.Implementation;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _db;

    public ProductService(IProductRepository productRepository, ApplicationDbContext db)
    {
        _productRepository = productRepository;
        _db = db;
    }

    public async Task<Result<List<ProductDto>>> GetAllProducts(CancellationToken ct)
    {
        var products = await _productRepository.GetAllProducts(ct);
        return products.ToDtos();
    }

    public async Task<Result<ProductDto>> GetProductById(decimal id, CancellationToken ct)
    {
        var product = await _productRepository.GetProductById(id, ct);
        
        if (product == null)
            return Error.NotFound("ProductNotFound", $"Product with ID {id} not found");

        return product.ToDto();
    }

    public async Task<Result<ProductDto>> CreateProduct(CreateProductRequest request, CancellationToken ct)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(request.ProductName))
            return Error.Validation("InvalidName", "Product name is required");

        if (request.Price <= 0)
            return Error.Validation("InvalidPrice", "Product price must be positive");

        // Create entity
        var product = new Product
        {
            ProductName = request.ProductName,
            ProductPrice = request.Price,
            IsActive = true
        };

        // Save
        _productRepository.Add(product);
        await _db.SaveChangesAsync(ct);

        return product.ToDto();
    }
}
```

### Step 9: Controller

**File**: `ImageLinks®.API/Controllers/ProductController.cs`

```csharp
using ImageLinks_.Application.Features.Products.DTO;
using ImageLinks_.Application.Features.Products.Requests;
using ImageLinks_.Application.Features.Products.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageLinks_.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ApiController
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    [EndpointSummary("Get all active products")]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _productService.GetAllProducts(ct);
        return result.Match(
            response => Ok(response),
            Problem
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [EndpointSummary("Get product by ID")]
    public async Task<IActionResult> GetById(decimal id, CancellationToken ct)
    {
        var result = await _productService.GetProductById(id, ct);
        return result.Match(
            response => Ok(response),
            Problem
        );
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [EndpointSummary("Create a new product")]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request,
        CancellationToken ct)
    {
        var result = await _productService.CreateProduct(request, ct);
        return result.Match(
            response => CreatedAtAction(nameof(GetById), new { id = response.ProductId }, response),
            Problem
        );
    }
}
```

### Step 10: Register Services

**File**: `ImageLinks®.API/Program.cs`

Add these lines with the other service registrations:

```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
```

### Step 11: Update DbContext

**File**: `ImageLinks®.Infrastructure/Data/ApplicationDbContext.cs`

Add the DbSet:

```csharp
public DbSet<Product> PRODUCTS { get; set; }
```

### Step 12: Test Your Feature

1. **Run the application**
   ```bash
   dotnet run --project ImageLinks®.API
   ```

2. **Test with curl or Postman**

   **Get all products:**
   ```bash
   curl https://localhost:7249/api/product
   ```

   **Create a product** (requires authentication):
   ```bash
   curl -X POST https://localhost:7249/api/product \
     -H "Content-Type: application/json" \
     -H "Authorization: Bearer YOUR_TOKEN" \
     -d '{"productName":"Test Product","price":99.99}'
   ```

---

## Testing Your Code

### Authentication Flow

1. **Login to get a token:**
   ```bash
   curl -X POST https://localhost:7249/api/identity/login \
     -H "Content-Type: application/json" \
     -d '{"userName":"admin","password":"password123"}'
   ```

   Response:
   ```json
   {
     "accessToken": "eyJhbGc...",
     "expiresOnUtc": "2025-01-01T12:00:00Z"
   }
   ```

2. **Use the token in subsequent requests:**
   ```bash
   curl https://localhost:7249/api/user/da \
     -H "Authorization: Bearer eyJhbGc..."
   ```

---

## Troubleshooting

### Issue: "Connection to the database failed"

**Solution**: 
- Check your connection string in `appsettings.json`
- Ensure the database server is running
- Verify credentials

### Issue: "Unable to resolve service for type 'IXxxRepository'"

**Solution**: 
- Make sure you registered the service in `Program.cs`:
  ```csharp
  builder.Services.AddScoped<IXxxRepository, XxxRepository>();
  ```

### Issue: "Unauthorized" when calling protected endpoints

**Solution**: 
- Get a token by calling `/api/identity/login` first
- Include the token in the `Authorization` header:
  ```
  Authorization: Bearer YOUR_TOKEN
  ```

### Issue: Build errors related to nullable reference types

**Solution**: 
- Use `?` for nullable types: `public string? Name { get; set; }`
- Or initialize: `public string Name { get; set; } = string.Empty;`

---

## Useful Commands

```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build

# Run tests (when test project exists)
dotnet test

# Clean build artifacts
dotnet clean

# List installed packages
dotnet list package

# Add a package
dotnet add package PackageName

# Format code
dotnet format
```

---

## SVN Workflow


### Commit Message Guidelines

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `refactor`: Code refactoring
- `test`: Adding tests
- `chore`: Maintenance tasks

**Example:**
```
feat: add product management endpoints

- Added Product entity and DTO
- Implemented ProductService with CRUD operations
- Created ProductController with GET and POST endpoints
- Added repository and service registrations

Closes #123
```

---

## Development Best Practices

### ✅ DO:
- Follow the established folder structure
- Use the Result pattern for service methods
- Pass `CancellationToken` to async methods
- Use DTOs for API requests/responses
- Add XML documentation to public methods
- Write unit tests for business logic
- Use dependency injection
- Keep controllers thin (delegate to services)
- Use AsNoTracking() for read-only queries

### ❌ DON'T:
- Put business logic in controllers
- Access the database directly from controllers
- Use `async void` (use `async Task` instead)
- Block on async code with `.Result` or `.Wait()`
- Catch and swallow exceptions without logging
- Hardcode connection strings or secrets
- Commit `appsettings.Development.json` with real credentials

---

## Resources

### Internal Documentation
- **Architecture Documentation**: See `ImageLinks-Architecture-Documentation.md`
- **API Documentation**: Available at `/openapi/v1.json` when running

### External Resources
- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Dapper Documentation](https://github.com/DapperLib/Dapper)
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

---

## Getting Help

If you're stuck:

1. Check the **Architecture Documentation** for detailed guidelines
2. Review similar existing features for reference (e.g., Users, SysSettingDetail)
3. Ask team members on Slack/Teams
4. Consult the external resources listed above

---

## Next Steps

Now that you're set up:

1. ✅ Complete the "Product" feature tutorial above
2. ✅ Read the full **Architecture Documentation**
3. ✅ Review existing code in the Users feature
4. ✅ Set up your development environment preferences
5. ✅ Join the team communication channels
6. ✅ Attend the next team standup

Welcome to the team, and happy coding! 🎉

---

**Document Version**: 1.0  
**Last Updated**: 2025  
**Maintained By**: ImageLinks® Development Team
