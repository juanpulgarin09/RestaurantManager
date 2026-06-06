# 🍽️ RestaurantManager API

Restaurant management system developed as the **Final Project** for the **Web Programming** course at the Instituto Tecnológico Metropolitano (ITM) — 2026.

---

## 👤 Author

| Name | GitHub |
|------|--------|
| Juan Pulgarin | [@juanpulgarin09](https://github.com/juanpulgarin09) |

---

## 📋 Description

**RestaurantManager** is a REST API for the complete administration of a restaurant. It allows managing restaurants, tables, customers, menu items, and reservations, with real business logic validations such as table availability, maximum capacity per table, and future dates.

The project applies the same architectural patterns covered in class with the **SportsLeague** project, following an N-Layer architecture with a clear separation of responsibilities between layers.

---

## 🛠️ Technologies Used

### Backend
| Technology | Version | Purpose |
|-----------|---------|---------|
| .NET | 10.0 | Main framework |
| C# | — | Programming language |
| ASP.NET Core Web API | 10.0 | REST API framework |
| Entity Framework Core | 9.0.5 | ORM — Code First with SQL Server |
| SQL Server | — | Database engine |
| AutoMapper | 12.0.1 | Automatic mapping between Entities and DTOs |
| Swagger / Swashbuckle | 6.5.0 | Interactive API documentation |

---

## 🏗️ Architecture

The solution follows an **N-Layer Architecture** with three independent projects, identical to the SportsLeague reference project:

```
RestaurantManager/
├── RestaurantManager.Domain       → Domain Layer (the brain)
├── RestaurantManager.DataAccess   → Data Layer (the storage)
└── RestaurantManager.API          → Presentation Layer (the window)
```

### Layer Responsibilities

**RestaurantManager.Domain** — Depends on no one:
- `Entities/` — Classes representing DB tables, all inheriting from `AuditBase`
- `Enums/` — Fixed value lists (TableStatus, ReservationStatus, MenuCategory)
- `Interfaces/Repositories/` — Contracts that DataAccess must implement
- `Interfaces/Services/` — Contracts consumed by the API layer
- `Services/` — Business logic and validations

**RestaurantManager.DataAccess** — Depends only on Domain:
- `Context/` — EF Core configuration and entity relationships
- `Repositories/` — Concrete implementations that communicate with SQL Server
- `Seeders/` — Automatic initial data loaded on startup
- `Migrations/` — Database change history

**RestaurantManager.API** — Depends on Domain and DataAccess:
- `Controllers/` — Receive HTTP requests and return JSON
- `DTOs/Request/` — Define what data enters the API
- `DTOs/Response/` — Define what data the API returns
- `Mappings/` — AutoMapper converts between DTOs and Entities
- `Program.cs` — Configuration and dependency injection

---

## 🗄️ Data Model

### Entities (8 tables)

| Entity | Description |
|--------|-------------|
| `Restaurant` | Restaurant data (name, address, phone, email) |
| `Table` | Tables with number, capacity, and status |
| `Customer` | Customers with name, unique email, and phone |
| `MenuItem` | Menu items with name, price, and category |
| `Reservation` | Reservations linking a customer to a table |
| `Order` | Order associated with a reservation (1:1) |
| `OrderItem` | Junction table for the N:M relationship between Order and MenuItem |
| `AuditBase` | Abstract base class with Id, CreatedAt, and UpdatedAt |

### Relationships

| Type | Description |
|------|-------------|
| **1:N** | `Restaurant` → `Tables` |
| **1:N** | `Customer` → `Reservations` |
| **1:N** | `Table` → `Reservations` |
| **1:1** | `Reservation` → `Order` |
| **N:M** | `Order` ↔ `MenuItem` via `OrderItem` |

### Enums

```csharp
TableStatus:       Available = 0 | Occupied = 1 | Reserved = 2
ReservationStatus: Pending = 0   | Confirmed = 1 | Cancelled = 2 | Completed = 3
MenuCategory:      Starter = 0   | MainCourse = 1 | Dessert = 2  | Beverage = 3
```

---

## 🚀 Setup Instructions

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) (Express, Developer, or LocalDB)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or VS Code

---

### 1️⃣ Clone the repository

```bash
git clone https://github.com/juanpulgarin09/RestaurantManager.git
cd RestaurantManager
```

---

### 2️⃣ Configure the connection string

