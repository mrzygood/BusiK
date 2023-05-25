using System.Text.Json.Serialization;
using BusiK.ValueObjects;

namespace BusiK.Publishers;

public sealed class MessageEnvelope<TMessage> where TMessage : class
{
    public MessageEnvelope(TMessage message, Urn urn)
    {
        Message = message;
        Urn = urn.Value;
    }
    
    [JsonConstructor]
    public MessageEnvelope(TMessage message, string urn)
    {
        Message = message;
        Urn = new Urn(urn).Value;
    }

    public TMessage Message { get; set; }
    public string Urn { get; set; }
}

public sealed record MessageUrnEnvelope(string Urn);
