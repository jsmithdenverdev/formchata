package main

import (
	"log"
	"os"

	runtime "github.com/aws/aws-lambda-go/lambda"
	"github.com/formchata/services/form/dynamodb"
	"github.com/formchata/services/form/lambda"
)

func main() {
	var (
		table = os.Getenv("TABLE_NAME")
	)

	if table == "" {
		log.Fatal("missing required environment variable TABLE_NAME")
	}

	h := lambda.ReadHandler{
		Logger: log.Default(),
		Store:  dynamodb.NewFormStore(table),
	}

	runtime.Start(h.HandleAPIGateway)
}
