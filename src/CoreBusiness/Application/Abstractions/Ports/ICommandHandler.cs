using Application.Abstractions.Contracts;

namespace Application.Abstractions.Ports
{
    public interface ICommandHandler<TCommand>
    where TCommand : ICommand
    {
        Task Handle(TCommand command, CancellationToken cancellationToken);
    }
}
