package dynamodb

import (
	"context"
	"fmt"

	"github.com/aws/aws-sdk-go-v2/aws"
	"github.com/aws/aws-sdk-go-v2/config"
	"github.com/aws/aws-sdk-go-v2/feature/dynamodb/attributevalue"
	"github.com/aws/aws-sdk-go-v2/service/dynamodb"
	"github.com/aws/aws-sdk-go-v2/service/dynamodb/types"
	"github.com/formchata/services/form"
	"github.com/google/uuid"
)

type FormStore struct {
	table *string
	db    *dynamodb.Client
}

func NewFormStore(table string) *FormStore {
	cfg, err := config.LoadDefaultConfig(context.TODO(), func(o *config.LoadOptions) error {
		o.Region = "us-east-1"
		return nil
	})

	if err != nil {
		panic(err)
	}

	db := dynamodb.NewFromConfig(cfg)
	return &FormStore{
		table: aws.String(table),
		db:    db,
	}
}

func (store *FormStore) PutItem(ctx context.Context, form *form.Form) error {
	if form.ID == "" {
		form.ID = uuid.New().String()
	}

	m, err := attributevalue.MarshalMap(form)
	if err != nil {
		return err
	}

	fmt.Printf("m %+v\n", m)

	_, err = store.db.PutItem(ctx, &dynamodb.PutItemInput{
		TableName: store.table,
		Item:      m,
	})

	if err != nil {
		return err
	}

	return nil
}

func (store *FormStore) GetItem(ctx context.Context, id string) (form.Form, error) {
	result, err := store.db.GetItem(ctx, &dynamodb.GetItemInput{
		TableName: store.table,
		Key: map[string]types.AttributeValue{
			"id": &types.AttributeValueMemberS{
				Value: id,
			},
		},
	})

	if err != nil {
		return form.Form{}, err
	}

	f := &form.Form{}
	err = attributevalue.UnmarshalMap(result.Item, &f)

	if err != nil {
		return form.Form{}, err
	}

	return *f, nil
}

func (store *FormStore) DeleteItem(ctx context.Context, id string) error {
	_, err := store.db.DeleteItem(ctx, &dynamodb.DeleteItemInput{
		TableName: store.table,
		Key: map[string]types.AttributeValue{
			"id": &types.AttributeValueMemberS{
				Value: id,
			},
		},
	})

	return err
}

func (store *FormStore) QueryItems(ctx context.Context, query struct{ OwnerID string }) ([]form.Form, error) {
	var forms []form.Form

	results, err := store.db.Query(ctx, &dynamodb.QueryInput{
		TableName:              store.table,
		IndexName:              aws.String("OwnerIndex"),
		KeyConditionExpression: aws.String("ownerId = :ownerId"),
		ExpressionAttributeValues: map[string]types.AttributeValue{
			":ownerId": &types.AttributeValueMemberS{
				Value: query.OwnerID,
			},
		},
	})

	if err != nil {
		return forms, err
	}

	err = attributevalue.UnmarshalListOfMaps(results.Items, &forms)

	if err != nil {
		return []form.Form{}, err
	}

	return forms, nil
}
