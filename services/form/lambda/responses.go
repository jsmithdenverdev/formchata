package lambda

import (
	"encoding/json"
	"net/http"

	"github.com/aws/aws-lambda-go/events"
)

func responseNotFound() events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		StatusCode: http.StatusNotFound,
		Body:       "not found",
		Headers: map[string]string{
			"Content-Type": "text/plain",
		},
	}
}

func responseInternalServerError() events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		StatusCode: http.StatusInternalServerError,
		Body:       "internal server error",
		Headers: map[string]string{
			"Content-Type": "text/plain",
		},
	}
}

func responseCreated(id string) events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		StatusCode: http.StatusCreated,
		Body:       id,
		Headers: map[string]string{
			"Content-Type": "text/plain",
		},
	}
}

func responseNotImplemented() events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		StatusCode: http.StatusNotImplemented,
		Body:       "not implemented",
		Headers: map[string]string{
			"Content-Type": "text/plain",
		},
	}
}

func responseUnauthorized() events.APIGatewayProxyResponse {
	return events.APIGatewayProxyResponse{
		StatusCode: http.StatusUnauthorized,
		Body:       "unauthorized",
		Headers: map[string]string{
			"Content-Type": "text/plain",
		},
	}
}

// TODO there should be another responseOk that takes no input and returns an empty body
func responseOk(data interface{}) (events.APIGatewayProxyResponse, error) {
	if data != nil {
		body, err := json.Marshal(data)
		if err != nil {
			return events.APIGatewayProxyResponse{}, err
		}

		return events.APIGatewayProxyResponse{
			StatusCode: http.StatusOK,
			Body:       string(body),
			Headers: map[string]string{
				"Content-Type": "application/json",
			},
		}, nil
	}

	return events.APIGatewayProxyResponse{
		StatusCode: http.StatusOK,
		Body:       "",
		Headers: map[string]string{
			"Content-Type": "text/plain",
		},
	}, nil
}
