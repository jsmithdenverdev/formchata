package main

import (
	"context"

	"github.com/aws/aws-lambda-go/events"
	runtime "github.com/aws/aws-lambda-go/lambda"
)

func main() {
	runtime.Start(handler)
}

func handler(ctx context.Context, event events.APIGatewayCustomAuthorizerRequest) (events.APIGatewayCustomAuthorizerResponse, error) {
	return events.APIGatewayCustomAuthorizerResponse{
		PrincipalID: "user",
		PolicyDocument: events.APIGatewayCustomAuthorizerPolicy{
			Version: "2012-10-17",
			Statement: []events.IAMPolicyStatement{
				{
					Action: []string{"execute-api:Invoke"},
					Effect: "Allow",
					// TODO: this should be []string{event.MethodArn} but caching is making
					// that difficult. The policy is cached for 300 seconds once its executed
					// which means a user only has access to the first method they requested.
					Resource: []string{"*"},
				},
			},
		},
		Context: map[string]interface{}{
			"userId": "123-test",
		},
	}, nil
}
