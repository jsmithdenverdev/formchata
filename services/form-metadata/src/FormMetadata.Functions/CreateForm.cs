using System.Text.Json;
using System.Net;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using FormMetadata.Application.Commands;
using FormMetadata.Application.Commands.CreateForm;
using FormMetadata.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace FormMetadata.Functions;

public class CreateForm
{
    private readonly ICommandHandler<CreateFormCommand> _handler;
    private readonly ILogger<CreateForm> _logger;

    public CreateForm()
    {
        var loggerFactory = LoggerFactory.Create(b => b.AddConsole());

        // todo throw exception if null
        var tableName = Environment.GetEnvironmentVariable("TABLE_NAME");
        var dynamoDbClient = new AmazonDynamoDBClient();
        var repository = new FormRepository(dynamoDbClient, tableName);

        _logger = loggerFactory.CreateLogger<CreateForm>();
        _handler = new CreateFormCommandHandler(loggerFactory.CreateLogger<CreateFormCommandHandler>(), repository);
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var command = JsonSerializer.Deserialize<CreateFormCommand>(
            request.Body,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (command == null)
        {
            return new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Body = "No form provided",
                Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
            };
        }

        try
        {
            _handler.Handle(command);
        }
        catch (Exception e)
        {
            context.Logger.LogError(e.Message);
            return new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.InternalServerError,
                Body = "Internal Server Error",
                Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
            };
        }

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int) HttpStatusCode.OK,
            Body = "Hello AWS Serverless",
            Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
        };

        return response;
    }
}