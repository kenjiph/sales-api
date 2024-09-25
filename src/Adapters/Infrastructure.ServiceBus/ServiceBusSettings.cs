namespace Infrastructure.ServiceBus
{
    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string CreateSaleRequestQueue { get; set; }
        public string RegisterSaleCreatedQueue { get; set; }
    }

}
