import { DynamoDB } from '@aws-sdk/client-dynamodb';
import { marshall, unmarshall } from '@aws-sdk/util-dynamodb';
import { FormResponse } from 'src/types';
import { FormResponseStore } from 'src/types/storage';
import { Logger } from 'winston';

export default function (logger: Logger, tableName: string): FormResponseStore {
    const client = new DynamoDB({
        region: process.env!.AWS_REGION,
    });

    return {
        async write(response: FormResponse): Promise<string> {
            client.putItem({
                TableName: tableName,
                Item: marshall(response),
            });

            return response.id;
        },

        async read(id: string): Promise<FormResponse | null> {
            var response = await client.getItem({
                TableName: tableName,
                Key: {
                    id: {
                        S: id,
                    },
                },
            });

            if (!response || !response.Item) {
                return null;
            }

            return <FormResponse>unmarshall(response.Item);
        },
        async delete(id: string): Promise<void> {
            await client.deleteItem({
                TableName: tableName,
                Key: {
                    id: {
                        S: id,
                    },
                },
            });
        },
    };
}
