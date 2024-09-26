using Application.Abstractions.Ports;
using Azure.Messaging.ServiceBus;
using Domain.Modules.Sales.Aggregates;
using Domain.Modules.Sales.Entitites.Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.ServiceBus.Listener
{
    public class ServiceBusListener : IServiceBusListener
    {
        private readonly ServiceBusSettings _serviceBusSettings;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusProcessor _processor;
        private readonly ISaleRepository _saleRepository;
        private TaskCompletionSource<bool> _messageProcessed;

        public ServiceBusListener(IOptions<ServiceBusSettings> serviceBusSettings, ISaleRepository saleRepository)
        {
            _serviceBusSettings = serviceBusSettings.Value;
            var clientOptions = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            _serviceBusClient = new ServiceBusClient(_serviceBusSettings.ConnectionString, clientOptions);
            var processorOptions = new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1,
                PrefetchCount = 0
            };
            _processor = _serviceBusClient.CreateProcessor(_serviceBusSettings.CreateSaleRequestQueue, new ServiceBusProcessorOptions());
            _saleRepository = saleRepository;
            _messageProcessed = new TaskCompletionSource<bool>();
        }

        public async Task CreateSaleRequestAsync()
        {
            try
            {
                _processor.ProcessMessageAsync += MessageHandler;
                _processor.ProcessErrorAsync += ErrorHandler;

                await _processor.StartProcessingAsync();
                await _messageProcessed.Task;

                await _processor.StopProcessingAsync();
            }
            finally
            {
                await _processor.DisposeAsync();
                await _serviceBusClient.DisposeAsync();
            }
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var saleRequest = JsonConvert.DeserializeObject<Sale>(args.Message.Body.ToString());
            
            if (saleRequest != null)
            {
                var saleItems = saleRequest.Items.Select(item => new SaleItem(
                productName: item.ProductName,
                quantity: item.Quantity,
                unitPrice: item.UnitPrice)
                ).ToList();

                var sale = new Sale(saleRequest.SaleDate, saleItems);

                await _saleRepository.AddSaleAsync(sale);
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                await args.DeadLetterMessageAsync(args.Message, "Unable to deserialize Sale");
            }

            _messageProcessed.SetResult(true);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
