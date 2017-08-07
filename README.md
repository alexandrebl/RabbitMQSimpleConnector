# RabbitMQSimpleConnector
RabbitMQ simple connector


```cs
using System;
using RabbitMQSimpleConnector.Entity;

namespace RabbitMQSimpleConnector.ExampleOfUse {
    class Program {
        static void Main() {
            var connectionconfig = new ConnectionConfig() {
                HostName = "10.152.20.103",
                Password = "mudar123...#",
                UserName = "dnauth",
                VirtualHost = "dnauth"
            };

            var queueManager = new QueueManager<Aluno>(connectionconfig, "queueTest", 100);

            queueManager.WatchInit();

            queueManager.ReceiveMessage += (aluno, deliveryTag) => {
                Console.WriteLine($"Nome: {aluno.Nome} | Matricula: {aluno.Matricula} | DeliveryTag: {deliveryTag}");

                queueManager.Ack(deliveryTag);
            };

            for (var index = 1; index <= 1000; index++) {
                queueManager.Publish(new Aluno() {
                    Nome = "Slash",
                    Matricula = $"{index}"
                });
            }
            Console.ReadKey();
        }
    }

    public class Aluno {
        public string Nome { get; set; }
        public string Matricula { get; set; }
    }
}
```
