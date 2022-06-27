package mock

import (
	"context"
	"errors"

	"github.com/formchata/services/form"
	"github.com/google/uuid"
)

type FormStore struct {
	forms map[string]form.Form
}

func NewFormStore() *FormStore {
	forms := make(map[string]form.Form)
	return &FormStore{
		forms,
	}
}

func (store *FormStore) PutItem(ctx context.Context, form *form.Form) error {
	id := uuid.New().String()
	store.forms[id] = *form
	form.ID = id

	return nil
}

func (store *FormStore) GetItem(ctx context.Context, id string) (form.Form, error) {
	f, ok := store.forms[id]
	if !ok {
		return form.Form{}, errors.New("form not found")
	}

	return f, nil
}

func (store *FormStore) DeleteItem(ctx context.Context, id string) error {
	_, ok := store.forms[id]
	if !ok {
		return errors.New("form not found")
	}

	delete(store.forms, id)

	return nil
}
