export type CommandHandler<TCommand, TResponse = void> = (command: TCommand) => Promise<TResponse>;
