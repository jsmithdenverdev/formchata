using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Xunit;

namespace FormMetadata.Functions.Tests;

public class CreateFormTests
{
    public CreateFormTests()
    {
    }

    public void TestHandle()
    {
        var createForm = new CreateForm();
        var request = new APIGatewayProxyRequest();
        var context = new TestLambdaContext();
        var response = createForm.Handle(request, context);

        Assert.Equal(200, response.StatusCode);
        Assert.Equal("Hello AWS Serverless", response.Body);
    }
}