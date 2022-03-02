using System.Text.Json;
using Form.Application.Interfaces;
using Form.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Form.Application.Commands.CreateForm;

public class CreateFormCommand : ICommand
{
    public FormMeta Form { get; set; }
}

public class CreateFormCommandHandler : ICommandHandler<CreateFormCommand, string>
{
    private readonly ILogger<CreateFormCommandHandler> _logger;
    private readonly IFormRepository _formRepository;

    public CreateFormCommandHandler(ILogger<CreateFormCommandHandler> logger, IFormRepository formRepository)
    {
        _logger = logger;
        _formRepository = formRepository;
    }

    public async Task<string?> Handle(CreateFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        var formId = Guid.NewGuid().ToString();
        command.Form.Id = formId;

        foreach (var section in command.Form.Sections)
        {
            var sectionId = Guid.NewGuid().ToString();
            section.Id = sectionId;

            foreach (var control in section.Controls)
            {
                var controlId = Guid.NewGuid().ToString();
                control.Id = controlId;
            }
        }

        await _formRepository.Create(command.Form);

        return formId;
    }
}