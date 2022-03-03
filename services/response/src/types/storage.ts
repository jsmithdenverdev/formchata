import { FormResponse } from '.';

export type FormResponseWriter = {
    write(response: FormResponse): Promise<string>;
};

export type FormResponseReader = {
    read(id: string): Promise<FormResponse | null>;
};

export type FormResponseDeleter = {
    delete(id: string): Promise<void>;
};

export type FormResponseStore = FormResponseWriter & FormResponseReader & FormResponseDeleter;
