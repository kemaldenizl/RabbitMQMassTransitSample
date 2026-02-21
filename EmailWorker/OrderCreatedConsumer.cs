using MassTransit;
using SharedModels;

namespace EmailWorker
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var data = context.Message;
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[MAİL GÖNDERİLDİ] Alıcı: {data.CustomerEmail}");
            Console.WriteLine($"İçerik: {data.OrderId} numaralı {data.ProductName} siparişiniz başarıyla alınmıştır.");
            Console.WriteLine("--------------------------------------------------");

            return Task.CompletedTask;
        }
    }
}