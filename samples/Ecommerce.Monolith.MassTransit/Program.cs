using Ecommerce.Messages;
using Ecommerce.Monolith.MassTransit;
using Ecommerce.Monolith.MassTransit.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransitIntegration(typeof(ProductModificationConsumer).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello orders!");
app.MapGet("/products/add", async (IPublishEndpoint publisher, string name) =>
{
    Console.WriteLine($"Message '{nameof(ProductAdded)}' published");
    await publisher.Publish(new ProductAdded(Guid.NewGuid(), name));
});
app.MapGet("/products/remove", async (IPublishEndpoint publisher) =>
{
    Console.WriteLine($"Message '{nameof(ProductRemoved)}' published");
    await publisher.Publish(new ProductRemoved(Guid.NewGuid()));
});
app.MapGet("/order/place", async (IPublishEndpoint publisher) =>
{
    Console.WriteLine($"Message '{nameof(OrderPlaced)}' published");
    await publisher.Publish(new OrderPlaced(Guid.NewGuid(), 2));
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
