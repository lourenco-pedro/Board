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

        public MatchUpdateEventPayload(Match match, int snapshotIndex, int previusSnapshotIndex, bool isNow, Dictionary<int, string[]> teams)
        {
            Match = match;
            SnapshotIndex = snapshotIndex;
            PreviusSnapshotIndex = previusSnapshotIndex;
            IsNow = isNow;
            Teams = teams;
        }
    }
}
