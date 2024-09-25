using Application.Abstractions.Handlers;
using Application.Abstractions.Ports;
using Application.Contracts;
using Domain.Modules.Sales.Aggregates;
using Domain.Modules.Sales.Entitites.Domain.Entities;
namespace Application.Modules.Sales.CommandHandles
{
    public class CreateSaleFromServiceBusHandler : CommandHandler<Command.CreateSaleFromServiceBusCommand>
    {
        private readonly ISaleRepository _repository;
        private readonly IServiceBusListener _serviceBusListener;

        public CreateSaleFromServiceBusHandler(ISaleRepository repository, IServiceBusListener serviceBusListener)
        {
            _repository = repository;
            _serviceBusListener = serviceBusListener;
        }

        public override async Task Handle(Command.CreateSaleFromServiceBusCommand command, CancellationToken cancellationToken)
        {
            await _serviceBusListener.CreateSaleRequestAsync();
        }
    }
}
