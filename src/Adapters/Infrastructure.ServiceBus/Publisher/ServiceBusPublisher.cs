using Application.Abstractions.Ports;
using Azure.Messaging.ServiceBus;
using Domain.Modules.Sales.Aggregates;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.ServiceBus.Publisher
{
    public class ServiceBusPublisher : IServiceBusPublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSettings _serviceBusSettings;
        ServiceBusSender _sender;

        public ServiceBusPublisher(IOptions<ServiceBusSettings> serviceBusSettings)
        {
            _serviceBusSettings = serviceBusSettings.Value;
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            _serviceBusClient = new ServiceBusClient(_serviceBusSettings.ConnectionString, clientOptions);
            _sender = _serviceBusClient.CreateSender(_serviceBusSettings.RegisterSaleCreatedQueue);
        }

        public async Task RegisterSaleCreatedAsync(Sale sale)
        {
            // Serialize the event as a message body
            var messageBody = JsonConvert.SerializeObject(sale);
            var message = new ServiceBusMessage(messageBody)
            {
                ContentType = "application/json"
            };

            // Send the message to the queue
            await _sender.SendMessageAsync(message);
        }
    }
}
