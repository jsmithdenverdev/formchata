using Amazon.DynamoDBv2.DataModel;
using Form.Application.Interfaces;
using Form.Domain.Entities;

namespace Form.Application.Queries.ReadForm;

public class ReadFormQuery : IQuery
{
    public string Id { get; set; }
}

public class ReadFormQueryHandler : IQueryHandler<ReadFormQuery, FormMeta>
{
    private const string IndexName = "FormIdIndex";
    private readonly IDynamoDBContext _context;

    public ReadFormQueryHandler(IDynamoDBContext context)
    {
        _context = context;
    }

    public async Task<FormMeta?> Handle(ReadFormQuery query)
    {
        var queryConfig = new DynamoDBOperationConfig {IndexName = IndexName};
        var formQuery = _context.QueryAsync<FormMeta>(query.Id, queryConfig);
        var results = await formQuery.GetRemainingAsync();

        // The index formIndex uses the Id of the form as a partition key and has no sort key. This guarantees that
        // the result should have 0 or 1 elements. Because of that guarantee we can safely return first or default.
        return results.FirstOrDefault();
    }
}