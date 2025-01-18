using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;
using Store.Infrastructure.Repositories.IRepositories;
using Store.Infrastructure.Models;
using Store.Infrastructure.Repositories;
using Store.Core.Application.Sarvice.ISarvice;
using Store.Core.Application.Sarvice;
using Store.Core.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<Supplier>), typeof(GenericRopository<Supplier>));
builder.Services.AddScoped(typeof(IGenericRepository<Product>), typeof(GenericRopository<Product>));
builder.Services.AddScoped(typeof(IGenericRepository<Order>), typeof(GenericRopository<Order>));
builder.Services.AddScoped(typeof(IGenericRepository<StockMovement>), typeof(GenericRopository<StockMovement>));

builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IStockMovementService, StockMovementSarvice>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IStockMovementRepository, StockMovementRepository>();


MappingConfig.RegisterMappings();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Dashboard}/{id?}");

app.Run();
