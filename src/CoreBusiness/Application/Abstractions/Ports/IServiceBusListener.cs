namespace Application.Abstractions.Ports
{
    public interface IServiceBusListener
    {
        Task CreateSaleRequestAsync();
    }
}
