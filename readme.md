🚀 RabbitMQ & MassTransit: Advanced Messaging Patterns
This repository demonstrates a robust microservices communication architecture using RabbitMQ and MassTransit in .NET. It covers essential patterns to ensure data consistency and reliability in distributed systems.

🌟 Key Features
The project is structured into different implementation stages to showcase the evolution of reliable messaging:

Pure API Implementation: Basic RabbitMQ integration using MassTransit for asynchronous communication.

Outbox Pattern: Ensures "Exactly-once" processing and prevents data loss by saving messages to the database before publishing.

Idempotent Consumer: Prevents duplicate message processing at the consumer side, ensuring system stability even if messages are retried.

Entity Framework Integration: Seamlessly integrated with SQLite for persistent storage and outbox state management.

🏗️ Architecture & Patterns
📥 Transactional Outbox Pattern

Implemented in the OrderApi, this pattern guarantees that a message is only sent to the broker if the local database transaction succeeds. This avoids the "dual-write" problem where a database update succeeds but the message fails to publish.

🛡️ Idempotent Consumer

The EmailWorker utilizes an idempotency check by tracking processed message IDs in the EmailDbContext. If the same message arrives multiple times, the consumer identifies it via the database and skips re-processing.

🛠️ Tech Stack
Runtime: .NET 8 / 9

Message Broker: RabbitMQ

Abstraction: MassTransit

Database: SQLite with Entity Framework Core

Patterns: Outbox, Idempotency, Publisher-Subscriber

📁 Project Structure
OrderApi: The producer service that creates orders and publishes OrderCreatedEvent.

EmailWorker: The consumer service that listens for events and handles email processing logic (simulated).

SharedModels: Contains the shared contracts and events used by both services.

🚀 Getting Started
Clone the repository:

Bash
git clone https://github.com/yourusername/rabbitmq-masstransit-sample.git
Run RabbitMQ:
Ensure you have RabbitMQ running locally or via Docker:

Bash
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
Update Database:
Run migrations for both OrderApi and EmailWorker:

Bash
dotnet ef database update
Run the Applications:
Start both the API and the Worker service to see the messaging in action.