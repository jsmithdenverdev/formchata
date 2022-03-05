using System.Text.Json;
using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Form.Application.Commands;
using Form.Application.Commands.CreateForm;
using Form.Application.Interfaces;
using Form.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Form.Functions;

public class CreateForm
{
    private readonly ILogger<CreateForm> _logger;
    private readonly ICommandHandler<CreateFormCommand, string> _commandHandler;

    public CreateForm()
    {
        var collection = new ServiceCollection();

        ConfigureServices(collection);

        var provider = collection.BuildServiceProvider();

        _logger = provider.GetService<ILogger<CreateForm>>() ??
                  throw new InvalidOperationException();
        _commandHandler = provider.GetService<ICommandHandler<CreateFormCommand, string>>() ??
                          throw new InvalidOperationException();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(b => b.AddConsole());

        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDbContext = new DynamoDBContext(dynamoDbClient);

        services.AddSingleton<IDynamoDBContext>(dynamoDbContext);
        services.AddTransient<ICommandHandler<CreateFormCommand, string>, CreateFormCommandHandler>();
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            // Deserialize the request body into a CreateFormCommand
            var command = JsonSerializer.Deserialize<CreateFormCommand>(
                              request.Body,
                              new JsonSerializerOptions {PropertyNameCaseInsensitive = true}) ??
                          throw new Exception("No form provided.");

            command.Form.OwnerId = context.Identity.IdentityId;

            // Pass the CreateFormCommand to its handler and return the created forms id
            var result = await _commandHandler.Handle(command);

            return new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = result,
                Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            // TODO: handled exceptions
            return new APIGatewayProxyResponse
            {
                StatusCode = (int) HttpStatusCode.InternalServerError,
                Body = e.Message,
                Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
            };
        }
    }
}