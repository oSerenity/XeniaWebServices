using System.Net;
using XeniaWebServices.Controllers;

namespace XeniaWebServices.queries
{
    public class FindPlayerQuery
    {
        public IpAddress HostAddress { get; }

        public FindPlayerQuery(IpAddress hostAddress)
        {
            HostAddress = hostAddress;
        }
    }
}