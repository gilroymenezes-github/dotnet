// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using KafkaConsumer;

Console.WriteLine("Hello, Kafka Consumer!");

var consumer = new Consumer<Message>();
await consumer.ConsumeAsync();
