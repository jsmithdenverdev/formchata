import { FormResponse } from 'src/types';
import { CommandHandler } from 'src/types/command';
import { FormResponseWriter } from 'src/types/storage';
import { v4 as uuidv4 } from 'uuid';
import { Logger } from 'winston';

export type CreateFormResponse = {
    response: FormResponse;
};

type HandlerArgs = {
    writer: FormResponseWriter;
    logger: Logger;
};

export default function ({ writer, logger }: HandlerArgs): CommandHandler<CreateFormResponse, string> {
    return async function (command: CreateFormResponse) {
        const id = uuidv4();

        command.response.id = id;

        logger.log('info', `creating response ${command.response.id}`, command);

        await writer.write(command.response);

        return id;
    };
}
