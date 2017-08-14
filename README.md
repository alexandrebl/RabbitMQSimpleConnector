# RabbitMQSimpleConnector
RabbitMQ Simple Connector

<strong>Develop branch</strong><br />
<img src="https://ci.appveyor.com/api/projects/status/github/alexandrebl/RabbitMQSimpleConnector?branch=develop&svg=true" alt="Project Badge" with="300">

<strong>Master branch</strong><br />
<img src="https://ci.appveyor.com/api/projects/status/github/alexandrebl/RabbitMQSimpleConnector?branch=master&svg=true" alt="Project Badge" with="300">

How to use:
Install-Package RabbitMQSimpleConnector

```cs
using System;
using RabbitMQSimpleConnector.Entity;

namespace RabbitMQSimpleConnector.ExampleOfUse {
    class Program {
        static void Main() {
            var connectionSetting = new ConnectionSetting() {
                HostName = "<IP Address>",
                Password = "<Password>",
                UserName = "<User Name>",
                VirtualHost = "<Virtual Host>"
            };

            queueManager = new QueueManager<Aluno>("queueTest")
                .WithConnectionSetting(connectionSetting)
                .WithProducer() 
                .WithConsumer();
			
			queueManager.Consumer.WatchInit();

			queueManager.Consumer.OnReceiveMessageException += (exception, deliveryTag) => {
                Console.WriteLine(exception.Message);
				queueManager.Consumer.Ack(deliveryTag);
            };

            queueManager.Consumer.ReceiveMessage += (aluno, deliveryTag) => {
                Console.WriteLine($"Nome: {aluno.Nome} | Matricula: {aluno.Matricula} | DeliveryTag: {deliveryTag}");

                queueManager.Consumer.Ack(deliveryTag);
            };

            for (var index = 1; index <= 1000; index++) {
                 queueManager.Producer?.Publish(new Aluno() {
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
