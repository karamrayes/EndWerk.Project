using EndWerk.Project.Data;
using Microsoft.EntityFrameworkCore;
using Order.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Repository>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDB")));

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderDetailsService>();
builder.Services.AddScoped<OrderService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
