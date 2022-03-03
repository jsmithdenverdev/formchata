import { CommandHandler } from 'src/types/command';
import { FormResponseDeleter } from 'src/types/storage';
import { Logger } from 'winston';

export type DeleteFormResponse = {
    id: string;
};

type HandlerArgs = {
    deleter: FormResponseDeleter;
    logger: Logger;
};

export default function ({ deleter, logger }: HandlerArgs): CommandHandler<DeleteFormResponse, void> {
    return async function (command: DeleteFormResponse) {
        logger.log('info', `deleting response ${command.id}`, command);

        await deleter.delete(command.id);
    };
}
