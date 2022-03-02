using System.Net;
using Amazon.DynamoDBv2;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using FormMetadata.Application.Commands;
using FormMetadata.Application.Commands.DeleteForm;
using FormMetadata.Application.Interfaces;
using FormMetadata.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FormMetadata.Functions;

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
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddSingleton<IFormRepository, FormRepository>();
        services.AddTransient<ICommandHandler<DeleteFormCommand, string>, DeleteFormCommandHandler>();
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var id = request.PathParameters["id"] ??
                     throw new Exception("No id provided.");

            // TODO: The handler is just returning the supplied ID. There should be a check to see if this record exists
            // before we attempt to delete it.
            var result = await _commandHandler.Handle(new DeleteFormCommand {Id = id});

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