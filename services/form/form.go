package form

type Form struct {
	OwnerID     string
	ID          string
	Title       string
	Description string
	Sections    []Section
}
