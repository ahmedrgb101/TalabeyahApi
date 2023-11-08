using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using TalabeyahTaskApi.Application.Common.Persistence;
using TalabeyahTaskApi.Domain.Common.Contracts;
using TalabeyahTaskApi.Infrastructure.Persistence.Context;

namespace TalabeyahTaskApi.Infrastructure.Persistence.Repository;
// Inherited from Ardalis.Specification's RepositoryBase<T>
public class ApplicationDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
  private readonly ApplicationDbContext _dbContext;
  public ApplicationDbRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

  public IQueryable<T> Entities => _dbContext.Set<T>();

    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
  protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        ApplySpecification(specification, false)
            .ProjectToType<TResult>();
}