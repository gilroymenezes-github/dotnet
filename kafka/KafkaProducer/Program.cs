// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using KafkaProducer;

Console.WriteLine("Hello, Kafka Producer!");

var producer = new Producer<Message>();

for (int i = 0; i < 25; i++)
{
    await producer.ProducerAsync(new Message
    {
        Data = $"Pushing Data {i} !!",
    });

    await Task.Delay(1000);
}

Console.WriteLine("Publish Success!");
Console.ReadKey();