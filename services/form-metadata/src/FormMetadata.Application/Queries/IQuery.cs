namespace FormMetadata.Application.Queries;

public interface IQuery
{
}

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery
{
    public Task<TResult?> Handle(TQuery query);
}