using Application.Abstractions.Ports;
using Application.Contracts;
using Application.Modules.Sales.CommandHandles;
using Application.Modules.Sales.QueryHandles;
using Domain.Modules.Sales.Aggregates;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationLayerDependency
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<Command.CreateSaleCommand>, CreateSaleHandler>();
            services.AddTransient<ICommandHandler<Command.CreateSaleFromServiceBusCommand>, CreateSaleFromServiceBusHandler>();
            services.AddTransient<ICommandHandler<Command.UpdateSaleCommand>, UpdateSaleHandler>();
            services.AddTransient<ICommandHandler<Command.DeleteSaleCommand>, DeleteSaleHandler>();

            services.AddTransient<IQueryHandler<Query.GetAllSalesQuery, IEnumerable<Sale>>, GetAllSalesHandler>();
            services.AddTransient<IQueryHandler<Query.GetSaleByIdQuery, Sale>, GetSaleByIdHandler>();
        }
    }
}
