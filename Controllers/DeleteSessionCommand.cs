namespace XeniaWebServices.Controllers
{
    public class DeleteSessionCommand
    {
        public TitleId Title { get; }
        public SessionId SessionId { get; }

        public DeleteSessionCommand(TitleId title, SessionId sessionId)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            SessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
        }
    }
}