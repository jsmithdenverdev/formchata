package lambda

import (
	"context"
	"log"
	"net/http"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type CreateHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *CreateHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		Body:       "not implemented",
		StatusCode: http.StatusNotImplemented,
	}
}
