using FormMetadata.Domain.Entities;

namespace FormMetadata.Application.Interfaces;

public interface IFormRepository
{
    public Task Create(Form form);
    public Task<Form> Read(string id);
    public Task Delete(string id);
    public Task<string> Update(string id, Form form);
}