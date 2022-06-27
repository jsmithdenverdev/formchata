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
	ID          string
	Title       string
	Description string
	Type        ControlType
	Required    bool
}
