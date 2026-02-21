using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace EmailWorker
{
    public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly EmailDbContext _dbContext;

        public OrderCreatedConsumer(EmailDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var messageId = context.MessageId.GetValueOrDefault();
            bool isAlreadyProcessed = await _dbContext.ProcessedMessages.AnyAsync(m => m.MessageId == messageId);

            if (isAlreadyProcessed)
            {
                Console.WriteLine($"[IDEMPOTENT] {messageId} ID'li mesaj zaten işlenmiş! Atlanıyor...");
                return;
            }

            var data = context.Message;
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"[MAİL GÖNDERİLDİ] Alıcı: {data.CustomerEmail}");
            Console.WriteLine($"İçerik: {data.OrderId} numaralı {data.ProductName} siparişiniz başarıyla alınmıştır.");
            Console.WriteLine("--------------------------------------------------");

            _dbContext.ProcessedMessages.Add(new ProcessedMessage
            {
                MessageId = messageId,
                ProcessedAt = DateTime.UtcNow
            });
            
            await _dbContext.SaveChangesAsync();
        }
    }
}