using System.Text.Json;

namespace KafkaConsumer;

internal class Message
{
    public string Id { get; set; } 
    public string? Data { get; set; }
    public DateTime TimeStamp { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

