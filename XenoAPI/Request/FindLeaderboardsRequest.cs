namespace XeniaWebServices.XenoAPI.Request
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class FindLeaderboardsRequest
    {
        [JsonProperty("players")]
        public List<string> Players { get; set; }

        [JsonProperty("titleId")]
        public string TitleId { get; set; }

        [JsonProperty("queries")]
        public List<FindLeaderboardRequestLeaderboardQuery> Queries { get; set; }
    }

    public class FindLeaderboardRequestLeaderboardQuery
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("statisticIds")]
        public List<string> StatisticIds { get; set; }
    }

}