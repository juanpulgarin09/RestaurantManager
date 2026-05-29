using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;

namespace RestaurantManager.DataAccess.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(RestaurantDbContext context)
    {
        // Aplica migraciones pendientes automáticamente
        await context.Database.MigrateAsync();

        if (await context.Restaurants.AnyAsync()) return; // Ya hay datos

        // --- Restaurante ---
        var restaurant = new Restaurant
        {
            Name = "La Terraza Gourmet",
            Address = "Calle 10 # 43-25, Medellín",
            Phone = "604-555-0101",
            Email = "contacto@laterrazagourmet.com"
        };
        await context.Restaurants.AddAsync(restaurant);
        await context.SaveChangesAsync();

        // --- Mesas ---
        var tables = new List<Table>
        {
            new() { Number = 1, Capacity = 2, Status = TableStatus.Available, RestaurantId = restaurant.Id },
            new() { Number = 2, Capacity = 4, Status = TableStatus.Available, RestaurantId = restaurant.Id },
            new() { Number = 3, Capacity = 4, Status = TableStatus.Reserved,  RestaurantId = restaurant.Id },
            new() { Number = 4, Capacity = 6, Status = TableStatus.Available, RestaurantId = restaurant.Id },
            new() { Number = 5, Capacity = 8, Status = TableStatus.Occupied,  RestaurantId = restaurant.Id },
        };
        await context.Tables.AddRangeAsync(tables);
        await context.SaveChangesAsync();

        // --- Clientes ---
        var customers = new List<Customer>
        {
            new() { FullName = "Carlos Gómez",   Email = "carlos@email.com",  Phone = "300-111-2233" },
            new() { FullName = "María Restrepo",  Email = "maria@email.com",   Phone = "301-222-3344" },
            new() { FullName = "Andrés Lopera",   Email = "andres@email.com",  Phone = "302-333-4455" },
        };
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();

        // --- Menú ---
        var menuItems = new List<MenuItem>
        {
            new() { Name = "Ensalada César",      Description = "Lechuga romana, crutones y aderezo César", Price = 18000, Category = MenuCategory.Starter,    IsAvailable = true },
            new() { Name = "Sopa del día",         Description = "Sopa casera según temporada",               Price = 15000, Category = MenuCategory.Starter,    IsAvailable = true },
            new() { Name = "Filete de Res",        Description = "Filete de 250g con papas y vegetales",      Price = 55000, Category = MenuCategory.MainCourse, IsAvailable = true },
            new() { Name = "Pollo a la Plancha",   Description = "Pechuga con arroz y ensalada",              Price = 38000, Category = MenuCategory.MainCourse, IsAvailable = true },
            new() { Name = "Pasta Carbonara",      Description = "Pasta con salsa carbonara y tocineta",      Price = 35000, Category = MenuCategory.MainCourse, IsAvailable = true },
            new() { Name = "Tiramisú",             Description = "Postre italiano clásico",                   Price = 20000, Category = MenuCategory.Dessert,    IsAvailable = true },
            new() { Name = "Jugo Natural",         Description = "Jugo de fruta de temporada",                Price = 10000, Category = MenuCategory.Beverage,   IsAvailable = true },
            new() { Name = "Agua con Gas",         Description = "Agua mineral con gas 500ml",                Price = 8000,  Category = MenuCategory.Beverage,   IsAvailable = true },
        };
        await context.MenuItems.AddRangeAsync(menuItems);
        await context.SaveChangesAsync();

        // --- Reservas ---
        var reservation1 = new Reservation
        {
            ReservationDate = DateTime.UtcNow.AddDays(1),
            GuestCount = 2,
            Status = ReservationStatus.Confirmed,
            Notes = "Anniversario, decoración especial",
            CustomerId = customers[0].Id,
            TableId = tables[0].Id
        };
        var reservation2 = new Reservation
        {
            ReservationDate = DateTime.UtcNow.AddDays(2),
            GuestCount = 4,
            Status = ReservationStatus.Pending,
            CustomerId = customers[1].Id,
            TableId = tables[1].Id
        };
        await context.Reservations.AddRangeAsync(reservation1, reservation2);
        await context.SaveChangesAsync();

        // --- Orden con ítems (N:M) ---
        var order = new Order
        {
            CreatedAt = DateTime.UtcNow,
            ReservationId = reservation1.Id,
            OrderItems = new List<OrderItem>
            {
                new() { MenuItemId = menuItems[0].Id, Quantity = 2, UnitPrice = menuItems[0].Price },
                new() { MenuItemId = menuItems[2].Id, Quantity = 2, UnitPrice = menuItems[2].Price },
                new() { MenuItemId = menuItems[6].Id, Quantity = 2, UnitPrice = menuItems[6].Price },
            }
        };
        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
    }
}