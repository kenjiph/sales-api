using Application.Abstractions.Handlers;
using Application.Abstractions.Ports;
using Application.Contracts;
using Domain.Modules.Sales.Entitites.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Sales.CommandHandles
{
    public class UpdateSaleHandler : CommandHandler<Command.UpdateSaleCommand>
    {
        private readonly ISaleRepository _repository;

        public UpdateSaleHandler(ISaleRepository repository)
            => _repository = repository;

        public override async Task Handle(Command.UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var existingSale = await _repository.GetSaleByIdAsync(command.Id);
            if (existingSale == null)
            {
                throw new Exception("Sale not found.");
            }

            existingSale.UpdateSaleDate(command.SaleDate);
            existingSale.UpdateSaleItems(command.Items.Select(item => new SaleItem(
                productName: item.ProductName,
                quantity: item.Quantity,
                unitPrice: item.UnitPrice)
            ).ToList());

            await _repository.UpdateSaleAsync(existingSale);
        }
    }
}
