using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;

namespace RestaurantManager.DataAccess.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(RestaurantDbContext context)
    {
        // Solo ejecutar si la BD está vacía
        if (await context.Restaurants.AnyAsync()) return;

        // ═══ 1. RESTAURANTE ═══
        var restaurant = new Restaurant
        {
            Name = "La Terraza Gourmet",
            Address = "Calle 10 # 43-25, El Poblado, Medellín",
            Phone = "604-555-0101",
            Email = "contacto@laterrazagourmet.com"
        };
        await context.Restaurants.AddAsync(restaurant);
        await context.SaveChangesAsync();

        // ═══ 2. MESAS ═══
        var tables = new List<Table>
        {
            new() { Number=1, Capacity=2, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=2, Capacity=2, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=3, Capacity=4, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=4, Capacity=4, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=5, Capacity=6, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=6, Capacity=6, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=7, Capacity=8, Status=TableStatus.Available, RestaurantId=restaurant.Id },
            new() { Number=8, Capacity=8, Status=TableStatus.Available, RestaurantId=restaurant.Id },
        };
        await context.Tables.AddRangeAsync(tables);
        await context.SaveChangesAsync();

        // ═══ 3. CLIENTES ═══
        var customers = new List<Customer>
        {
            new() { FullName="Carlos Gómez",    Email="carlos.gomez@email.com",   Phone="300-111-2233" },
            new() { FullName="María Restrepo",   Email="maria.restrepo@email.com",  Phone="301-222-3344" },
            new() { FullName="Andrés Lopera",    Email="andres.lopera@email.com",   Phone="302-333-4455" },
            new() { FullName="Valentina Ríos",   Email="valentina.rios@email.com",  Phone="303-444-5566" },
            new() { FullName="Santiago Herrera", Email="santiago.herrera@email.com",Phone="304-555-6677" },
        };
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();

        // ═══ 4. MENÚ ═══
        var menuItems = new List<MenuItem>
        {
            // Starters
            new() { Name="Ensalada César",       Description="Lechuga romana, crutones y aderezo César",       Price=18000,  Category=MenuCategory.Starter,    IsAvailable=true  },
            new() { Name="Sopa del Día",          Description="Sopa casera según temporada",                    Price=15000,  Category=MenuCategory.Starter,    IsAvailable=true  },
            new() { Name="Tabla de Quesos",       Description="Selección de quesos con mermelada y galletas",   Price=32000,  Category=MenuCategory.Starter,    IsAvailable=true  },
            // Main Courses
            new() { Name="Filete de Res",         Description="Filete 250g con papas y vegetales asados",       Price=55000,  Category=MenuCategory.MainCourse, IsAvailable=true  },
            new() { Name="Pollo a la Plancha",    Description="Pechuga con arroz y ensalada fresca",            Price=38000,  Category=MenuCategory.MainCourse, IsAvailable=true  },
            new() { Name="Pasta Carbonara",       Description="Pasta con salsa carbonara y tocineta",           Price=35000,  Category=MenuCategory.MainCourse, IsAvailable=true  },
            new() { Name="Salmón al Horno",       Description="Salmón con puré de papa y espárragos",           Price=65000,  Category=MenuCategory.MainCourse, IsAvailable=true  },
            new() { Name="Risotto de Champiñones",Description="Arroz cremoso con champiñones y parmesano",      Price=40000,  Category=MenuCategory.MainCourse, IsAvailable=false },
            // Desserts
            new() { Name="Tiramisú",              Description="Postre italiano clásico con café",               Price=20000,  Category=MenuCategory.Dessert,    IsAvailable=true  },
            new() { Name="Cheesecake de Frutos",  Description="Cheesecake con salsa de frutos rojos",           Price=18000,  Category=MenuCategory.Dessert,    IsAvailable=true  },
            // Beverages
            new() { Name="Jugo Natural",          Description="Jugo de fruta de temporada 350ml",               Price=10000,  Category=MenuCategory.Beverage,   IsAvailable=true  },
            new() { Name="Agua con Gas",          Description="Agua mineral con gas 500ml",                     Price=8000,   Category=MenuCategory.Beverage,   IsAvailable=true  },
            new() { Name="Café Americano",        Description="Café negro preparado al momento",                Price=7000,   Category=MenuCategory.Beverage,   IsAvailable=true  },
            new() { Name="Copa de Vino Tinto",    Description="Vino tinto de la casa 150ml",                    Price=25000,  Category=MenuCategory.Beverage,   IsAvailable=true  },
        };
        await context.MenuItems.AddRangeAsync(menuItems);
        await context.SaveChangesAsync();

        // ═══ 5. RESERVAS ═══
        var reservation1 = new Reservation
        {
            ReservationDate = DateTime.UtcNow.AddDays(1),
            GuestCount = 2,
            Status = ReservationStatus.Confirmed,
            Notes = "Aniversario, decoración especial por favor",
            CustomerId = customers[0].Id,
            TableId = tables[0].Id
        };
        var reservation2 = new Reservation
        {
            ReservationDate = DateTime.UtcNow.AddDays(2),
            GuestCount = 4,
            Status = ReservationStatus.Confirmed,
            CustomerId = customers[1].Id,
            TableId = tables[2].Id
        };
        var reservation3 = new Reservation
        {
            ReservationDate = DateTime.UtcNow.AddDays(3),
            GuestCount = 6,
            Status = ReservationStatus.Pending,
            Notes = "Cumpleaños, traer pastel",
            CustomerId = customers[2].Id,
            TableId = tables[4].Id
        };

        // Marcar mesas como reservadas
        tables[0].Status = TableStatus.Reserved;
        tables[2].Status = TableStatus.Reserved;
        tables[4].Status = TableStatus.Reserved;

        await context.Reservations.AddRangeAsync(reservation1, reservation2, reservation3);
        await context.SaveChangesAsync();

        // ═══ 6. PEDIDO con ORDERITEMS (relación N:M) ═══
        var order = new Order
        {
            ReservationId = reservation1.Id,
            OrderItems = new List<OrderItem>
            {
                new() { MenuItemId=menuItems[0].Id, Quantity=2, UnitPrice=menuItems[0].Price  },
                new() { MenuItemId=menuItems[3].Id, Quantity=2, UnitPrice=menuItems[3].Price  },
                new() { MenuItemId=menuItems[8].Id, Quantity=2, UnitPrice=menuItems[8].Price  },
                new() { MenuItemId=menuItems[10].Id, Quantity=2, UnitPrice=menuItems[10].Price },
            }
        };
        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }
}