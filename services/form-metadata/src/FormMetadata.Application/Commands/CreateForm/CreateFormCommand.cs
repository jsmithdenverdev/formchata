using System.Text.Json;
using FormMetadata.Application.Interfaces;
using FormMetadata.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FormMetadata.Application.Commands.CreateForm;

public class CreateFormCommand : ICommand
{
    public Form Form { get; set; }
}

public class CreateFormCommandHandler : ICommandHandler<CreateFormCommand>
{
    private readonly ILogger<CreateFormCommandHandler> _logger;
    private readonly IFormRepository _formRepository;

    public CreateFormCommandHandler(ILogger<CreateFormCommandHandler> logger, IFormRepository formRepository)
    {
        _logger = logger;
        _formRepository = formRepository;
    }

    public async Task Handle(CreateFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        var id = new Guid().ToString();
        command.Form.Id = id;

        await _formRepository.Create(command.Form);
    }
}