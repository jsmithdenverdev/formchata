using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;

namespace FormMetadata.Functions;

public class GetForm
{
    public GetForm()
    {
    }

    [LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]
    public APIGatewayProxyResponse Handle(APIGatewayProxyRequest request, ILambdaContext context)
    {
        context.Logger.LogInformation("Get Request\n");

        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int) HttpStatusCode.OK,
            Body = "Hello AWS Serverless",
            Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
        };

        return response;
    }
}