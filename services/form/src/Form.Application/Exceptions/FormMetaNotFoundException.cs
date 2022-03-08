namespace Form.Application.Exceptions;

public class FormMetaNotFoundException : Exception
{
    public FormMetaNotFoundException(string id) : base($"Form with id {id} not found.")
    {
    }

    public FormMetaNotFoundException(string id, string ownerId) : base(
        $"Form with id {id} not found or does not belong to user {ownerId}.")
    {
    }
}