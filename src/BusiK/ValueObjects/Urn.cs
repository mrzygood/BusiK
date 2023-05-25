using System.Text.RegularExpressions;

namespace BusiK.ValueObjects;

public sealed record Urn
{
    private static Regex UrnRegExp =>
        new ("^urn:[a-zA-Z0-9][a-zA-Z0-9-]{0,31}:[a-zA-Z0-9()+,.\\-:=@;$_!*'%\\/?#]+$");
    
    public static Urn FromMessageType(Type messageType)
    {
        return new Urn($"urn:message-type:{messageType.FullName}");
    }

    public Urn(string urn)
    {
        if (UrnRegExp.IsMatch(urn) is false)
        {
            throw new Exception();
        }
        
        Value = urn;
    }
    
    public string Value { get; private set; }

    public string GetMessageTypeFullName() => Value.Split(":")[2];
}
