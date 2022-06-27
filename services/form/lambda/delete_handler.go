package lambda

import (
	"context"
	"log"
	"net/http"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type DeleteHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *DeleteHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		Body:       "not implemented",
		StatusCode: http.StatusNotImplemented,
	}
}
