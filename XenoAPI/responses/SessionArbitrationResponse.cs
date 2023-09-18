namespace XeniaWebServices.XenoAPI.responses
{
    using System.Collections.Generic;
    using XeniaWebServices.XenoAPI.aggregates;

    public interface SessionArbitrationResponse
    {
        int TotalPlayers { get; set; }
        List<Machine> Machines { get; set; }
    }

    public class Machine
    {
        public string? Id { get; set; }
        public List<Player>? Players { get; set; }
    }

}
