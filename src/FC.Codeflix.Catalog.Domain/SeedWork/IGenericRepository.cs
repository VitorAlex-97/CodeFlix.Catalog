namespace FC.Codeflix.Catalog.Domain.SeedWork;
public interface IGenericRepository<TAggregate, TKey> : IRepository
{
    public Task Insert(TAggregate aggregate, CancellationToken cancellationToken);
    public Task<TAggregate> Get(TKey id, CancellationToken cancellationToken);
}