namespace XeniaWebServices.XenoAPI.Interfaces
{
    public interface ICommand
    {
    }

    public interface IQuery<TResult>
    {
    }

    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }

    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
