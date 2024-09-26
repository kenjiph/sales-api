using Domain.Abstractions.Aggregates;
using Domain.Modules.Sales.Entitites.Domain.Entities;

namespace Domain.Modules.Sales.Aggregates
{
    public class Sale : AggregateRoot
    {
        public DateTime SaleDate { get; set; }
        public List<SaleItem> Items { get; set; }
        public decimal TotalAmount { get; private set; }

        public Sale() { }

        public Sale(DateTime saleDate, List<SaleItem> items, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            SaleDate = saleDate;
            Items = items ?? new List<SaleItem>();
            TotalAmount = Items.Sum(item => item.Amount);
        }

        public void AddItem(SaleItem item)
        {
            Items.Add(item);
            TotalAmount += item.Amount;
        }

        public void RemoveItem(Guid itemId)
        {
            var item = Items.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                Items.Remove(item);
                TotalAmount -= item.Amount;
            }
        }

        public void UpdateSaleDate(DateTime saleDate)
        {
            SaleDate = saleDate;
        }
        public void UpdateSaleItems(List<SaleItem> updatedItems)
        {
            Items = updatedItems;
            TotalAmount = Items.Sum(item => item.Amount);
        }
    }
}
