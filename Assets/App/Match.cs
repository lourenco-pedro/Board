using System.Collections.Generic;

namespace App
{
    public class Match
    {
        public Dictionary<string, string> Pawns;
        public Dictionary<int, string[]> Teams;
        public List<string> BlockedTiles;
        public List<MatchSnapshot> Snapshots;
    }
}