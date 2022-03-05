using System.Text.Json;
using Amazon.DynamoDBv2.DataModel;
using Form.Application.Interfaces;
using Form.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Form.Application.Commands.DeleteForm;

public class DeleteFormCommand : ICommand
{
    public string OwnerId { get; set; }
    public string Id { get; set; }
}

public class DeleteFormCommandHandler : ICommandHandler<DeleteFormCommand, string>
{
    private readonly ILogger<DeleteFormCommandHandler> _logger;
    private readonly IDynamoDBContext _context;

    public DeleteFormCommandHandler(ILogger<DeleteFormCommandHandler> logger, IDynamoDBContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<string?> Handle(DeleteFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        await _context.DeleteAsync<FormMeta>(command.OwnerId, command.Id);

        return command.Id;
    }
}