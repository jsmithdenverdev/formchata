package dynamodb

import (
	"context"

	"github.com/aws/aws-sdk-go/aws"
	"github.com/aws/aws-sdk-go/aws/session"
	"github.com/aws/aws-sdk-go/service/dynamodb"
	"github.com/aws/aws-sdk-go/service/dynamodb/dynamodbattribute"
	"github.com/formchata/services/form"
	"github.com/google/uuid"
)

type FormStore struct {
	table *string
	db    *dynamodb.DynamoDB
}

func NewFormStore(table string) *FormStore {
	sess := session.Must(session.NewSession())
	db := dynamodb.New(sess)
	return &FormStore{
		table: aws.String(table),
		db:    db,
	}
}

func (store *FormStore) PutItem(ctx context.Context, form *form.Form) error {
	form.ID = uuid.New().String()

	m, err := dynamodbattribute.MarshalMap(form)
	if err != nil {
		return err
	}

	_, err = store.db.PutItem(&dynamodb.PutItemInput{
		TableName: store.table,
		Item:      m,
	})

	if err != nil {
		return err
	}

	return nil
}

func (store *FormStore) GetItem(ctx context.Context, id string) (form.Form, error) {
	result, err := store.db.GetItem(&dynamodb.GetItemInput{
		TableName: store.table,
		Key: map[string]*dynamodb.AttributeValue{
			"id": {
				S: aws.String(id),
			},
		},
	})

	if err != nil {
		return form.Form{}, err
	}

	f := &form.Form{}
	err = dynamodbattribute.UnmarshalMap(result.Item, &f)

	if err != nil {
		return form.Form{}, err
	}

	return *f, nil
}

func (store *FormStore) DeleteItem(ctx context.Context, id string) error {
	_, err := store.db.DeleteItem(&dynamodb.DeleteItemInput{
		TableName: store.table,
		Key: map[string]*dynamodb.AttributeValue{
			"id": {
				S: aws.String(id),
			},
		},
	})

	return err
}
