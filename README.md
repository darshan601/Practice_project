# Microservices Practice Project

A comprehensive .NET Core microservices architecture project demonstrating modern software development patterns including API Gateway, JWT authentication, message queuing, and Entity Framework Core.

## 🏗️ Architecture Overview

This project implements a microservices architecture using the following components:

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│  Client Apps    │    │  Ocelot Gateway  │    │  Authentication │
│                 │◄──►│    (Port 5004)   │◄──►│   Service       │
└─────────────────┘    └──────────────────┘    │   (Port 5001)   │
                              │                 └─────────────────┘
                              │
                    ┌─────────┼─────────┐
                    │         │         │
            ┌───────▼───┐ ┌──▼────┐ ┌──▼──────┐
            │Movie/Genre│ │ Task  │ │ Order   │
            │ Service   │ │Service│ │ Service │
            │(Port 5002)│ │       │ │(Port    │
            └───────────┘ └───────┘ │ 5003)   │
                   │                └─────────┘
                   │                     │
            ┌──────▼──────┐       ┌─────▼──────┐
            │ SQL Server  │       │  RabbitMQ  │
            │ Database    │       │            │
            └─────────────┘       └────────────┘
```

## 🔧 Technologies Used

- **.NET 8** - Modern cross-platform framework
- **Entity Framework Core** - ORM for database operations
- **Ocelot** - API Gateway for routing and load balancing
- **JWT Authentication** - Secure token-based authentication
- **RabbitMQ** - Message broker for asynchronous communication
- **SQL Server** - Relational database
- **AutoMapper** - Object-to-object mapping
- **Serilog** - Structured logging
- **Docker** - Containerization support
- **Swagger/OpenAPI** - API documentation

## 📋 Services Overview

### 1. 🔐 Authentication Service (Port: 5001)
**Purpose**: Handles user authentication and JWT token generation

**Features**:
- User registration and login
- JWT token generation and validation
- Role-based authentication
- Secure password handling

**Key Endpoints**:
- `POST /api/authentication/register` - User registration
- `POST /api/authentication/login` - User authentication

**Technologies**: Entity Framework Core, SQL Server, JWT

### 2. 🎬 Movie/Genre Service (EFCore) (Port: 5002)
**Purpose**: Comprehensive movie and genre management system

**Features**:
- CRUD operations for movies and genres
- Many-to-many relationship management
- Entity Framework Core with SQL Server
- AutoMapper for object mapping
- Memory caching support
- Message publishing to RabbitMQ

**Key Endpoints**:
- `GET /api/movie/get-all` - Retrieve all movies
- `POST /api/movie/add-movie` - Create new movie (Auth required)
- `PUT /api/movie/update/{id}` - Update movie (Auth required)
- `DELETE /api/movie/delete/{id}` - Delete movie (Auth required)
- `GET /api/genre/genres/` - Retrieve all genres
- `POST /api/genre/add-genre` - Create new genre (Auth required)

**Technologies**: Entity Framework Core, SQL Server, AutoMapper, RabbitMQ

### 3. ✅ Task Service (Port: Not specified in Gateway)
**Purpose**: Simple task management system with in-memory storage

**Features**:
- Task creation and retrieval
- In-memory repository pattern
- Simple CRUD operations

**Key Endpoints**:
- `GET /api/tasks/getAll` - Retrieve all tasks
- `POST /api/tasks/add` - Create new task

**Technologies**: In-memory repository, .NET Core Web API

### 4. 📦 Order Service (Port: 5003)
**Purpose**: Order processing with message queue integration

**Features**:
- RabbitMQ message consumption
- Background service for message processing
- Movie-related event handling
- Asynchronous message processing

**Key Endpoints**:
- `GET /api/order/get-message` - Simple message endpoint

**Technologies**: RabbitMQ, Background Services, Message Processing

### 5. 🚪 Ocelot API Gateway (Port: 5004)
**Purpose**: Single entry point for all microservices

**Features**:
- Request routing to appropriate microservices
- Rate limiting and throttling
- JWT authentication integration
- CORS support
- Request/Response transformation

**Configuration**:
- Routes all requests to appropriate downstream services
- Handles authentication for protected endpoints
- Implements rate limiting (1 request per 30 seconds for auth)

### 6. 📚 Shared Library
**Purpose**: Common functionality across services

**Features**:
- Global exception handling middleware
- API Gateway signature validation
- Structured logging with Serilog
- Service resilience patterns

**Components**:
- `GlobalException` - Centralized error handling
- `ListenToOnlyApiGateway` - Security middleware
- `SharedServiceContainer` - Dependency injection setup

### 7. 🔑 JWT Library
**Purpose**: JWT authentication utilities

**Features**:
- JWT token generation and validation
- Custom JWT authentication extension
- User account models
- Token handling utilities

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full version)
- RabbitMQ (can use Docker)
- Visual Studio 2022 or VS Code

### Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/darshan601/Practice_project.git
   cd Practice_project
   ```

