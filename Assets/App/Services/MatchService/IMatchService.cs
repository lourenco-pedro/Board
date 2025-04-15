using ppl.Services.Core;

namespace App.Services.MatchService
{
    public interface IMatchService : IService
    {
        bool SetMatch(Match match, int overrideSnapshotIndex = -1);
        int GetHistoryIndex();
        void SetHistoryIndex(int index);
        bool IsBot(string pawnId);
        void RegisterMatchHistory(MatchSnapshot snapshot);
    }
}
