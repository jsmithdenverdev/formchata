using System.Text.Json;
using FormMetadata.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FormMetadata.Application.Commands.DeleteForm;

public class DeleteFormCommand : ICommand
{
    public string Id { get; set; }
}

public class DeleteFormCommandHandler : ICommandHandler<DeleteFormCommand, string>
{
    private readonly ILogger<DeleteFormCommandHandler> _logger;
    private readonly IFormRepository _formRepository;

    public DeleteFormCommandHandler(ILogger<DeleteFormCommandHandler> logger, IFormRepository formRepository)
    {
        _logger = logger;
        _formRepository = formRepository;
    }

    public async Task<string> Handle(DeleteFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        await _formRepository.Delete(command.Id);

        return command.Id;
    }
}