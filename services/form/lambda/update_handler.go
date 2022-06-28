package lambda

import (
	"context"
	"encoding/json"
	"log"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type UpdateHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *UpdateHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) (events.APIGatewayProxyResponse, error) {
	var f form.Form
	id, ok := event.PathParameters["id"]
	if !ok {
		return responseNotFound(), nil
	}

	err := json.Unmarshal([]byte(event.Body), &f)
	if err != nil {
		handler.Logger.Printf("Unmarshal failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	f.ID = id

	err = handler.Store.PutItem(ctx, &f)
	if err != nil {
		handler.Logger.Printf("PutItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	response, err := responseOk(struct{ ID string }{id})
	if err != nil {
		handler.Logger.Printf("PutItem failed: %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	return response, nil
}
