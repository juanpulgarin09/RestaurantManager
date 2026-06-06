# 🍽️ RestaurantManager API

Sistema de gestión de restaurantes desarrollado como **Proyecto Final** de la asignatura **Programación Web** del Instituto Tecnológico Metropolitano (ITM) — 2026.

---

## 👤 Integrante

| Nombre | GitHub |
|--------|--------|
| Juan Pulgarin | [@juanpulgarin09](https://github.com/juanpulgarin09) |

---

## 📋 Descripción

**RestaurantManager** es una API REST para la administración completa de un restaurante. Permite gestionar restaurantes, mesas, clientes, ítems del menú y reservas, con validaciones de lógica de negocio reales como disponibilidad de mesas, capacidad máxima por mesa y fechas futuras.

El proyecto aplica los mismos patrones de arquitectura vistos en clase con el proyecto **SportsLeague**, siguiendo una arquitectura N-Layer con separación clara de responsabilidades entre capas.

---

## 🛠️ Tecnologías Usadas

### Backend
| Tecnología | Versión | Propósito |
|-----------|---------|-----------|
| .NET | 10.0 | Framework principal |
| C# | — | Lenguaje de programación |
| ASP.NET Core Web API | 10.0 | Framework para la API REST |
| Entity Framework Core | 9.0.5 | ORM — Code First con SQL Server |
| SQL Server | — | Motor de base de datos |
| AutoMapper | 12.0.1 | Mapeo automático entre Entidades y DTOs |
| Swagger / Swashbuckle | 6.5.0 | Documentación interactiva de la API |

---

## 🏗️ Arquitectura

La solución sigue una **Arquitectura N-Layer** con tres proyectos independientes, idéntica al proyecto de referencia SportsLeague:

```
RestaurantManager/
├── RestaurantManager.Domain       → Capa de Dominio (el cerebro)
├── RestaurantManager.DataAccess   → Capa de Datos (el almacenamiento)
└── RestaurantManager.API          → Capa de Presentación (la ventana)
```

### Responsabilidad de cada capa

**RestaurantManager.Domain** — No depende de nadie:
- `Entities/` — Clases que representan las tablas de la BD, todas heredando de `AuditBase`
- `Enums/` — Listas de valores fijos (TableStatus, ReservationStatus, MenuCategory)
- `Interfaces/Repositories/` — Contratos que DataAccess debe implementar
- `Interfaces/Services/` — Contratos que la capa API consume
- `Services/` — Lógica de negocio y validaciones

**RestaurantManager.DataAccess** — Depende solo de Domain:
- `Context/` — Configuración de EF Core y relaciones entre entidades
- `Repositories/` — Implementaciones concretas que hablan con SQL Server
- `Seeders/` — Datos iniciales automáticos al arrancar
- `Migrations/` — Historial de cambios de la base de datos

**RestaurantManager.API** — Depende de Domain y DataAccess:
- `Controllers/` — Reciben peticiones HTTP y devuelven JSON
- `DTOs/Request/` — Definen qué datos entran a la API
- `DTOs/Response/` — Definen qué datos devuelve la API
- `Mappings/` — AutoMapper convierte entre DTOs y Entidades
- `Program.cs` — Configuración e inyección de dependencias

---

## 🗄️ Modelo de Datos

### Entidades (8 tablas)

| Entidad | Descripción |
|---------|-------------|
| `Restaurant` | Datos del restaurante (nombre, dirección, teléfono, email) |
| `Table` | Mesas con número, capacidad y estado |
| `Customer` | Clientes con nombre, email único y teléfono |
| `MenuItem` | Ítems del menú con nombre, precio y categoría |
| `Reservation` | Reservas que vinculan un cliente con una mesa |
| `Order` | Pedido asociado a una reserva (1:1) |
| `OrderItem` | Tabla intermedia de la relación N:M entre Order y MenuItem |
| `AuditBase` | Clase base abstracta con Id, CreatedAt y UpdatedAt |

### Relaciones

| Tipo | Descripción |
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

## 🚀 Instrucciones para Ejecutar

### Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/sql-server) (Express, Developer o LocalDB)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o VS Code

---

### 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/juanpulgarin09/RestaurantManager.git
cd RestaurantManager
```

---

### 2️⃣ Configurar la cadena de conexión

Edita `RestaurantManager.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RestaurantManagerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Si usas SQL Server Express cambia `localhost` por `localhost\SQLEXPRESS`.

---

### 3️⃣ Aplicar migraciones

En el **Package Manager Console** de Visual Studio con `RestaurantManager.DataAccess` como proyecto por defecto:

```powershell
Update-Database -StartupProject RestaurantManager.API
```

> Las migraciones ya están incluidas en el repositorio.

---

### 4️⃣ Ejecutar la aplicación

Presiona **F5** en Visual Studio o:

```bash
dotnet run --project RestaurantManager.API
```

> El **DataSeeder** se ejecuta automáticamente y puebla la BD con datos de prueba si está vacía.

---

### 5️⃣ Abrir Swagger

```
https://localhost:7082/swagger
```

---

## 🌱 DataSeeder — Datos Iniciales

| Qué | Cantidad | Detalle |
|-----|----------|---------|
| Restaurante | 1 | La Terraza Gourmet — El Poblado, Medellín |
| Mesas | 8 | Capacidades de 2, 4, 6 y 8 personas |
| Clientes | 5 | Clientes colombianos de prueba |
| Ítems del menú | 14 | Starters, platos principales, postres y bebidas |
| Reservas | 3 | Con estados Confirmed y Pending |
| Pedido | 1 | Con 4 OrderItems (relación N:M) |

El Seeder solo actúa si la base de datos está vacía.

---

## 📡 Endpoints Disponibles

### Restaurants
| Método | Ruta | Descripción | HTTP |
|--------|------|-------------|------|
| GET | `/api/Restaurants` | Listar restaurantes | 200 |
| GET | `/api/Restaurants/{id}` | Obtener por ID | 200 / 404 |
| POST | `/api/Restaurants` | Crear restaurante | 201 / 409 |
| PUT | `/api/Restaurants/{id}` | Actualizar restaurante | 204 / 404 / 409 |
| DELETE | `/api/Restaurants/{id}` | Eliminar restaurante | 204 / 404 |

### Tables
| Método | Ruta | Descripción | HTTP |
|--------|------|-------------|------|
| GET | `/api/Tables` | Listar mesas | 200 |
| GET | `/api/Tables/{id}` | Obtener por ID | 200 / 404 |
| POST | `/api/Tables` | Crear mesa | 201 / 404 / 409 |
| PUT | `/api/Tables/{id}` | Actualizar mesa | 204 / 404 |
| DELETE | `/api/Tables/{id}` | Eliminar mesa | 204 / 404 |

### Customers
| Método | Ruta | Descripción | HTTP |
|--------|------|-------------|------|
| GET | `/api/Customers` | Listar clientes | 200 |
| GET | `/api/Customers/{id}` | Obtener por ID | 200 / 404 |
| POST | `/api/Customers` | Crear cliente | 201 / 409 |
| PUT | `/api/Customers/{id}` | Actualizar cliente | 204 / 404 / 409 |
| DELETE | `/api/Customers/{id}` | Eliminar cliente | 204 / 404 |

### MenuItems
| Método | Ruta | Descripción | HTTP |
|--------|------|-------------|------|
| GET | `/api/MenuItems` | Listar ítems del menú | 200 |
| GET | `/api/MenuItems/{id}` | Obtener por ID | 200 / 404 |
| POST | `/api/MenuItems` | Crear ítem | 201 / 409 |
| PUT | `/api/MenuItems/{id}` | Actualizar ítem | 204 / 404 / 409 |
| DELETE | `/api/MenuItems/{id}` | Eliminar ítem | 204 / 404 |

### Reservations
| Método | Ruta | Descripción | HTTP |
|--------|------|-------------|------|
| GET | `/api/Reservations` | Listar reservas con detalles | 200 |
| GET | `/api/Reservations/{id}` | Obtener por ID con detalles | 200 / 404 |
| POST | `/api/Reservations` | Crear reserva | 201 / 404 / 409 |
| PUT | `/api/Reservations/{id}` | Actualizar / cancelar reserva | 204 / 404 / 409 |
| DELETE | `/api/Reservations/{id}` | Eliminar reserva | 204 / 404 |

---

## ✅ Validaciones de Lógica de Negocio

### Restaurants
- El nombre debe ser único en el sistema.

### Customers
- El email debe ser único en el sistema.
- El nombre no puede estar vacío.

### MenuItems
- El nombre es obligatorio.
- El precio debe ser mayor a 0.

### Tables
- La capacidad debe ser mayor a 0.
- El restaurante al que pertenece debe existir.

### Reservations
- La fecha de reserva debe ser futura.
- El número de comensales debe ser mayor a 0.
- El cliente debe existir.
- La mesa debe existir.
- La mesa debe estar en estado `Available`.
- Los comensales no pueden superar la capacidad de la mesa.
- Al crear una reserva, la mesa cambia automáticamente a `Reserved`.
- Al cancelar o completar, la mesa vuelve automáticamente a `Available`.

---

## 📁 Estructura del Repositorio

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

## 👤 Autor

**Juan Pulgarin**  
Estudiante de Diseño de Software  
Instituto Tecnológico Metropolitano — ITM  
Profesor: Carlos Díaz  
Semestre 2026-1  
GitHub: [@juanpulgarin09](https://github.com/juanpulgarin09)
