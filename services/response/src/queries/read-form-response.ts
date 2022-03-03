import { FormResponse } from 'src/types';
import { QueryHandler } from 'src/types/query';
import { FormResponseReader } from 'src/types/storage';
import { Logger } from 'winston';

export type ReadFormResponse = {
    id: string;
};

type HandlerArgs = {
    reader: FormResponseReader;
    logger: Logger;
};

export default function ({ reader, logger }: HandlerArgs): QueryHandler<ReadFormResponse, FormResponse | null> {
    return async function (query: ReadFormResponse): Promise<FormResponse | null> {
        logger.log('info', `reading response ${query.id}`, query);

        return await reader.read(query.id);
    };
}
