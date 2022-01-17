using System.Text.Json;

namespace KafkaProducer;

internal class Message
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Data { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

