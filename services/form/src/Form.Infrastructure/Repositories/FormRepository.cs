using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Form.Application.Interfaces;
using Form.Domain.Entities;

namespace Form.Infrastructure.Repositories;

public class FormRepository : IFormRepository
{
    private readonly DynamoDBContext _context;

    public FormRepository(IAmazonDynamoDB client)
    {
        // TODO: Register context?
        _context = new DynamoDBContext(client);
    }

    public async Task Create(FormMeta form)
    {
        await _context.SaveAsync(form);
    }

    public async Task<FormMeta> Read(string id)
    {
        return await _context.LoadAsync<FormMeta>(id);
    }

    public async Task Delete(string id)
    {
        await _context.DeleteAsync<FormMeta>(id);
    }

    public async Task<string> Update(string id, FormMeta form)
    {
        form.Id = id;

        await _context.SaveAsync(form);

        return id;
    }
}