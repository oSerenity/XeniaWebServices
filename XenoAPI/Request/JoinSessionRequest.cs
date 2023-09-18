using XeniaWebServices.Controllers;

namespace XeniaWebServices.XenoAPI.Request
{
    public class JoinSessionRequest
    {
        public SessionId SessionId { get; internal set; }
        public TitleId TitleId { get; internal set; }
        public List<Xuid> Xuids { get; internal set; }
    }
}