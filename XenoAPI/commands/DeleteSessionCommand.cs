using XeniaWebServices.Controllers;
using XeniaWebServices.XenoAPI.aggregates;
using XeniaWebServices.XenoAPI.Interfaces;

namespace XeniaWebServices.XenoAPI.commands
{
    public class DeleteSessionCommand : ICommand
    {
        public TitleId Title { get; }
        public SessionId SessionId { get; }

        public DeleteSessionCommand(TitleId title, SessionId sessionId)
        {
            Title = title;
            SessionId = sessionId;
        }
    }

    public class GetSessionQuery : IQuery<Session>
    {
        internal int FilledPrivateSlots;

        public TitleId Title { get; }
        public SessionId SessionId { get; }
        public string Id { get; internal set; }
        public int FilledPublicSlots { get; internal set; }
        public int OpenPrivateSlots { get; internal set; }
        public int OpenPublicSlots { get; internal set; }
        public int PrivateSlotsCount { get; internal set; }
        public int PublicSlotsCount { get; internal set; }
        public string MacAddress { get; internal set; }
        public int Port { get; internal set; }
        public string HostAddress { get; internal set; }
        public int Flags { get; set; }
        public List<Player> Players { get; internal set; }

        public GetSessionQuery(TitleId title, SessionId sessionId)
        {
            Title = title;
            SessionId = sessionId;
        }
    }

    public class DeleteSessionCommandHandler : ICommandHandler<DeleteSessionCommand>
    {
        public async Task HandleAsync(DeleteSessionCommand command)
        {
            // Implement the logic to delete a session here
        }
    }

    public class GetSessionQueryHandler : IQueryHandler<GetSessionQuery, Session>
    {
        public async Task<Session> HandleAsync(GetSessionQuery query)
        {
            // Implement the logic to get a session here and return it
            //return new Session(); // Replace with the actual implementation
            return null;
        }
    }
}