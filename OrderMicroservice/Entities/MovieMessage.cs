namespace OrderMicroservice.Entities;

public class MovieMessage
{
    public string MessageType { get; set; }

    public object Payload { get; set; }
}