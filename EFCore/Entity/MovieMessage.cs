namespace EFCore.Entity;

public class MovieMessage
{
    public string MessageType { get; set; }

    public object Payload { get; set; }
}