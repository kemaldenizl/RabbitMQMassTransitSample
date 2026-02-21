namespace SharedModels
{
    public record OrderCreatedEvent
    {
        public int OrderId { get; init; }
        public string CustomerEmail { get; init; }
        public string ProductName { get; init; }
    }
}