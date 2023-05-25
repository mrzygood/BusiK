namespace BusiK.Subscribers;

public interface IConsumeContext
{
}

public interface IConsumeContext<TMessage> where TMessage : class
{
    TMessage Message { get; set; }
}
