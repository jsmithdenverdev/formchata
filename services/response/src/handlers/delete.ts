import { APIGatewayProxyEvent, APIGatewayProxyResult } from 'aws-lambda';
import commandHandler, { DeleteFormResponse } from 'src/commands/delete-form-response';
import createStore from 'src/storage/form-response-store';
import { FormResponseStore } from 'src/types/storage';
import { Logger } from 'winston';
import { createLogger } from 'src/logger/logger';

let store: FormResponseStore;
let dynamoLogger, logger: Logger;

(function init() {
    try {
        if (!process.env.TABLE_NAME) {
            throw new Error('missing required environment variable TABLE_NAME');
        }

        logger = createLogger('info', 'CreateFormResponse');
        dynamoLogger = createLogger('info', 'DynamoDB CreateFormResponse');

        store = createStore(dynamoLogger, process.env!.TABLE_NAME);
    } catch (e) {
        console.log(`PANIC: ${JSON.stringify(e)}`);
        process.exit(1);
    }
})();

export const lambdaHandler = async (event: APIGatewayProxyEvent): Promise<APIGatewayProxyResult> => {
    if (!event.pathParameters || !event.pathParameters['id']) {
        return {
            statusCode: 404,
            body: '',
        };
    }

    const command: DeleteFormResponse = {
        id: event.pathParameters['id'],
    };

    try {
        await commandHandler({ deleter: store, logger })(command);

        return {
            statusCode: 200,
            body: command.id,
        };
    } catch (e: any) {
        logger.error(e.message);

        return {
            statusCode: 500,
            body: 'Internal server error.',
        };
    }
};
