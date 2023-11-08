namespace TalabeyahTaskApi.Application.Common.Models;

public static class PaginationResponseExtensions
{
    public static async Task<PaginationResponse<TDestination>> PaginatedListAsync<T, TDestination>(
        this IReadRepositoryBase<T> repository, ISpecification<T, TDestination> spec, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        where T : class
        where TDestination : class, IDto
    {
        var list = await repository.ListAsync(spec, cancellationToken);
        int count = await repository.CountAsync(spec, cancellationToken);

        return new PaginationResponse<TDestination>(list, count, pageNumber, pageSize);
    }
    public static async Task<PaginationResponse<T>> ToPaginatedListAsync<T>(this IEnumerable<T> source, int pageNumber, int pageSize) where T : class
    {
        if (source == null) throw new Exception();
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 5 : pageSize;
        int count =   source.Count();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items =   source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginationResponse<T>(items, count, pageNumber, pageSize);
    }
    public static async Task<PaginationResponse<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize) where T : class
    {
        if (source == null) throw new Exception();
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 5 : pageSize;
        int count = source.Count();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginationResponse<T>(items, count, pageNumber, pageSize);
    }
}