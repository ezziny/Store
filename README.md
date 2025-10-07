# 🛍️ Store API

A comprehensive e-commerce API built with ASP.NET Core and .NET 9, featuring a clean architecture with advanced caching, authentication, and payment processing capabilities.

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [API Endpoints](#-api-endpoints)
- [Authentication](#-authentication)
- [Caching](#-caching)
- [Payment Integration](#-payment-integration)
- [Database](#-database)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)

## Features

- **Product Management**: Full CRUD operations for products, brands, and categories
- **User Authentication**: JWT-based authentication with ASP.NET Core Identity
- **Shopping Cart**: Redis-based shopping cart with real-time persistence
- **Order Management**: Complete order processing with payment integration
- **Payment Processing**: Stripe integration for secure payment handling
- **Caching**: Redis caching with custom cache attributes for improved performance
- **Response Handling**: Standardized API responses with custom exception middleware
- **Data Seeding**: Automatic database seeding with sample data
- **Clean Architecture**: Repository pattern with Unit of Work implementation
- **AutoMapper**: Object-to-object mapping for DTOs
- **Specification Pattern**: Advanced querying with specifications
- **Swagger Documentation**: Interactive API documentation

## Architecture

This project follows Clean Architecture principles with clear separation of concerns:

```
├── Store.Data/          # Data models and database context
├── Store.Repository/    # Data access layer with repositories
├── Store.Services/      # Business logic and application services
└── Store.Web/          # API controllers and web configuration
```

## Tech Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core
- **Caching**: Redis (StackExchange.Redis)
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **Payments**: Stripe API integration
- **Documentation**: Swagger/OpenAPI
- **Mapping**: AutoMapper
- **Validation**: Data Annotations

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [Stripe Account](https://stripe.com) (for payment processing)

## Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/ezziny/Store.git
   cd store-api
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Set up databases**
   ```bash
   # Create main database migration
   dotnet ef migrations add "InitialCreate" --project Store.Data --startup-project Store.Web --context StoreDbContext
   
   # Create identity database migration
   dotnet ef migrations add "AddedIdentity" --project Store.Data --startup-project Store.Web --context StoreIdentityDbContext
   
   # Update databases
   dotnet ef database update --project Store.Data --startup-project Store.Web --context StoreDbContext
   dotnet ef database update --project Store.Data --startup-project Store.Web --context StoreIdentityDbContext
   ```

4. **Start Redis server**
   ```bash
   redis-server
   ```

5. **Run the application**
   ```bash
   dotnet run --project Store.Web
   ```

The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## Configuration

Update the `appsettings.json` file in the `Store.Web` project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=StoreDb;Trusted_Connection=true;",
    "IdentityConnection": "Server=.;Database=StoreIdentityDb;Trusted_Connection=true;",
    "Redis": "localhost"
  },
  "Token": {
    "Key": "your-super-secret-jwt-key-here",
    "Issuer": "your-issuer-here"
  },
  "Stripe": {
    "PublishableKey": "your-stripe-publishable-key",
    "SecretKey": "your-stripe-secret-key",
    "EndpointSecret": "your-webhook-endpoint-secret"
  }
}
```

> **Security Note**: Never commit sensitive keys to version control. Use `dotnet user-secrets` for local development and environment variables for production.

## API Endpoints

### Authentication
- `POST /api/account/register` - Register new user
- `POST /api/account/login` - User login
- `GET /api/account/current` - Get current user details

### Products
- `GET /api/products` - Get all products (with caching)
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/brands` - Get all brands
- `GET /api/products/types` - Get all product types

### Shopping Cart
- `GET /api/basket/{id}` - Get basket by ID
- `POST /api/basket` - Create/Update basket
- `DELETE /api/basket/{id}` - Delete basket

### Orders
- `POST /api/order` - Create new order
- `GET /api/order` - Get user orders
- `GET /api/order/{id}` - Get order by ID
- `GET /api/order/delivery-methods` - Get delivery methods

### Payments
- `POST /api/payment/create-update-intent` - Create/Update payment intent
- `POST /api/payment/webhook` - Stripe webhook endpoint

## Authentication

The API uses JWT Bearer token authentication:

1. **Register/Login** to receive a JWT token
2. **Include the token** in the Authorization header:
   ```
   Authorization: Bearer <your-jwt-token>
   ```

Protected endpoints require valid authentication tokens.

## Caching

The application implements Redis caching with a custom `[Cache]` attribute:

```csharp
[HttpGet]
[Cache(15)] // Cache for 15 minutes
public async Task<ActionResult<IReadOnlyList<ProductDetailsDto>>> GetAllProducts([FromQuery] ProductSpecification input)
```

Cache keys are automatically generated based on request paths and query parameters.

## Payment Integration

Stripe integration provides secure payment processing:

- **Payment Intents**: Automatic creation and updates
- **Webhooks**: Real-time payment status updates
- **Security**: Webhook signature verification
- **Currency**: USD support (configurable)

## Database

The application uses two separate databases:

- **StoreDb**: Product catalog, orders, and business data
- **StoreIdentityDb**: User authentication and identity data

### Sample Data

The application automatically seeds sample data including:
- Product brands and types
- Sample products with images
- Delivery methods

## Project Structure

```
Store.sln
├── Store.Data/                    # Data Layer
│   ├── Entities/                  # Domain entities
│   │   ├── Product.cs
│   │   ├── ProductBrand.cs
│   │   ├── Basket/               # Basket entities (Redis-only)
│   │   ├── OrderEntities/        # Order-related entities
│   │   └── IdentityEntities/     # User entities
│   ├── Contexts/                  # Database contexts
│   │   ├── StoreDbContext.cs
│   │   └── StoreIdentityDbContext.cs
│   └── Migrations/               # EF Core migrations
│
├── Store.Repository/             # Data Access Layer
│   ├── Interfaces/               # Repository contracts
│   ├── Repositories/             # Repository implementations
│   ├── Specifications/           # Query specifications
│   └── SeedData/                # JSON seed data
│
├── Store.Services/              # Business Logic Layer
│   ├── Services/                # Application services
│   │   ├── Product/            # Product services
│   │   ├── BasketService/      # Shopping cart logic
│   │   ├── OrderService/       # Order processing
│   │   ├── UserService/        # User management
│   │   ├── PaymentService/     # Stripe integration
│   │   ├── TokenService/       # JWT token generation
│   │   └── CacheService/       # Redis caching
│   └── HandleResponses/        # Custom response models
│
└── Store.Web/                  # Presentation Layer
    ├── Controllers/            # API controllers
    ├── Extensions/             # Service extensions
    ├── Middlewares/           # Custom middleware
    ├── Helper/                # Utility classes
    └── Program.cs             # Application entry point
```

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is not licensed but let's pretend it is lmfao.

## Related Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Redis Documentation](https://redis.io/documentation)
- [Stripe API Documentation](https://stripe.com/docs/api)

---

**Happy Coding!**
