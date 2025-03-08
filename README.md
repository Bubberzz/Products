# 📦 Products API
A RESTful Web API for managing products with **CQRS, MediatR, and Entity Framework Core**.

## 🚀 Features
✅ Retrieve a list of products  
✅ Retrieve a single product by ID  
✅ Create a new product  
✅ Update an existing product  
✅ Delete a product  

---

## 🔗 Endpoints

| Method   | Endpoint                  | Description                     |
|----------|---------------------------|---------------------------------|
| **GET**  | `/api/v1/products`        | Get a list of products         |
| **GET**  | `/api/v1/products/{id}`   | Get a single product by ID     |
| **POST** | `/api/v1/products`        | Create a new product           |
| **PUT**  | `/api/v1/products/{id}`   | Update an existing product     |
| **DELETE** | `/api/v1/products/{id}` | Delete a product               |

---

## 🛠️ Technology Stack

| Technology                     | Purpose                                       |
|---------------------------------|-----------------------------------------------|
| **.NET 8**                      | Core framework                               |
| **ASP.NET Core Web API**        | Web API framework                           |
| **Entity Framework Core**       | ORM for database access                     |
| **MediatR**                     | CQRS pattern implementation                 |
| **SQL Server / SQLite**         | Database support                            |
| **FluentValidation**            | Request validation                          |
| **AutoMapper**                  | Object mapping                              |
| **xUnit, Moq, FluentAssertions** | Unit testing                                |
| **WebApplicationFactory, In-Memory DB** | Integration testing                |

---

## 🏗️ Architectural Patterns & Best Practices

### 📌 **Clean Architecture**
- Separation of concerns with **Application, Domain, Infrastructure, and API** layers.
- **CQRS (Command Query Responsibility Segregation)** using MediatR.
- **Repository Pattern** for abstracting data access.

### 📌 **Middleware & API Enhancements**
- **Global Exception Handling Middleware** → Provides consistent error responses.
- **Request Logging Middleware** → Logs incoming requests and execution times.
- **API Versioning** → Ensuring backward compatibility.

### 📌 **Testing Strategy**
✅ **Unit Tests** → Covering Application Layer (**Commands, Queries, Handlers**)  
✅ **Integration Tests** → Validating API behavior with **WebApplicationFactory & In-Memory Database**  
✅ **Mocking with Moq** → Simulating dependencies (**Repositories, MediatR, Logging**)  

---
