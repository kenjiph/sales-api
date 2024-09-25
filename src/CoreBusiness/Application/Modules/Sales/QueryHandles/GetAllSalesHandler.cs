using Application.Abstractions.Handlers;
using Application.Abstractions.Ports;
using Application.Contracts;
using Domain.Modules.Sales.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.Sales.QueryHandles
{
    public class GetAllSalesHandler : QueryHandler<Query.GetAllSalesQuery, IEnumerable<Sale>>
    {
        private readonly ISaleRepository _repository;

        public GetAllSalesHandler(ISaleRepository repository)
            => _repository = repository;
        public override async Task<IEnumerable<Sale>> Handle(Query.GetAllSalesQuery query, CancellationToken cancellationToken)
        {
            var sales = await _repository.GetAllSalesAsync();
            return sales;
        }
    }
}
