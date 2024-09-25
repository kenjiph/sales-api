using Application.Abstractions.Handlers;
using Application.Abstractions.Ports;
using Application.Contracts;

namespace Application.Modules.Sales.CommandHandles
{
    public class DeleteSaleHandler : CommandHandler<Command.DeleteSaleCommand>
    {
        private readonly ISaleRepository _repository;

        public DeleteSaleHandler(ISaleRepository repository)
            => _repository = repository;

        public override async Task Handle(Command.DeleteSaleCommand command, CancellationToken cancellationToken)
        {
            await _repository.DeleteSaleAsync(command.Id);
        }
    }
}
