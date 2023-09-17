namespace XeniaWebServices.aggregates
{
    using System.Collections.Generic;

    public interface SessionArbitrationResponse
    {
        int TotalPlayers { get; set; }
        List<Machine> Machines { get; set; }
    }

    public class Machine
    {
        public string Id { get; set; }
        public List<Player> Players { get; set; }
    }

}
