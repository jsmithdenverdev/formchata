using Amazon.DynamoDBv2.DataModel;

namespace FormMetadata.Domain.Entities;

[DynamoDBTable("form-metadata", LowerCamelCaseProperties = true)]
public class Form
{
    [DynamoDBHashKey] public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<FormSection> Sections { get; set; }
}

public class FormSection
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<FormControl> Controls { get; set; }
}

public class FormControl
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Type { get; set; }
    public bool Required { get; set; }
}