using Application.Abstractions.Ports;
using Dapper;
using Domain.Modules.Sales.Aggregates;
using Domain.Modules.Sales.Entitites.Domain.Entities;
using Npgsql;

namespace Infrastructure.PostgreSql.Database
{
    public class SaleRepository : ISaleRepository
    {
        private readonly NpgsqlConnection _connection;

        public SaleRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        private async Task<IEnumerable<Sale>> GetSalesAsync(Guid? id = null)
        {
            var sql = @"
                SELECT s.id, s.sale_date AS SaleDate, s.total_amount AS TotalAmount, s.is_deleted AS IsDeleted, 
                       si.id AS Id, si.product_name AS ProductName, si.quantity AS Quantity, si.unit_price AS UnitPrice, si.is_deleted AS IsDeleted
                FROM sale s
                LEFT JOIN sale_item si ON s.id = si.sale_id
                WHERE s.is_deleted = false AND si.is_deleted = false";

            if (id.HasValue) sql += " AND s.id = @Id";

            var saleDictionary = new Dictionary<Guid, Sale>();

            var sales = await _connection.QueryAsync<Sale, SaleItem, Sale>(
                sql,
                (sale, item) =>
                {
                    if (!saleDictionary.TryGetValue(sale.Id, out var saleEntry))
                    {
                        saleEntry = sale;
                        saleEntry.Items = new List<SaleItem>();
                        saleDictionary.Add(sale.Id, saleEntry);
                    }

                    if (item != null)
                    {
                        saleEntry.Items.Add(item);
                    }

                    return saleEntry;
                },
                id.HasValue ? new { Id = id.Value } : null,
                splitOn: "Id"
            );

            return saleDictionary.Values;
        }

        public async Task<IEnumerable<Sale>> GetAllSalesAsync()
        {
            return await GetSalesAsync();
        }

        public async Task<Sale> GetSaleByIdAsync(Guid id)
        {
            var sales = await GetSalesAsync(id);
            return sales?.FirstOrDefault();
        }


        public async Task AddSaleAsync(Sale sale)
        {
            string query = "INSERT INTO sale (id, sale_date, total_amount, is_deleted) VALUES (@Id, @SaleDate, @TotalAmount, @IsDeleted)";
            await _connection.ExecuteAsync(query, sale);
            foreach (var item in sale.Items)
            {
                string itemQuery = "INSERT INTO sale_item (id, sale_id, product_name, quantity, unit_price, is_deleted) VALUES (@Id, @SaleId, @ProductName, @Quantity, @UnitPrice, @IsDeleted)";
                await _connection.ExecuteAsync(itemQuery, new { item.Id, SaleId = sale.Id, item.ProductName, item.Quantity, item.UnitPrice, item.IsDeleted });
            }
        }

        public async Task UpdateSaleAsync(Sale sale)
        {
            string updateSaleQuery = "UPDATE sale SET sale_date = @SaleDate, total_amount = @TotalAmount WHERE id = @Id";
            await _connection.ExecuteAsync(updateSaleQuery, sale);

            string deleteSaleItemsQuery = "DELETE FROM sale_item WHERE sale_id = @SaleId";
            await _connection.ExecuteAsync(deleteSaleItemsQuery, new { SaleId = sale.Id });

            foreach (var item in sale.Items)
            {
                string updateSaleItemsQuery = "INSERT INTO sale_item (id, sale_id, product_name, quantity, unit_price) VALUES (@Id, @SaleId, @ProductName, @Quantity, @UnitPrice)";
                await _connection.ExecuteAsync(updateSaleItemsQuery, new { item.Id, SaleId = sale.Id, item.ProductName, item.Quantity, item.UnitPrice });
            }
        }

        public async Task DeleteSaleAsync(Guid id)
        {
            string query = "UPDATE sale SET is_deleted = true WHERE id = @Id";
            await _connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
