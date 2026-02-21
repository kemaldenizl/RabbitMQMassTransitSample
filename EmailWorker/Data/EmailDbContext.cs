using Microsoft.EntityFrameworkCore;

public class EmailDbContext : DbContext
{
    public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }

    public DbSet<ProcessedMessage> ProcessedMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcessedMessage>().HasKey(m => m.MessageId);
    }
}