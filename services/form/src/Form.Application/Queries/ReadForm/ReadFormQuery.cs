using Form.Application.Interfaces;
using Form.Domain.Entities;

namespace Form.Application.Queries.ReadForm;

public class ReadFormQuery : IQuery
{
    public string Id { get; set; }
}

public class ReadFormQueryHandler : IQueryHandler<ReadFormQuery, FormMeta>
{
    private readonly IFormRepository _repository;

    public ReadFormQueryHandler(IFormRepository repository)
    {
        _repository = repository;
    }

    public async Task<FormMeta> Handle(ReadFormQuery query)
    {
        return await _repository.Read(query.Id);
    }
}