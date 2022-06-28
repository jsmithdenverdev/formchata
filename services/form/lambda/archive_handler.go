package lambda

import (
	"context"
	"log"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type ArchiveHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *ArchiveHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) (events.APIGatewayProxyResponse, error) {
	id, ok := event.PathParameters["id"]
	if !ok {
		return responseNotFound(), nil
	}

	f, err := handler.Store.GetItem(ctx, id)
	if err != nil {
		handler.Logger.Printf("GetItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	f.Archived = true

	err = handler.Store.PutItem(ctx, &f)
	if err != nil {
		handler.Logger.Printf("PutItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	response, _ := responseOk(nil)
	return response, nil
}
