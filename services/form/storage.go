package form

import "context"

// Store defines a standard set of storage methods for Form's . It is generally implemented in a standalone
// package with a name reflecting the storage mechanism (e.g., dynamodb). Operations needing storage  capabilities for
// Form's should use this interface.
type Store interface {
	// PutItem persists the provided Form in the configured data store. If an error occurs it will be returned.
	PutItem(ctx context.Context, form *Form) error

	// GetItem locates a Form in the configured data store using the provided id. If an error occurs it will be returned.
	GetItem(ctx context.Context, id string) (Form, error)

	// DeleteItem deletes a Form from the configured data store using the provided id. If an error occurs it will be returned.
	DeleteItem(ctx context.Context, id string) error

	// DeleteItem deletes a Form from the configured data store using the provided id. If an error occurs it will be returned.
	QueryItems(ctx context.Context, query struct{ OwnerID string }) ([]Form, error)
}
