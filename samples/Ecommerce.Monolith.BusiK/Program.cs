using Ecommerce.Messages;
using BusiK;
using BusiK.Publishers;
using BusiK.Subscribers;
using Ecommerce.Monolith.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusiK(config =>
{
    config.AddConsumersAutoRegistration(builder.Services);
});

builder.Services.Scan(s => s.FromCallingAssembly()
    .AddClasses(c => c.AssignableTo(typeof(IConsumer<>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

var app = builder.Build();

app.MapGet("/", () => "Hello custom monolith!");
app.MapGet("/products/add", async (IMessagePublisher publisher, string name) =>
{
    Console.WriteLine($"Message '{nameof(ProductAdded)}' published");
    await publisher.PublishAsync(new ProductAdded(Guid.NewGuid(), name));
});
app.MapGet("/products/remove", async (IMessagePublisher publisher) =>
{
    Console.WriteLine($"Message '{nameof(ProductRemoved)}' published");
    await publisher.PublishAsync(new ProductRemoved(Guid.NewGuid()));
});
app.MapGet("/order/place", async (IMessagePublisher publisher) =>
{
    Console.WriteLine($"Message '{nameof(OrderPlaced)}' published");
    await publisher.PublishAsync(new OrderPlaced(Guid.NewGuid(), 2));
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
