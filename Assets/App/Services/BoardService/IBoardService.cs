using ppl.Services.Core;

namespace App.Services.BoardService
{
    public interface IBoardService : IService
    {
        int BoardWidth();
        int BoardHeight();
        float BoardScreenWidth();
        float BoardScreenHeight();
        void HighlightGrid(Coordinate coordinate);
        Path[] HighlightedPaths();
        void HighlightGrids(Path path);
        void WatchPawn(string pawnId);
        void UnwatchPawn();
        void InterpolateWatchedPawnTo(Path path);
        void UnHighlightGrids();
        UnityEngine.Vector2 BoardOrigin();
        UnityEngine.Vector2 GetCenter(string boardCoordinate);

        Coordinate GetNeighborhoodCoordinate(Coordinate from, Direction direction);
        Path EvaluatePathFromDirections(string pawnId, Coordinate from,PathDirection[] directions, bool inclusive = false);
    }
}
