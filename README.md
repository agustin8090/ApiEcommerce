## E-commerce  RESTful API | Sales and Users Management
Wasup!
I built this End-to-End project with the goal of implementing the newest Technologies and be able of feel how it is to work with these like the current job market demands.

## 🌟 Overview
This project is scalable Backend solution for an E-commerce platform. Built with .NET 8, it implements modern architectural patterns and industry-standard security protocols.

Unlike standard academic projects, this API focuses on production-ready features such as database containerization, role-based access control, and optimized data mapping.

## 🚀 Key Features

# 👤 Identity & Security

* **Authentication & Authorization:** Implemented via ASP.NET Core Identity and JWT (JSON Web Tokens).

* **RBAC (Role-Based Access Control):** Granular permissions management with Admin and User roles.

* **Secure Password Hashing:** Leverages Identity's built-in security providers.


# 📦 Product & Inventory Management

* **Full CRUD Operations:** Comprehensive management for Products and Categories.

* **Advanced Data Handling:** Server-side Pagination and dynamic filtering to ensure high performance under load.

* **Stock Control:** Logic for inventory updates and purchase simulations.


# 🛠️ Technical Excellence

* **Clean Architecture:** Strict separation of concerns using the Repository Pattern.

* **High-Speed Mapping:** Strategically migrated from AutoMapper to Mapster for near-native performance and reduced boilerplate.

* **API Versioning:** Support for multiple API versions (v1, v2) to ensure backward compatibility.

* **Documentation:** Fully documented interactive UI via Swagger/OpenAPI.


## 🧩 Tech Stack & Tools

* **Languag:** C# 12

* **Framework:** ASP.NET Core Web API (.NET 8)

* **Database:** Microsoft SQL Server

* **ORM:** Entity Framework Core (Code First)

* **Containerization:** Docker & Docker-Compose (SQL Server instance)

* **Mapping:** Mapster

* **Version Control:** Git



## 🏗️ Architecture & Best Practices
The system is built with **Maintability** in mind:

* **Repository Pattern:** Decouples business logic from data access, making the codebase test-ready.

* **DTOs:** Ensures domain entities are never exposed directly, protecting the internal data structure.

* **Dependency Injection:** Managed via the native .NET IoC container for better resource lifecycle management.


## 🚦 How Start?

**Prerequisites:**
* https://dotnet.microsoft.com/download/dotnet/8.0

* https://www.docker.com/products/docker-desktop/

**Installation & Setup**

* Clone the repository: 
 "git clone https://github.com/agustin8090/ApiEcommerce.git"

* Spin up the Database (Docker):
 "docker-compose up -d"

* Update the Database:

 "dotnet ef database update"

* Run the API:

 "dotnet run"
 

## ✉️ Contact
I am a Software Engineering student and Data Analyst Intern passionate about building robust software solutions

* **LinkedIn:** https://www.linkedin.com/in/agustin-gonzalez-data/
* **Email:** agustingonzalez4371@gmail.com