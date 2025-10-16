using Zoo.Application.Interfaces;

namespace Zoo.Infrastructure.Persistence.InMemory;
public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync() => Task.CompletedTask;
}
