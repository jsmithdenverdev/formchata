using System.Text.Json;
using FormMetadata.Application.Interfaces;
using FormMetadata.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FormMetadata.Application.Commands.UpdateForm;

public class UpdateFormCommand : ICommand
{
    public string Id { get; set; }
    public Form Form { get; set; }
}

public class UpdateFormCommandHandler : ICommandHandler<UpdateFormCommand, string>
{
    private readonly ILogger<UpdateFormCommandHandler> _logger;
    private readonly IFormRepository _formRepository;

    public UpdateFormCommandHandler(ILogger<UpdateFormCommandHandler> logger, IFormRepository formRepository)
    {
        _logger = logger;
        _formRepository = formRepository;
    }

    public async Task<string?> Handle(UpdateFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        var id = await _formRepository.Update(command.Id, command.Form);

        return id;
    }
}