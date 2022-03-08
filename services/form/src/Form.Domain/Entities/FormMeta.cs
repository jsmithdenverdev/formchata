using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace Form.Domain.Entities;

[DynamoDBTable("form", LowerCamelCaseProperties = true)]
public class FormMeta
{
    [DynamoDBHashKey] public string OwnerId { get; set; }
    [DynamoDBRangeKey("formId")] public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<FormSection> Sections { get; set; }
}

public class FormSection
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<FormControl> Controls { get; set; }
}

public class FormControl
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Type { get; set; }
    public bool Required { get; set; }
}