# Enterprise Marketplace API

A production-ready, highly scalable E-Commerce REST API built with **.NET 10** and **C# 14**. This project strictly adheres to **Clean Architecture** principles, ensuring a robust separation of concerns, maximum testability, and enterprise-grade security.

## 🏗️ Architecture Overview

This solution is divided into four distinct layers following Dependency Inversion principles:

* **1. Domain (`Marketplace.Domain`):** The absolute core of the system. Contains pure C# enterprise entities (`User`, `Product`, `Order`) with zero dependencies on external frameworks or databases.
* **2. Application (`Marketplace.Application`):** The business logic and use case layer. Contains strictly typed immutable DTOs (using modern C# `record` types), Service Interfaces, and a unified `Result<T>` pattern.
* **3. Infrastructure (`Marketplace.Infrastructure`):** Implements the application interfaces and handles all external communication, including Entity Framework Core, SQL Server interactions, and BCrypt password hashing.
* **4. API (`Marketplace.Api`):** The presentation layer and Composition Root. Handles HTTP requests via MVC Controllers, Dependency Injection routing, JWT Authentication middleware, Global Exception Handling, and Rate Limiting.

## ✨ Key Features

* **Advanced Security & RBAC:** Stateless JWT-based authentication with secure BCrypt password hashing. Implements strict Role-Based Access Control (RBAC), locking down catalog management to authorized Administrators.
* **Data Integrity & Concurrency:** Utilizes Entity Framework Core with **Optimistic Concurrency** (`RowVersion`) to prevent database race conditions and overselling during high-volume stock purchases.
* **Result Pattern for Control Flow:** Eliminates computationally expensive exception-throwing for expected business logic failures. Utilizes a unified `Result<T>` wrapper for predictable, high-performance HTTP response mapping.
* **Global Exception Handling:** Centralized middleware implementing the modern `IExceptionHandler` to catch unexpected system faults, returning standardized RFC 7807 `ProblemDetails` without leaking sensitive server information.
* **Automated Data Seeding:** Securely provisions a master Administrator account directly into the database upon initial application startup.
* **Relational Data Mapping:** Efficient SQL JOINs via EF Core `.Include()` for complex data retrieval (e.g., Order History).
* **DDoS Protection:** Built-in Fixed Window Rate Limiting (max 5 requests per 10 seconds) to protect critical authentication endpoints.
* **Modern API Documentation:** Fully configured Swagger/OpenAPI UI built on the new .NET 10 document generation, featuring integrated JWT Bearer token authorization.

## 🛠️ Tech Stack

* **Framework:** .NET 10.0
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core 10
* **Security:** BCrypt.Net-Next, Microsoft.AspNetCore.Authentication.JwtBearer
* **Documentation:** Microsoft.AspNetCore.OpenApi, Swashbuckle v10