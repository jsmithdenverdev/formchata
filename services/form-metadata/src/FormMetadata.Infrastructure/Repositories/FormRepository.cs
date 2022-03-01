using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using FormMetadata.Application.Interfaces;
using FormMetadata.Domain.Entities;

namespace FormMetadata.Infrastructure.Repositories;

public class FormRepository : IFormRepository
{
    private readonly Table _table;

    public FormRepository(IAmazonDynamoDB client, string tableName)
    {
        _table = Table.LoadTable(client, tableName);
    }

    public async Task Create(Form form)
    {
        var sectionEntities = new DynamoDBList();
        var formEntity = new Document();

        foreach (var section in form.Sections)
        {
            var controlEntities = new DynamoDBList();
            var sectionEntity = new Document();

            foreach (var control in section.Controls)
            {
                var controlEntity = new Document();

                controlEntity["title"] = control.Title;
                controlEntity["description"] = control.Description;
                controlEntity["type"] = control.Type;
                controlEntity["required"] = new DynamoDBBool(control.Required);

                controlEntities.Add(controlEntity);
            }

            sectionEntity["title"] = section.Title;
            sectionEntity["description"] = section.Description;
            sectionEntity["controls"] = controlEntities;

            sectionEntities.Add(sectionEntity);
        }

        formEntity["id"] = form.Id;
        formEntity["title"] = form.Title;
        formEntity["description"] = form.Description;
        formEntity["sections"] = sectionEntities;

        await _table.PutItemAsync(formEntity);
    }
}