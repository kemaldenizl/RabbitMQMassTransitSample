using MassTransit;
using SharedModels;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("BaseConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.UseSqlite();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
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

    using var transaction = await context.Database.BeginTransactionAsync();
    try{
        await context.Orders.AddAsync(order);

        await publishEndpoint.Publish(orderEvent);

        await context.SaveChangesAsync();

        await transaction.CommitAsync();

        return Results.Created($"/api/orders/{order.Id}", order);
    }
    catch (Exception exception){
        await transaction.RollbackAsync();
        return Results.Problem("Sipariş işlenirken bir hata oluştu: " + exception.Message);
    }
});

app.MapGet("/api/orders/{id}", async (AppDbContext context, int id) =>
{
    var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
    return Results.Ok(order);
});

app.Run();