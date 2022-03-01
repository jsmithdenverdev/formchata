using FormMetadata.Domain.Entities;

namespace FormMetadata.Application.Interfaces;

public interface IFormRepository
{
    public Task Create(Form form);
}