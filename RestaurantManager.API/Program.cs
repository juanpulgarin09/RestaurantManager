using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.DataAccess.Repositories;
using RestaurantManager.DataAccess.Seeders;
using RestaurantManager.Domain.Interfaces.Repositories;
using RestaurantManager.Domain.Interfaces.Services;
using RestaurantManager.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<RestaurantDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// ── Services ──
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── CORS para Angular ──
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Controllers y Swagger ──
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── DataSeeder automático ──
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<RestaurantDbContext>();

    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

// ── Middleware ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();