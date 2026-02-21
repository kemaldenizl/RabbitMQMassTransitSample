using MassTransit;
using SharedModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("BaseConnection")));

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();

app.MapPost("/api/orders", async (IPublishEndpoint publishEndpoint, AppDbContext context, Order order) =>
{
    var orderEvent = new OrderCreatedEvent
    {
        OrderId = order.Id,
        CustomerEmail = order.CustomerEmail,
        ProductName = order.ProductName
    };

    await context.Orders.AddAsync(order);
    await context.SaveChangesAsync();
    
    await publishEndpoint.Publish(orderEvent);

    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapGet("/api/orders/{id}", async (AppDbContext context, int id) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    return Results.Ok(order);
});

app.Run();