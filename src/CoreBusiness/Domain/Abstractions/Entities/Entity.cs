namespace Domain.Abstractions.Entities;

public abstract class Entity : IEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; protected set; }
}
