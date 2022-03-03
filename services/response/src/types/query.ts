export type QueryHandler<TQuery, TResponse = void> = (query: TQuery) => Promise<TResponse>;