2. **Setup RabbitMQ** (using Docker)
   ```bash
   cd OrderMicroservice
   docker-compose up -d
   ```

3. **Configure Connection Strings**
   
   Update `appsettings.json` in each service:
   
   **Authentication Service**:
   ```json
   {
     "ConnectionStrings": {
       "UserConnection": "Server=(localdb)\\mssqllocaldb;Database=UserDb;Trusted_Connection=true;"
     }
   }
   ```
   
   **Movie/Genre Service (EFCore)**:
   ```json
   {
     "ConnectionStrings": {
       "MovieConnection": "Server=(localdb)\\mssqllocaldb;Database=MovieDb;Trusted_Connection=true;"
     }
   }
   ```

4. **Run Database Migrations**
   ```bash
   # For Authentication Service
   cd Authentication
   dotnet ef database update
   
   # For Movie Service
   cd ../EFCore
   dotnet ef database update
   ```

5. **Build the Solution**
   ```bash
   dotnet build
   ```

6. **Run Services** (in separate terminals)
   ```bash
   # Authentication Service
   cd Authentication
   dotnet run
   
   # Movie/Genre Service
   cd ../EFCore
   dotnet run
   
   # Task Service
   cd ../TaskMicroservice
   dotnet run
   
   # Order Service
   cd ../OrderMicroservice
   dotnet run
   
   # API Gateway (start last)
   cd ../OcelotApiGateway
   dotnet run
   ```

### Service URLs
- **API Gateway**: https://localhost:5004
- **Authentication**: http://localhost:5001
- **Movie/Genre**: http://localhost:5002
- **Order Service**: http://localhost:5014
- **Task Service**: http://localhost:5047
- **Swagger UI**: Available at `/swagger` for each service

## 📖 API Usage Examples

### Authentication Flow

1. **Register a new user**:
   ```bash
   curl -X POST "https://localhost:5004/api/authentication/register" \
        -H "Content-Type: application/json" \
        -d '{
          "userName": "testuser",
          "email": "test@example.com",
          "password": "Password123!",
          "role": "User"
        }'
   ```

2. **Login to get JWT token**:
   ```bash
   curl -X POST "https://localhost:5004/api/authentication/login" \
        -H "Content-Type: application/json" \
        -d '{
          "email": "test@example.com",
          "password": "Password123!"
        }'
   ```

3. **Use JWT token for protected endpoints**:
   ```bash
   curl -X POST "https://localhost:5004/api/movie/add-movie" \
        -H "Authorization: Bearer YOUR_JWT_TOKEN" \
        -H "Content-Type: application/json" \
        -d '{
          "movieName": "The Matrix",
          "director": "Wachowski Sisters",
          "releaseYear": 1999,
          "genreIds": [1, 2]
        }'
   ```

## 🏛️ Design Patterns & Architecture Principles

### Implemented Patterns:
- **API Gateway Pattern** - Single entry point via Ocelot
- **Repository Pattern** - Data access abstraction
- **Dependency Injection** - Loose coupling and testability
- **Background Services** - Asynchronous message processing
- **Middleware Pattern** - Cross-cutting concerns
- **JWT Authentication** - Stateless authentication

### Microservices Principles:
- **Single Responsibility** - Each service has a focused purpose
- **Independent Deployment** - Services can be deployed separately
- **Database per Service** - Each service owns its data
- **Communication via HTTP/REST** - Service-to-service communication
- **Centralized Configuration** - API Gateway routing

## 🔒 Security Features

- **JWT Authentication** - Secure token-based authentication
- **API Gateway Validation** - Only gateway-routed requests accepted
- **Rate Limiting** - Prevents abuse and DDoS attacks
- **CORS Configuration** - Cross-origin request security
- **Input Validation** - Model validation on all endpoints

## 📊 Monitoring & Logging

- **Structured Logging** with Serilog
- **File-based Logging** with rolling intervals
- **Console and Debug Output** for development
- **Global Exception Handling** for error tracking

## 🧪 Testing

The project structure supports:
- Unit testing for individual services
- Integration testing for API endpoints
- End-to-end testing through the API Gateway

## 🔄 Message Flow (RabbitMQ)

1. Movie service publishes events to RabbitMQ
2. Order service consumes movie-related messages
3. Events include: `add-movie`, `update-movie`, `delete-movie`
4. Asynchronous processing with acknowledgments

## 📝 Development Notes

### Service Ports:
- Gateway: 5004 (HTTPS)
- Authentication: 5001 (HTTP)
- Movie/Genre: 5002 (HTTP)
- Order: 5014 (HTTP)  
- Task: 5047 (HTTP)

### Database Schema:
- **Users**: Authentication data
- **Movies**: Movie information with genres (many-to-many)
- **Genres**: Genre classifications

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📄 License

This project is for educational purposes and demonstrates microservices architecture patterns.

---

**Note**: This is a practice project demonstrating microservices architecture. For production use, consider additional patterns like service mesh, distributed tracing, and comprehensive monitoring solutions.
