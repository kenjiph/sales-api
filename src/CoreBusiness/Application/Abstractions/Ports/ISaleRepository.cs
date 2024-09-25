using Domain.Modules.Sales.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Ports
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetAllSalesAsync();
        Task<Sale> GetSaleByIdAsync(Guid id);
        Task AddSaleAsync(Sale sale);
        Task UpdateSaleAsync(Sale sale);
        Task DeleteSaleAsync(Guid id);
    }
}
