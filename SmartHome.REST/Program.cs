using Microsoft.EntityFrameworkCore;
using SmartHome.Infrastructure;
using SmartHome.Infrastructure.Models;
using SmartHome.Common;

var builder = WebApplication.CreateBuilder(args);

// 1. Підключення БД SQLite
builder.Services.AddDbContext<SmartHomeContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Реєстрація репозиторію та асинхронного CRUD-сервісу
builder.Services.AddScoped<IRepository<DeviceModel>, Repository<DeviceModel>>();
builder.Services.AddScoped<ICrudServiceAsync<DeviceModel>, DbCrudServiceAsync>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Автоматичне застосування міграцій при старті
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartHomeContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();