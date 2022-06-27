package main

import (
	"log"

	runtime "github.com/aws/aws-lambda-go/lambda"
	"github.com/formchata/services/form/lambda"
	"github.com/formchata/services/form/mock"
)

func main() {
	h := lambda.UpdateHandler{
		Logger: log.Default(),
		Store:  mock.NewFormStore(),
	}

	runtime.Start(h.HandleAPIGateway)
}
