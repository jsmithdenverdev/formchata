package lambda

import (
	"context"
	"log"

	"github.com/aws/aws-lambda-go/events"
	"github.com/formchata/services/form"
)

type ListHandler struct {
	Logger *log.Logger
	Store  form.Store
}

func (handler *ListHandler) HandleAPIGateway(ctx context.Context, event events.APIGatewayProxyRequest) (events.APIGatewayProxyResponse, error) {
	userId, ok := event.RequestContext.Authorizer["userId"]
	if !ok {
		return responseUnauthorized(), nil
	}

	forms, err := handler.Store.QueryItems(ctx, struct{ OwnerID string }{userId.(string)})
	if err != nil {
		handler.Logger.Printf("QueryItems failed; %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	response, err := responseOk(forms)
	if err != nil {
		handler.Logger.Printf("responseOk failed; %s\n", err.Error())
		return responseInternalServerError(), nil
	}

	return response, nil
}
