using XeniaWebServices.Controllers;

namespace XeniaWebServices.XenoAPI.commands
{
    internal class JoinSessionCommand
    {
        private TitleId titleId;
        private SessionId sessionId;
        private Xuid xuids;

        public JoinSessionCommand(TitleId titleId, SessionId sessionId, List<Xuid> xuids)
        {
            this.titleId = titleId;
            this.sessionId = sessionId;
            xuids = xuids;
        }
    }
}