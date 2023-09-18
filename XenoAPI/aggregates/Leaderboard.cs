using XeniaWebServices.Controllers;
using XeniaWebServices.value_objects;

namespace XeniaWebServices.XenoAPI.aggregates
{
    public class Leaderboard
    {
        private readonly LeaderboardProps _props;

        public Leaderboard(LeaderboardProps props)
        {
            _props = props;
        }

        public static Leaderboard Create(LeaderboardProps props)
        {
            return new Leaderboard(new LeaderboardProps
            {
                Id = props.Id,
                TitleId = props.TitleId,
                Player = props.Player,
                Stats = new Dictionary<string, LeaderboardStat>(props.Stats)
            });
        }

        public void Update(LeaderboardUpdateProps props)
        {
            foreach (var kvp in props.Stats)
            {
                if (!_props.Stats.ContainsKey(kvp.Key))
                {
                    _props.Stats[kvp.Key] = new LeaderboardStat
                    {
                        Type = kvp.Value.Type,
                        Value = kvp.Value.Value
                    };
                }

                _props.Stats[kvp.Key].Type = kvp.Value.Type;

                if (kvp.Value.Method == "sum")
                {
                    _props.Stats[kvp.Key].Value += kvp.Value.Value;
                }
                else if (kvp.Value.Method == "set")
                {
                    _props.Stats[kvp.Key].Value = kvp.Value.Value;
                }
                else if (kvp.Value.Method == "min")
                {
                    _props.Stats[kvp.Key].Value = Math.Min(kvp.Value.Value, _props.Stats[kvp.Key].Value);
                }
            }
        }

        public LeaderboardId Id => _props.Id;
        public TitleId TitleId => _props.TitleId;
        public Xuid Player => _props.Player;
        public Dictionary<string, LeaderboardStat> Stats => _props.Stats;
    }

    public class LeaderboardProps
    {
        public LeaderboardId? Id { get; set; }
        public TitleId? TitleId { get; set; }
        public Xuid? Player { get; set; }
        public Dictionary<string, LeaderboardStat>? Stats { get; set; }
    }

    public class LeaderboardUpdateProps
    {
        public Dictionary<string, LeaderboardStatUpdate>? Stats { get; set; }
    }

    public class LeaderboardStat
    {
        public int Type { get; set; }
        public int Value { get; set; }
    }

    public class LeaderboardStatUpdate
    {
        public int Type { get; set; }
        public int Value { get; set; }
        public string? Method { get; set; }
    }
}
