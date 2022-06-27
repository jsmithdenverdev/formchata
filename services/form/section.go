package form

type Section struct {
	FormID      string
	ID          string
	Title       string
	Description string
	Controls    []Control
}
