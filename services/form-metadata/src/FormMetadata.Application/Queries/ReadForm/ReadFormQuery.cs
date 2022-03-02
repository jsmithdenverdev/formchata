using FormMetadata.Application.Interfaces;
using FormMetadata.Domain.Entities;

namespace FormMetadata.Application.Queries.ReadForm;

public class ReadFormQuery : IQuery
{
    public string Id { get; set; }
}

public class ReadFormQueryHandler : IQueryHandler<ReadFormQuery, Form>
{
    private readonly IFormRepository _repository;

    public ReadFormQueryHandler(IFormRepository repository)
    {
        _repository = repository;
    }

    public async Task<Form> Handle(ReadFormQuery query)
    {
        return await _repository.Read(query.Id);
    }
}