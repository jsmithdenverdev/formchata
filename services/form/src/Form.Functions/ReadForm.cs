using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Form.Application.Interfaces;
using Form.Application.Queries;
using Form.Application.Queries.ReadForm;
using Form.Domain.Entities;
using Form.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Form.Functions;

public class ReadForm
{
    private readonly ILogger<ReadForm> _logger;
    private readonly IQueryHandler<ReadFormQuery, FormMeta> _queryHandler;

    public ReadForm()
    {
        var services = new ServiceCollection();

        ConfigureServices(services);

        var provider = services.BuildServiceProvider();

        _logger = provider.GetService<ILogger<ReadForm>>() ??
                  throw new InvalidOperationException();
        _queryHandler = provider.GetService<IQueryHandler<ReadFormQuery, FormMeta>>() ??
                        throw new InvalidOperationException();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(b => b.AddConsole());

        var dynamoDbClient = new AmazonDynamoDBClient();
        var dynamoDbContext = new DynamoDBContext(dynamoDbClient);

        services.AddSingleton<IDynamoDBContext>(dynamoDbContext);
        services.AddTransient<IQueryHandler<ReadFormQuery, FormMeta>, ReadFormQueryHandler>();
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public async Task<APIGatewayProxyResponse> Handle(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            var id = request.PathParameters["id"] ??
                     throw new Exception("No id provided.");

            var result = await _queryHandler.Handle(new ReadFormQuery {Id = id});

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
                Body = JsonSerializer.Serialize(result,
                    new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase}),
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
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