namespace Domain.Modules.Sales.Entitites
{
    namespace Domain.Entities
    {
        public class SaleItem
        {
            public Guid Id { get; private set; }
            public string ProductName { get; set; } // Ensure this is public
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Amount => Quantity * UnitPrice; // Read-only property to calculate the amount
            public bool IsDeleted { get; set; }

            public SaleItem()
            {
                Id = Guid.NewGuid();
            }

            public SaleItem(string productName, int quantity, decimal unitPrice)
            {
                Id = Guid.NewGuid();
                ProductName = productName;
                Quantity = quantity;
                UnitPrice = unitPrice;
            }
        }
    }
}
