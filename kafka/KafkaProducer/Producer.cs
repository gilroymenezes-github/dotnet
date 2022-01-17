using Confluent.Kafka;
using KafkaShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafkaProducer;
internal class Producer<T>
{
    readonly string? _host;
    readonly int _port;
    readonly string? _topic;

    public Producer()
    {
        _host = "localhost";
        _port = 9092;
        _topic = "producer_logs";
    }

    ProducerConfig GetProducerConfig()
    {
        return new ProducerConfig
        {
            BootstrapServers = $"{_host}:{_port}"
        };
    }

    public async Task ProducerAsync(T data)
    {
        using (var producer = new ProducerBuilder<Null, T>(GetProducerConfig())
            .SetValueSerializer(new CustomValueSerializer<T>())
            .Build())
        {
            await producer.ProduceAsync(_topic, new Message<Null, T> { Value = data });
            Console.WriteLine($"{data} published");
        }
    }
}

