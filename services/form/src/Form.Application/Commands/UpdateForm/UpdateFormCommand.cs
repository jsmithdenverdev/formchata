using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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
    private readonly IDynamoDBContext _context;

    public UpdateFormCommandHandler(ILogger<UpdateFormCommandHandler> logger, IDynamoDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<string?> Handle(UpdateFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        var formEntity = await _context.LoadAsync<FormMeta>(command.Id);

        if (formEntity == null)
        {
            // TODO: Replace this with a custom exception. FormMetaNotFoundException();
            return string.Empty;
        }

        // Update Form
        formEntity.Title = command.Form.Title;
        formEntity.Description = command.Form.Description;

        // Update Sections
        foreach (var updatedSection in command.Form.Sections)
        {
            // If an Id exists this is an existing section that needs to be updated
            if (!string.IsNullOrEmpty(updatedSection.Id))
            {
                var section = formEntity.Sections.FirstOrDefault(s => s.Id == updatedSection.Id);
                if (section != null)
                {
                    // Update Controls
                    section.Title = updatedSection.Title;
                    section.Description = updatedSection.Description;

                    foreach (var updatedControl in updatedSection.Controls)
                    {
                        if (!string.IsNullOrEmpty(updatedControl.Id))
                        {
                            var control = section.Controls.FirstOrDefault(c => c.Id == updatedControl.Id);
                            if (control != null)
                            {
                                // Only Title, Description and Required may be updated on controls.
                                // The underlying Control Type may not be modified after the control has been created.
                                control.Title = updatedControl.Title;
                                control.Description = updatedControl.Description;
                                control.Required = updatedControl.Required;

                                continue;
                            }

                            section.Controls.Add(updatedControl);
                            continue;
                        }

                        // If no Id exists this is a new Control that must be appended
                        updatedControl.Id = Guid.NewGuid().ToString();
                        section.Controls.Add(updatedControl);
                    }

                    continue;
                }

                // This will be hit if we have a section with an Id that matches no existing sections. Our default
                // behavior will be to add the section as a brand new section.
                formEntity.Sections.Add(updatedSection);
                continue;
            }

            // If no Id exists this is a new Section that must be appended
            updatedSection.Id = Guid.NewGuid().ToString();
            formEntity.Sections.Add(updatedSection);
        }

        // This assumes that SaveAsync is an upsert (it is for DynamoDB operations).
        await _context.SaveAsync(formEntity);

        return command.Id;
    }
}