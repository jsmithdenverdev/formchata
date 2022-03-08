using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Form.Application.Exceptions;
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
    private readonly IAmazonDynamoDB _documentClient;

    public DeleteFormCommandHandler(ILogger<DeleteFormCommandHandler> logger, IAmazonDynamoDB documentClient)
    {
        _logger = logger;
        _documentClient = documentClient;
    }

    public async Task<string?> Handle(DeleteFormCommand command)
    {
        _logger.LogInformation($"Handling command {JsonSerializer.Serialize(command)}");

        var request = new DeleteItemRequest(
            "form",
            new Dictionary<string, AttributeValue>
            {
                {"ownerId", new AttributeValue(command.OwnerId)},
                {"formId", new AttributeValue(command.Id)}
            })
        {
            ReturnValues = ReturnValue.ALL_OLD
        };

        var response = await _documentClient.DeleteItemAsync(request);

        if (response.Attributes.Count == 0)
        {
            throw new FormMetaNotFoundException(command.Id, command.OwnerId);
        }

        return command.Id;
    }
}