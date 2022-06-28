package form

type Form struct {
	OwnerID     string    `json:"ownerId" dynamodbav:"ownerId"`
	ID          string    `json:"id" dynamodbav:"id"`
	Title       string    `json:"title" dynamodbav:"title"`
	Description string    `json:"description" dynamodbav:"description"`
	Archived    bool      `json:"archived" dynamodbav:"archived"`
	Sections    []Section `json:"sections" dynamodbav:"sections"`
}
