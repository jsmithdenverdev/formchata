using System.Text.Json;
using Form.Application.Interfaces;
using Form.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Form.Application.Commands.UpdateForm;

public class UpdateFormCommand : ICommand
{
    public string Id { get; set; }
    public FormMeta Form { get; set; }
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