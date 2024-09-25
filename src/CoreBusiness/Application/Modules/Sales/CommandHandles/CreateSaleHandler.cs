using Application.Abstractions.Handlers;
using Application.Abstractions.Ports;
using Application.Contracts;
using Domain.Modules.Sales.Aggregates;
using Domain.Modules.Sales.Entitites.Domain.Entities;

namespace Application.Modules.Sales.CommandHandles
{
    public class CreateSaleHandler : CommandHandler<Command.CreateSaleCommand>
    {
        private readonly ISaleRepository _repository;
        private readonly IServiceBusPublisher _serviceBusPublisher;

        public CreateSaleHandler(ISaleRepository repository, IServiceBusPublisher serviceBusPublisher)
        {
            _repository = repository;
            _serviceBusPublisher = serviceBusPublisher;
        }

        public override async Task Handle(Command.CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var saleItems = command.Items.Select(item => new SaleItem(
                productName: item.ProductName,
                quantity: item.Quantity,
                unitPrice: item.UnitPrice)
            ).ToList();

            var sale = new Sale(command.SaleDate, saleItems);

            await _repository.AddSaleAsync(sale);

            await _serviceBusPublisher.RegisterSaleCreatedAsync(sale);
        }
    }
}
