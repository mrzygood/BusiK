### MegaMessaging

Simple library doing part of the job the MassTransit is doing.
It generates the same RabbitMq exchanges and queues structure.
Every message class has it own exchange in format `<message_namespace>:<class_name>`.
Exchanges are created for consumers class names with 'kebaberized' format without `Consumer` appendix if name contain it.
Queues names are the same like for exchanges and bind to it.

Examples how to use it you can find in `samples` directory and project called `Ecommerce.Monolith.BusiK`.
