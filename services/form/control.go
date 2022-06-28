package form

type ControlType int

const (
	ControlTypeText ControlType = iota
)

var controlTypeNames map[ControlType]string = map[ControlType]string{
	ControlTypeText: "text",
}

func (ct ControlType) String() string {
	return controlTypeNames[ct]
}

type Control struct {
	ID          string      `json:"id" dynamodbav:"id"`
	Title       string      `json:"title" dynamodbav:"title"`
	Description string      `json:"description" dynamodbav:"description"`
	Type        ControlType `json:"type" dynamodbav:"type"`
	Required    bool        `json:"required" dynamodbav:"required"`
}
