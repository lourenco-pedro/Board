using ppl.Services.Core;

namespace App.Services.MatchService
{
    public interface IMatchService : IService
    {
        bool SetMatch(Match match, int overrdeSnapshotIndex = -1);
        int GetHistoryIndex();
        void SetHistoryIndex(int index);
        void RegisterMatchHistory(MatchSnapshot snapshot);
    }
}
