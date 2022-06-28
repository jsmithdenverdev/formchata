package lambda

import (
	"context"
	"log"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type DeleteHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *DeleteHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) (events.APIGatewayProxyResponse, error) {
	id, ok := event.PathParameters["id"]
	if !ok {
		return responseNotFound(), nil
	}

	err := handler.Store.DeleteItem(ctx, id)
	if err != nil {
		handler.Logger.Printf("GetItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	response, _ := responseOk(nil)
	return response, nil
}
