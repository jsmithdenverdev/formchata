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
            const params = {
                TableName: tableName,
                Item: marshall(response),
            };

            logger.log('info', `putItem params ${response.id}`, params);

            await client.putItem(params);

            return response.id;
        },

        async read(id: string): Promise<FormResponse | null> {
            const params = {
                TableName: tableName,
                Key: {
                    id: {
                        S: id,
                    },
                },
            };

            logger.log('info', `getItem params ${id}`, params);

            var response = await client.getItem(params);

            if (!response || !response.Item) {
                return null;
            }

            logger.log('info', `dynamodb record ${response.Item['id']}`, response);

            return <FormResponse>unmarshall(response.Item);
        },
        async delete(id: string): Promise<void> {
            const params = {
                TableName: tableName,
                Key: {
                    id: {
                        S: id,
                    },
                },
            };

            logger.log('info', `deleteItem params ${id}`, params);

            await client.deleteItem(params);
        },
    };
}
