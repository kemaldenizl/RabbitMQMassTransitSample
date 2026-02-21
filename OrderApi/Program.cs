using MassTransit;
using SharedModels;

var builder = WebApplication.CreateBuilder(args);

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

app.MapPost("/api/orders", async (IPublishEndpoint publishEndpoint) =>
{
    var orderEvent = new OrderCreatedEvent
    {
        OrderId = new Random().Next(1000, 9999),
        CustomerEmail = "test@example.com",
        ProductName = "Süper Bilgisayar"
    };
    
    await publishEndpoint.Publish(orderEvent);

    return Results.Ok(new { Message = "Sipariş başarıyla alındı ve kuyruğa gönderildi!", Order = orderEvent });
});

app.Run();