Edit `RestaurantManager.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RestaurantManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> If you use SQL Server Express, replace `localhost` with `localhost\SQLEXPRESS`.

---

### 3️⃣ Apply migrations

In the **Package Manager Console** in Visual Studio with `RestaurantManager.DataAccess` as the default project:

```powershell
Update-Database -StartupProject RestaurantManager.API
```

> Migrations are already included in the repository.

---

### 4️⃣ Run the application

Press **F5** in Visual Studio or:

```bash
dotnet run --project RestaurantManager.API
```

> The **DataSeeder** runs automatically and populates the DB with test data if it is empty.

---

### 5️⃣ Open Swagger

```
https://localhost:7082/swagger
```

---

## 🌱 DataSeeder — Initial Data

| What | Count | Detail |
|------|-------|--------|
| Restaurant | 1 | La Terraza Gourmet — El Poblado, Medellín |
| Tables | 8 | Capacities of 2, 4, 6, and 8 people |
| Customers | 5 | Colombian test customers |
| Menu Items | 14 | Starters, main courses, desserts, and beverages |
| Reservations | 3 | With Confirmed and Pending statuses |
| Order | 1 | With 4 OrderItems (N:M relationship) |

The Seeder only acts if the database is empty.

---

## 📡 Available Endpoints

### Restaurants
| Method | Route | Description | HTTP |
|--------|-------|-------------|------|
| GET | `/api/Restaurants` | List restaurants | 200 |
| GET | `/api/Restaurants/{id}` | Get by ID | 200 / 404 |
| POST | `/api/Restaurants` | Create restaurant | 201 / 409 |
| PUT | `/api/Restaurants/{id}` | Update restaurant | 204 / 404 / 409 |
| DELETE | `/api/Restaurants/{id}` | Delete restaurant | 204 / 404 |

### Tables
| Method | Route | Description | HTTP |
|--------|-------|-------------|------|
| GET | `/api/Tables` | List tables | 200 |
| GET | `/api/Tables/{id}` | Get by ID | 200 / 404 |
| POST | `/api/Tables` | Create table | 201 / 404 / 409 |
| PUT | `/api/Tables/{id}` | Update table | 204 / 404 |
| DELETE | `/api/Tables/{id}` | Delete table | 204 / 404 |

### Customers
| Method | Route | Description | HTTP |
|--------|-------|-------------|------|
| GET | `/api/Customers` | List customers | 200 |
| GET | `/api/Customers/{id}` | Get by ID | 200 / 404 |
| POST | `/api/Customers` | Create customer | 201 / 409 |
| PUT | `/api/Customers/{id}` | Update customer | 204 / 404 / 409 |
| DELETE | `/api/Customers/{id}` | Delete customer | 204 / 404 |

### MenuItems
| Method | Route | Description | HTTP |
|--------|-------|-------------|------|
| GET | `/api/MenuItems` | List menu items | 200 |
| GET | `/api/MenuItems/{id}` | Get by ID | 200 / 404 |
| POST | `/api/MenuItems` | Create item | 201 / 409 |
| PUT | `/api/MenuItems/{id}` | Update item | 204 / 404 / 409 |
| DELETE | `/api/MenuItems/{id}` | Delete item | 204 / 404 |

### Reservations
| Method | Route | Description | HTTP |
|--------|-------|-------------|------|
| GET | `/api/Reservations` | List reservations with details | 200 |
| GET | `/api/Reservations/{id}` | Get by ID with details | 200 / 404 |
| POST | `/api/Reservations` | Create reservation | 201 / 404 / 409 |
| PUT | `/api/Reservations/{id}` | Update / cancel reservation | 204 / 404 / 409 |
| DELETE | `/api/Reservations/{id}` | Delete reservation | 204 / 404 |

---

## ✅ Business Logic Validations

### Restaurants
- The name must be unique in the system.

### Customers
- The email must be unique in the system.
- The name cannot be empty.

### MenuItems
- The name is required.
- The price must be greater than 0.

### Tables
- The capacity must be greater than 0.
- The restaurant it belongs to must exist.

### Reservations
- The reservation date must be in the future.
- The number of guests must be greater than 0.
- The customer must exist.
- The table must exist.
- The table must have `Available` status.
- The number of guests cannot exceed the table's capacity.
- When a reservation is created, the table automatically changes to `Reserved`.
- When cancelled or completed, the table automatically returns to `Available`.

---

## 📁 Repository Structure

```
RestaurantManager/
│
├── RestaurantManager.API/
│   ├── Controllers/
│   ├── DTOs/
│   │   ├── Request/
│   │   └── Response/
│   ├── Mappings/
│   ├── appsettings.json
│   └── Program.cs
│
├── RestaurantManager.Domain/
│   ├── Entities/
│   ├── Enums/
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   └── Services/
│   └── Services/
│
└── RestaurantManager.DataAccess/
    ├── Context/
    ├── Repositories/
    ├── Seeders/
    └── Migrations/
```

---

## 👤 Author

**Juan Pulgarin**
Software Design Student
Instituto Tecnológico Metropolitano — ITM
Professor: Carlos Díaz
Semester 2026-1
GitHub: [@juanpulgarin09](https://github.com/juanpulgarin09)
