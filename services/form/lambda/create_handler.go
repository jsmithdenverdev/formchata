package lambda

import (
	"context"
	"encoding/json"
	"log"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type CreateHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *CreateHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) (events.APIGatewayProxyResponse, error) {
	var f form.Form
	userId, ok := event.RequestContext.Authorizer["userId"]
	if !ok {
		return responseUnauthorized(), nil
	}

	err := json.Unmarshal([]byte(event.Body), &f)
	if err != nil {
		handler.Logger.Printf("Unmarshal failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	f.OwnerID = userId.(string)

	err = handler.Store.PutItem(ctx, &f)
	if err != nil {
		handler.Logger.Printf("PutItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	return responseCreated(f.ID), nil
}
