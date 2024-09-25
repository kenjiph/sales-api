using Application.Abstractions.Contracts;

namespace Application.Contracts
{
    public static class Command
    {
        public record CreateSaleCommand(DateTime SaleDate, List<CreateSaleItemCommand> Items) : Message, ICommand;
        public record CreateSaleItemCommand(string ProductName, int Quantity, decimal UnitPrice) : Message, ICommand;
        public record CreateSaleFromServiceBusCommand() : Message, ICommand;
        public record UpdateSaleCommand(Guid Id, DateTime SaleDate, List<UpdateSaleItemCommand> Items) : Message, ICommand;
        public record UpdateSaleItemCommand(string ProductName, int Quantity, decimal UnitPrice);
        public record DeleteSaleCommand(Guid Id) : Message, ICommand;
    }
}
