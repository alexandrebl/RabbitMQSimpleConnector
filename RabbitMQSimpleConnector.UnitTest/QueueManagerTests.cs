using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQSimpleConnector.Entity;

namespace RabbitMQSimpleConnector.UnitTest {
    [TestClass]
    public class QueueManagerTests {
        private QueueManager<Aluno> _queueManager;
        private Aluno _actual;

        [TestInitialize]
        public void TestInitialize() {
            var connectionSetting = new ConnectionSetting() {
                HostName = "127.0.0.1",
                Password = "guest",
                UserName = "guest",
                VirtualHost = "/"
            };

            _queueManager = new QueueManager<Aluno>("queueTest")
                .WithConnectionSetting(connectionSetting)
                .WithProducer() //testar sem producer
                .WithConsumer(); //testar sem consumer

            _actual = new Aluno {
                Nome = "Edu",
                Matricula = $"{123456}"
            };
        }

        [TestCleanup]
        public void TestCleanup() {

            _queueManager.Dispose();
        }

        [TestMethod]

        public void Publish() {
            bool ok;
            try {
                _queueManager.Producer?.Publish(_actual);
                ok = true;
            } catch (System.Exception) {

                ok = false;
            }
            Assert.IsTrue(ok);
        }

        [TestMethod]
        public void ReceiveMessage() {

            if (_queueManager.Consumer != null) {

                _queueManager.Consumer.OnReceiveMessageException += exception => {
                    Console.WriteLine(exception.Message);
                };

                _queueManager.Consumer.ReceiveMessage += (aluno, deliveryTag) => {

                    Assert.AreEqual(aluno.Matricula, _actual.Matricula);
                    Assert.AreEqual(aluno.Nome, _actual.Nome);
                    Assert.IsInstanceOfType(aluno, typeof(Aluno));

                    _queueManager.Consumer.Ack(deliveryTag);
                };

                _queueManager.Consumer.WatchInit();
            } else {
                Assert.IsTrue(true);
            }
        }
    }
}
