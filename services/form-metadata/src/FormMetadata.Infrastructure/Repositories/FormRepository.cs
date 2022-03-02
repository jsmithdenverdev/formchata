using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FormMetadata.Application.Interfaces;
using FormMetadata.Domain.Entities;

namespace FormMetadata.Infrastructure.Repositories;

public class FormRepository : IFormRepository
{
    private readonly DynamoDBContext _context;

    public FormRepository(IAmazonDynamoDB client)
    {
        // TODO: Register context?
        _context = new DynamoDBContext(client);
    }

    public async Task Create(Form form)
    {
        await _context.SaveAsync(form);
    }

    public async Task<Form> Read(string id)
    {
        return await _context.LoadAsync<Form>(id);
    }

    public async Task Delete(string id)
    {
        await _context.DeleteAsync<Form>(id);
    }

    public async Task<string> Update(string id, Form form)
    {
        form.Id = id;

        await _context.SaveAsync(form);

        return id;
    }
}