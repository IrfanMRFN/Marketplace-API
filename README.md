# Enterprise Marketplace API

A production-ready, highly scalable E-Commerce REST API built with **.NET 10** and **C# 14**. This project strictly adheres to **Clean Architecture** principles, ensuring a robust separation of concerns, maximum testability, and enterprise-grade security.

## 🏗️ Architecture Overview

This solution is divided into four distinct layers following Dependency Inversion principles:

* **1. Domain (`Marketplace.Domain`):** The absolute core of the system. Contains pure C# enterprise entities (`User`, `Product`, `Order`) with zero dependencies on external frameworks or databases.
* **2. Application (`Marketplace.Application`):** The business logic and use case layer. Contains strictly typed immutable DTOs (using modern C# `record` types) and Service Interfaces.
* **3. Infrastructure (`Marketplace.Infrastructure`):** Implements the application interfaces and handles all external communication, including Entity Framework Core, SQL Server interactions, and BCrypt password hashing.
* **4. API (`Marketplace.Api`):** The presentation layer and Composition Root. Handles HTTP requests via MVC Controllers, Dependency Injection routing, JWT Authentication middleware, and Rate Limiting.

## ✨ Key Features

* **Stateless Security:** JWT-based authentication and authorization with secure BCrypt password hashing.
* **Transactional Integrity:** Order processing utilizes Entity Framework Core transactions to ensure inventory is accurately deducted only when an order is successfully placed.
* **Relational Data Mapping:** Efficient SQL JOINs via EF Core `.Include()` for complex data retrieval (e.g., Order History).
* **DDoS Protection:** Built-in Fixed Window Rate Limiting (max 5 requests per 10 seconds) to protect critical endpoints.
* **Cross-Origin Ready:** Configured CORS policy specifically tailored for modern SPA frontends (like React/Vite).
* **Modern C# Features:** Utilizes C# 14 features including Primary Constructors, Collection Expressions (`[]`), Null-Forgiving operators, and `record` types for DTOs.

## 🛠️ Tech Stack

* **Framework:** .NET 10.0
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core 10
* **Security:** BCrypt.Net-Next, Microsoft.AspNetCore.Authentication.JwtBearer