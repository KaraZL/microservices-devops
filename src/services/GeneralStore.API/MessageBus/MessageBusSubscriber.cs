using GeneralStore.API.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using System;
using RabbitMQ.Client.Events;
using System.Text;

namespace GeneralStore.API.MessageBus
{
    //Pas d'interface car on utilise BackgroundService, la classe doit fonctionner continuellement en arrière-plan
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _processor;
        private string _queueName;

        public IConnection _connection { get; private set; }
        public IModel _channel { get; private set; }

        //We can inject processor because it is also singleton
        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor processor)
        {
            _configuration = configuration;
            _processor = processor;

            InitializeRabbitMQ();
        }

        //ExecuteAsync (CancellationToken) est appelé pour exécuter le service d’arrière-plan.
        //L’implémentation retourne un Task qui représente la durée de vie totale du service d’arrière-plan. 
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            //Evenement reception message rabbitmq
            consumer.Received += (sender, e) => //Sender = ModuleHandle / e = ea
            {
                Console.WriteLine("--> Event Received !");

                //récupère le message
                var body = e.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                //on vérifie son event et on l'envoie dans la bdd
                _processor.ProcessEvent(notificationMessage);
            };

            //On continue à écouter rabbitmq
            Console.WriteLine("--> We are here ! Basic Consume");
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");

            Console.WriteLine($"--> Listening on the Message Bus : {_queueName}");

            //Ajouter un message (evenement) quand la connection est off
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}
