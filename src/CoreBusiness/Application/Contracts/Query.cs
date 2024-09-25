using Application.Abstractions.Contracts;
namespace Application.Contracts
{
    public static class Query
    {
        public record GetAllSalesQuery() : IQuery;
        public record GetSaleByIdQuery(Guid Id) : IQuery;
    }
}
