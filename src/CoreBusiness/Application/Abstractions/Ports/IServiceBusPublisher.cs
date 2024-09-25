using Domain.Modules.Sales.Aggregates;

namespace Application.Abstractions.Ports
{
    public interface IServiceBusPublisher
    {
        Task RegisterSaleCreatedAsync(Sale sale);
    }
}
