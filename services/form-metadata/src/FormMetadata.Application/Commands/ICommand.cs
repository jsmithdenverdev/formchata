namespace FormMetadata.Application.Commands;

public interface ICommand
{
}

public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand
{
    public Task<TResult> Handle(TCommand command);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    public Task Handle(TCommand command);
}