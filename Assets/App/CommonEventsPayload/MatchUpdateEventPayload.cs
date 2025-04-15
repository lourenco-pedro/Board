using System.Collections.Generic;

namespace App.CommonEventsPayload
{
    public class MatchUpdateEventPayload
    {
        public Match Match;
        public int SnapshotIndex;
        public int PreviusSnapshotIndex;
        public bool IsNow;
        public Dictionary<int, string[]> Teams;
        public int[] BotTeams;
        public Dictionary<string, string> BlockedTiles;
        
        public MatchUpdateEventPayload(Match match, int snapshotIndex, int previusSnapshotIndex, bool isNow, Dictionary<int, string[]> teams, int[] botTeams, Dictionary<string, string> blockedTiles)
        {
            Match = match;
            SnapshotIndex = snapshotIndex;
            PreviusSnapshotIndex = previusSnapshotIndex;
            IsNow = isNow;
            Teams = teams;
            BotTeams = botTeams;
            BlockedTiles = blockedTiles;
        }
    }
}
