using Application.Abstractions.Handlers;
using Application.Abstractions.Ports;
using Application.Contracts;
using Domain.Modules.Sales.Aggregates;

namespace Application.Modules.Sales.QueryHandles
{
    public class GetSaleByIdHandler : QueryHandler<Query.GetSaleByIdQuery, Sale>
    {
        private readonly ISaleRepository _repository;

        public GetSaleByIdHandler(ISaleRepository repository)
            => _repository = repository;
        public override async Task<Sale> Handle(Query.GetSaleByIdQuery query, CancellationToken cancellationToken)
        {

            var sale = await _repository.GetSaleByIdAsync(query.Id);
            return sale;
        }
    }
}
