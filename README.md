# 🚀 Auth1796 RESTful API

A robust, modular, and secure RESTful API built with .NET 8 and ASP.NET Core, designed for modern authentication, user management, and resource handling. The project follows best practices for REST, error handling, logging, and API documentation.

---

## 📚 Table of Contents
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [API Usage](#api-usage)
- [Authentication & Authorization](#authentication--authorization)
- [Logging](#logging)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Author / Contact](#author--contact)

---

## ✨ Features

- **RESTful API**: Follows REST principles, resource-based routing, and proper HTTP methods (GET, POST, PUT, DELETE).
- **Consistent Error Handling**: Centralized exception middleware returns structured error responses.
- **Authentication & Authorization**: JWT-based authentication, role-based access, and policy support.
- **User Management**: Endpoints for registration, login, password reset, email confirmation, and role assignment.
- **Logging**: Integrated Serilog for structured logging (console, file, and extensible sinks).
- **OpenAPI/Swagger**: Interactive API documentation and testing via Swagger UI.
- **Entity Framework Core**: Database access and migrations.
- **Extensible Architecture**: Clean separation of concerns (Core, Infrastructure, Host, Domain).
- **Middleware**: Custom middleware for logging, error handling, and request/response tracking.

---

## 🛠️ Technology Stack

- **.NET 8 / ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for data access
- **Serilog**: Structured logging (console, file, etc.)
- **Swashbuckle/Swagger**: API documentation and UI
- **Microsoft Identity**: User and role management
- **Other Libraries**: Ardalis.Specification, custom middleware, and helpers

---

## 🚦 Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or your configured database)
- (Optional) Visual Studio 2022+ or VS Code

### Installation

1. **Clone the repository:**
   ```sh
   git clone https://github.com/your-org/Auth1796.git
   cd Auth1796
   ```

2. **Restore dependencies:**
   ```sh
   dotnet restore
   ```

3. **Configure the database connection:**
   - ⚠️ **Update the `ConnectionStrings` section in `Host/appsettings.json` with your database connection string.**

4. **Run the application:**
   ```sh
   dotnet run --project Host
   ```
   - On first run, **database migrations will be applied automatically** and a few initial data records will be seeded into the database.

5. **Access the API:**
   - The API will be available at `https://localhost:5001` (or as configured).

---

## 📖 API Usage

### Swagger UI
- Access interactive API docs at: `https://localhost:5001/swagger`

### Example Endpoints
- `POST /api/v1/Account/Login` — Authenticate and receive JWT token.
- `POST /api/v1/Account/ForgotPassword` — Request password reset.
- `POST /api/v1/Account/ResetPassword` — Reset password with token.
- `GET /api/v1/Account/ConfirmEmail` — Confirm user email.
- `PUT /api/v1/Account/ChangePassword` — Change password (authenticated).
- `POST /api/v1/UserManagement` — Create new user (admin).
- `GET /api/v1/UserManagement/{id}` — Get user by ID.

### HTTP Methods & Resource Naming
- Uses standard RESTful conventions: plural resource names, verbs for actions, and proper status codes.

### Error Handling
- All errors return a consistent structure:
  ```json
  {
    "status": 400,
    "error": "ValidationError",
    "message": "Email is required."
  }
  ```

---

## 🔐 Authentication & Authorization

- **JWT Bearer Authentication**: Secure endpoints require a valid JWT in the `Authorization: Bearer <token>` header.
- **Role-Based Access**: Certain endpoints require specific roles or policies.
- **Open Endpoints**: Some endpoints (e.g., login, register) are marked with `[AllowAnonymous]`.

---

## 📋 Logging

- **Serilog** is configured for structured logging.
- Logs are written to console and file (see `appsettings.json` for configuration).
- Middleware logs requests, responses, and exceptions.
- Example log output:
  ```
  [2025-06-26 12:00:00 INF] HTTP POST /api/v1/Account/Login responded 200 in 120ms
  ```

---

## 🧪 Testing

- The solution is structured for unit and integration testing.
- To run tests (if present):
  ```sh
  dotnet test
  ```

---

## 🤝 Contributing

Contributions are welcome! Please fork the repository, create a feature branch, and submit a pull request. For major changes, open an issue first to discuss your proposal.

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## 👤 Author / Contact

- **Author:** Ahmad Abdulsalam
- **Contact:** Please open an issue or pull request on the repository for questions or support.

---

> _For more details, see the source code and Swagger UI. Update configuration and environment variables as needed for your deployment._