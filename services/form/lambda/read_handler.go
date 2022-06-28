package lambda

import (
	"context"
	"log"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type ReadHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *ReadHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) (events.APIGatewayProxyResponse, error) {
	userId, ok := event.RequestContext.Authorizer["userId"]
	if !ok {
		return responseUnauthorized(), nil
	}

	id, ok := event.PathParameters["id"]
	if !ok {
		return responseNotFound(), nil
	}

	f, err := handler.Store.GetItem(ctx, id)
	if err != nil {
		handler.Logger.Printf("GetItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	// TODO: Is there a more secure way to do this? At this point the data would
	// have already been read.
	if f.OwnerID != userId.(string) {
		return responseNotFound(), nil
	}

	response, err := responseOk(f)
	if err != nil {
		handler.Logger.Printf("responseOk failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	return response, nil
}
