using System.Net;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Form.Application.Commands;
using Form.Application.Commands.DeleteForm;
using Form.Application.Interfaces;
using Form.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Form.Functions;

public class DeleteForm
{
    private readonly ILogger<ReadForm> _logger;
    private readonly ICommandHandler<DeleteFormCommand, string> _commandHandler;

    public DeleteForm()
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        var provider = services.BuildServiceProvider();

        _logger = provider.GetService<ILogger<ReadForm>>() ??
                  throw new InvalidOperationException();
        _commandHandler = provider.GetService<ICommandHandler<DeleteFormCommand, string>>() ??
                          throw new InvalidOperationException();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(b => b.AddConsole());

        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDbContext = new DynamoDBContext(dynamoDbClient);

        services.AddSingleton<IDynamoDBContext>(dynamoDbContext);
        services.AddTransient<ICommandHandler<DeleteFormCommand, string>, DeleteFormCommandHandler>();
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var id = request.PathParameters["id"] ??
                     throw new Exception("No id provided.");
            var ownerId = request.RequestContext.Authorizer.Claims["cognito:username"];

            var command = new DeleteFormCommand
            {
                Id = id,
                OwnerId = ownerId,
            };

            // TODO: The handler is just returning the supplied ID. There should be a check to see if this record exists
            // before we attempt to delete it.
            var result = await _commandHandler.Handle(command);

            if (result == null)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int) HttpStatusCode.NotFound,
                    Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
                };
            }

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