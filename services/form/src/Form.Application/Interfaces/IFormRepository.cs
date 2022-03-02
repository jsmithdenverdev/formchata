using Form.Domain.Entities;

namespace Form.Application.Interfaces;

public interface IFormRepository
{
    public Task Create(FormMeta form);
    public Task<FormMeta> Read(string id);
    public Task Delete(string id);
    public Task<string> Update(string id, FormMeta form);
}