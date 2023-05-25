namespace BusiK.Subscribers;

public class ConsumeContext<TMessage> : IConsumeContext, IConsumeContext<TMessage> where TMessage : class
{
    public TMessage Message { get; set; }
    
    public ConsumeContext(TMessage message)
    {
        Message = message;
    }
}
