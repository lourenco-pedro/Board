using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.CommonEventsPayload;
using App.Entities;
using App.Entities.Exceptions;
using ppl.SimpleEventSystem;
using UnityEngine;
using Grid = App.Entities.Grid;
using NullEntityCatalogException = App.Entities.Exceptions.NullEntityCatalogException;
using Object = UnityEngine.Object;

namespace App.Services.BoardService.Implementations
{
    public class BoardService : IBoardService, IEventBindable
    {
        public string Name => nameof(BoardService);

        public BoardSettings Settings { get; set; }
        public RectTransform Board { get; set; }
        public EntitiesCatalog EntitiesCatalog { get; set; }
        
        public Color[] TeamColors { get; set; }
        
        private Transform _gridsParent;
        private Transform _pawnParent;
        private Transform _blocksParent;

        private Dictionary<string, Vector2> _centers = new Dictionary<string, Vector2>();
        private Dictionary<string, Grid> _grids = new Dictionary<string, Grid>();
        private Dictionary<string, Pawn> _pawns = new Dictionary<string, Pawn>();
        private Dictionary<string, Block> _blocks = new Dictionary<string, Block>();
        private Dictionary<string, string> _pawnIdByCoordinate = new Dictionary<string, string>();
        private Dictionary<int, Path> _highlightedGrids = new Dictionary<int, Path>();
        private Dictionary<int, string[]> _teams = new Dictionary<int, string[]>();

        private string _watchingPawnId;

        private void SpawnMatchPawns(Match match, int snapshotIndex, Dictionary<int, string[]> teams)
        {
            var missingPawns = match.Snapshots[snapshotIndex].Pieces
                .Where(pawn => !_pawns.ContainsKey(pawn.Key));

            foreach (var missingPawn in missingPawns)
            {
                float width = BoardScreenWidth() / BoardWidth();
                float height = BoardScreenHeight() / BoardHeight();
                
                string prefabId = match.Pawns[missingPawn.Key];
                string id = missingPawn.Key;
                int teamIndex = teams.FirstOrDefault(team => team.Value.Contains(id)).Key;
                
                Pawn pawnPrefab = EntitiesCatalog.GetEntityAs<Pawn>(prefabId);
                Pawn pawnInstance = Object.Instantiate(pawnPrefab, _pawnParent);
                pawnInstance.OverrideId(id);

                Coordinate coordinate = Coordinate.ToCoordinate(match.Snapshots[snapshotIndex].Pieces[id]);
                
                ((RectTransform)pawnInstance.transform).sizeDelta = new Vector2(width, height);
                pawnInstance.SetCoordinate(coordinate, true);
                pawnInstance.SetColor(TeamColors[teamIndex]);
                pawnInstance.name = missingPawn.Key;
                
                _pawns.Add(missingPawn.Key, pawnInstance);
                _pawnIdByCoordinate[coordinate.BoardCoordinate] = missingPawn.Key;
                pawnInstance.Setup();
            }
        }

        private void RemovePawnsNoLongerInMatch(Match match, int snapshotIndex)
        {
            var pawnsToRemove = _pawns.Keys
                .Except(match.Snapshots[snapshotIndex].Pieces.Keys)
                .ToArray();

            foreach (string pawnToRemove in pawnsToRemove)
            {
                _pawnIdByCoordinate[pawnToRemove] = "";
                Object.Destroy(_pawns[pawnToRemove].gameObject);
                _pawns.Remove(pawnToRemove);
            }
        }

        private void MovePawns(Match match, int snapshotIndex, int previusSnapshotIndex)
        {
            MatchSnapshot currentSnapshot = match.Snapshots[snapshotIndex];
            MatchSnapshot previousSnapshot = match.Snapshots[previusSnapshotIndex];
            
            string[] movingIds = currentSnapshot.Pieces
                .Where(piece => previousSnapshot.Pieces.ContainsKey(piece.Key) && piece.Value != previousSnapshot.Pieces[piece.Key])
                .Select(piece => piece.Key).ToArray();
            
            foreach (string movingId in movingIds)
            {
                Coordinate destination = Coordinate.ToCoordinate(currentSnapshot.Pieces[movingId]);
                _pawnIdByCoordinate[previousSnapshot.Pieces[movingId]] = "";
                _pawnIdByCoordinate[destination.BoardCoordinate] = movingId;
                
                _pawns[movingId].SetCoordinate(destination, true);
            }
        }

        private void SpawnGrids()
        {
            if (null == EntitiesCatalog)
            {
                throw new NullEntityCatalogException("Could not instantiate grids.");
            }

            Grid gridPrefab = EntitiesCatalog.GetEntityAs<Grid>(EntityNamesConstants.ENTITY_GRID);
            if (null == gridPrefab)
            {
                throw new NullEntityException("Could not find Grid prefab to instantiate");
            }
            
            for (int x = 0; x < Settings.Width; x++) 
            {
                for (int y = 0; y < Settings.Height; y++)
                {
                    float width = BoardScreenWidth() / BoardWidth();
                    float height = BoardScreenHeight() / BoardHeight();
                    float originOffset = width / 2;
                    
                    Vector2 position = new Vector2(BoardOrigin().x + (width * x) + originOffset, BoardOrigin().y + (height * y) + originOffset);
                    Coordinate coordinate = Coordinate.ToCoordinate(x, y, position);
                    _centers.Add(Coordinate.ToCoordinateString(x, y), position);
                    
                    Grid grid = Object.Instantiate(gridPrefab, _gridsParent);
                    grid.SetCoordinate(coordinate, true);
                    
                    ((RectTransform)grid.transform).sizeDelta = new Vector2(width, height);
                    grid.SetColor((x + y) % 2 == 0 ? Settings.TileColorA : Settings.TileColorB);
                    grid.SetHighlightColor(Settings.HighlightColor);
                    
                    _grids.Add(coordinate.BoardCoordinate, grid);
                    _pawnIdByCoordinate.Add(coordinate.BoardCoordinate, "");
                }
            }
        }

        private void RefreshBoard(Match match, int snapshotIndex, int previusSnapshotIndex, Dictionary<int, string[]> teams, Dictionary<string, string> blocks)
        {
            //Check if it has grids
            if (null == _gridsParent)
            {
                _gridsParent = new GameObject("Grids").transform;
                _gridsParent.SetParent(Board);
                _gridsParent.localPosition = Vector3.zero;
                SpawnGrids();
            }

            if (null == _pawnParent)
            {
                _pawnParent = new GameObject("Pawns").transform;
                _pawnParent.SetParent(Board);
                _pawnParent.localPosition = Vector3.zero;
            }
            
            RemovePawnsNoLongerInMatch(match, snapshotIndex);
            SpawnMatchPawns(match, snapshotIndex, teams);
            MovePawns(match, snapshotIndex, previusSnapshotIndex);

            RefreshBlockedTiles(blocks);
        }
        
        private void EventOnMatchChange(EventPayload<MatchUpdateEventPayload> e)
        {
            _teams = e.Args.Teams;
            RefreshBoard(
                e.Args.Match, 
                e.Args.SnapshotIndex, 
                e.Args.PreviusSnapshotIndex, 
                e.Args.Teams,
                e.Args.BlockedTiles);
            
            UnHighlightGrids();
            
            if(e.Args.IsNow)
                UnwatchPawn();
            else //Disabilitando todos os pawns em caso estiver no passado
                WatchPawn("invalidId");
        }

        private void RefreshBlockedTiles(Dictionary<string, string> blockedTiles)
        {

            if (null == _blocksParent)
            {
                _blocksParent = new GameObject("Blocks").transform;
                _blocksParent.SetParent(Board);
                _blocksParent.localPosition = Vector3.zero;
            }
            
            //Remove blocks inexistentes
            string[] blocksToRemove = _blocks.Keys.Except(blockedTiles.Keys).ToArray();
            foreach (var blockedTileCoordinate in blocksToRemove)
            {
                Object.Destroy(_blocks[blockedTileCoordinate]);
                _blocks.Remove(blockedTileCoordinate);
            }

            string[] newBlocks = blockedTiles.Keys.Except(_blocks.Keys).ToArray();
            
            //Instancia novos blocos
            foreach (var newBlock in newBlocks)
            {
                Coordinate coordinate = Coordinate.ToCoordinate(newBlock);
                string prefabId = blockedTiles[newBlock];

                Block blockPrefab = EntitiesCatalog.GetEntityAs<Block>(prefabId);
                Block blockInstance = Object.Instantiate(blockPrefab, _blocksParent);
                
                blockInstance.SetCoordinate(coordinate);
                blockInstance.transform.localPosition = coordinate.Center;
                blockInstance.SetColor(Settings.BlockerColor);
                _blocks.Add(coordinate.BoardCoordinate, blockInstance);
            }
        }

        private bool IsSameTeam(string pawnAId, string pawnBId)
        {
            int teamA = _teams.FirstOrDefault(team => team.Value.Contains(pawnAId)).Key;
            int teamB = _teams.FirstOrDefault(team => team.Value.Contains(pawnBId)).Key;
            
            return teamA == teamB;
        }

        public Task AsyncSetup(Dictionary<string, object> args = null)
        {
            if(args != null && args.TryGetValue("EntitiesCatalog", out object entitiesCatalog))
                EntitiesCatalog = (EntitiesCatalog)entitiesCatalog;
            if(args != null && args.TryGetValue("Settings", out object settings))
                Settings = (BoardSettings)settings;
            if(args != null && args.TryGetValue("Board", out object board))
                Board = (RectTransform)board;
            if (args != null && args.TryGetValue("TeamColors", out object colors))
                TeamColors = (Color[])colors;
            
            this.ListenToEvent<MatchUpdateEventPayload>(EventsConstants.EVENT_ON_MATCH_CHANGE, EventOnMatchChange);
            
            return Task.CompletedTask;
        }
       
         public int BoardWidth()
        {
            return Settings.Width;
        }

        public int BoardHeight()
        {
            return Settings.Height;
        }

        public float BoardScreenWidth()
        {
            return ((RectTransform)Board.transform).rect.width;
        }

        public float BoardScreenHeight()
        {
            return ((RectTransform)Board.transform).rect.height;
        }

        public Path[] HighlightedPaths()
        {
            return _highlightedGrids.Values.ToArray();
        }

        public void HighlightGrid(Coordinate coordinate)
        {
            _grids[coordinate.BoardCoordinate].Highlight();
        }

        public void HighlightGrids(Path path)
        {
            if(path.Coordinates.Length == 0)
                return;
            
            if(path.Coordinates.Length > 0)
                _highlightedGrids.Add(path.GetHashCode(), path);

            if (path.Inclusive)
            {
                foreach(Coordinate coordinate in path.Coordinates)
                    HighlightGrid(coordinate);
                return;
            }
            
            HighlightGrid(path.Coordinates[^1]);
        }

        public void WatchPawn(string pawnId)
        {
            _watchingPawnId = pawnId;
            this.EmitEvent(EventsConstants.EVENT_ON_PAWN_SELECTED, pawnId);
        }

        public void UnwatchPawn()
        {
            string watchedPawn = _watchingPawnId;
            _watchingPawnId = "";
            this.EmitEvent(EventsConstants.EVENT_ON_PAWN_DESELECTED, watchedPawn);
        }

        public void InterpolateWatchedPawnTo(Path path, Action onFinished = null)
        {
            if (string.IsNullOrEmpty(_watchingPawnId) || !_pawns.ContainsKey(_watchingPawnId))
                return;

            Coordinate destination = path.Coordinates[^1];
            _pawnIdByCoordinate[_pawns[_watchingPawnId].Coordinate.BoardCoordinate] = "";
            string pawnToKill = _pawnIdByCoordinate[destination.BoardCoordinate];
            
            _pawns[_watchingPawnId].Interpolate(path, .1f, onFinish: () =>
            {
                _pawns[_watchingPawnId].SetCoordinate(path.Coordinates[^1]);
                _pawnIdByCoordinate[destination.BoardCoordinate] = _watchingPawnId;

                string movedPawnId = _watchingPawnId;
                UnwatchPawn();

                Dictionary<string, string> piecesInBoard = !string.IsNullOrEmpty(pawnToKill) ? 
                    _pawns.Where(pawn => !pawn.Key.Equals(pawnToKill)).ToDictionary(k => k.Key, v => v.Value.Coordinate.BoardCoordinate) :
                    _pawns.ToDictionary(k => k.Key, v => v.Value.Coordinate.BoardCoordinate);
                
                this.EmitEvent(EventsConstants.EVENT_ON_MOVEMENT_DATA, new MatchSnapshot()
                {
                    NextChainnedMovements = 0,
                    Pieces = piecesInBoard  
                });
                
                this.EmitEvent(EventsConstants.EVENT_BEHAVIOURCHAIN_ON_FINISH_MOVE, new FinishMoveEventPayload
                {
                    EndPosition = destination.BoardCoordinate,
                    StartPosition = path.Coordinates[0].BoardCoordinate,
                    PawnId = movedPawnId,
                    Path = path
                });
                
                onFinished?.Invoke();
            });
        }

        public void UnHighlightGrids()
        {
            foreach (var highlightedPaths in _highlightedGrids.Values)
            {
                foreach (Coordinate highlightedCoordinate in highlightedPaths.Coordinates)
                {
                    if(!_grids.TryGetValue(highlightedCoordinate.BoardCoordinate, out var grid))
                        continue;
                    
                    grid.Unhighlight();
                }
            }
            
            _highlightedGrids.Clear();
        }

        public Coordinate GetPawnCoordinate(string pawnId)
        {
            if (_pawns.TryGetValue(pawnId, out var pawn))
            {
                return pawn.Coordinate;
            }
            
            return Coordinate.ToCoordinate("a0");
        }

        public Vector2 BoardOrigin()
        {
            return new Vector2(Board.rect.center.x - Board.rect.width / 2, Board.rect.center.y - Board.rect.height / 2);
        }

        public Vector2 GetCenter(string boardCoordinate)
        {
            if(_centers.TryGetValue(boardCoordinate, out Vector2 center))
                return center;
            
            return Vector2.zero;
        }
        
        public Coordinate GetNeighborhoodCoordinate(Coordinate from, Direction direction)
        {
            switch (direction)
            {
                default:
                case Direction.UP:
                {
                    if (from.Vertical == Settings.Height - 1)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal, from.Vertical + 1, _centers[Coordinate.ToCoordinateString(from.Horizontal, from.Vertical + 1)]);
                }
                case Direction.DOWN:
                    if (from.Vertical == 0)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal, from.Vertical - 1, _centers[Coordinate.ToCoordinateString(from.Horizontal, from.Vertical - 1)]);
                case Direction.RIGHT:
                {
                    if (from.Horizontal == Settings.Width - 1)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal + 1, from.Vertical, _centers[Coordinate.ToCoordinateString(from.Horizontal + 1, from.Vertical)]);
                }
                case Direction.LEFT:
                {
                    if (from.Horizontal == 0)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal - 1, from.Vertical, _centers[Coordinate.ToCoordinateString(from.Horizontal - 1, from.Vertical)]);
                }
                case Direction.UP_RIGHT:
                {
                    if (from.Horizontal == Settings.Width - 1 || from.Vertical == Settings.Height - 1)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal + 1, from.Vertical + 1, _centers[Coordinate.ToCoordinateString(from.Horizontal + 1, from.Vertical + 1)]);
                }
                case Direction.UP_LEFT:
                {
                    if (from.Horizontal == 0 || from.Vertical == Settings.Height - 1)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal - 1, from.Vertical + 1, _centers[Coordinate.ToCoordinateString(from.Horizontal - 1, from.Vertical + 1)]);
                }
                case Direction.DOWN_RIGHT:
                {
                    if (from.Horizontal == Settings.Width - 1 || from.Vertical == 0)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal + 1, from.Vertical - 1, _centers[Coordinate.ToCoordinateString(from.Horizontal + 1, from.Vertical - 1)]);
                }
                case Direction.DOWN_LEFT:
                {
                    if (from.Horizontal == 0 || from.Vertical == 0)
                        return from;
                    return Coordinate.ToCoordinate(from.Horizontal - 1, from.Vertical - 1, _centers[Coordinate.ToCoordinateString(from.Horizontal - 1, from.Vertical - 1)]);
                }
            }
        }
        
        public Path EvaluatePathFromDirections(string id, Coordinate from, PathDirection[] directions, bool inclusive = false)
        {
            List<Coordinate> coordinates = new List<Coordinate>() { from };
            
            for (int i = 0; i < directions.Length; i++) 
            {
                int x = from.Horizontal;
                int y = from.Vertical;
                
                for (int j = 0; j < directions[i].Length; j++)
                {
                    var newIntCoords = CoordinateUtils.AddCoordinatesInt(x, y, directions[i].Direction);
                    
                    x = newIntCoords.x;
                    y = newIntCoords.y;
                    
                    if (x < 0 || y < 0 || x == Settings.Width || y == Settings.Height || _blocks.ContainsKey(Coordinate.ToCoordinateString(x, y)))
                    {
                        if(inclusive)
                            break;
                        
                        return new Path { Coordinates = Array.Empty<Coordinate>() };
                    }

                    from = Coordinate.ToCoordinate(x, y, _centers[Coordinate.ToCoordinateString(x, y)]);
                    coordinates.Add(from);
                }
            }

            Coordinate firstFoundPawnInWay =
                coordinates.FirstOrDefault(coord => !string.IsNullOrEmpty(_pawnIdByCoordinate[coord.BoardCoordinate]) && _pawnIdByCoordinate[coord.BoardCoordinate] != id);
            
            string firstFoundPawnId = !string.IsNullOrEmpty(firstFoundPawnInWay.BoardCoordinate)
                ? _pawnIdByCoordinate[firstFoundPawnInWay.BoardCoordinate]
                : string.Empty;
            
            bool isFriend = IsSameTeam(firstFoundPawnId, id);

            if (!string.IsNullOrEmpty(firstFoundPawnId))
            {
                if (isFriend && inclusive)
                {
                    return coordinates.ToArray().ToPath(true).Split(firstFoundPawnInWay, inclusive: false);
                }
                else if (isFriend)
                {
                    return new Path { Coordinates = Array.Empty<Coordinate>() };
                }
                else if (inclusive || coordinates.IndexOf(firstFoundPawnInWay) == coordinates.Count - 1)
                {
                    return coordinates.ToArray().ToPath(inclusive).Split(firstFoundPawnInWay, inclusive: true);
                }
                else
                {
                    return new Path { Coordinates = Array.Empty<Coordinate>() };
                }
            }
            
            return coordinates.ToArray().ToPath(inclusive);
        }
        
#if UNITY_EDITOR
        public void DebugService()
        {
            GUILayout.Label("Coordinates:");
            GUILayout.BeginHorizontal();
            string gridsCoords = string.Join(',', _grids.Keys.ToArray());
            GUILayout.Label(gridsCoords);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            GUILayout.Label("Highlighted coordinates: " + _highlightedGrids.Count);
            
            GUILayout.Space(10);
            GUILayout.Label("Pawns position");
            string coordinates = string.Join(',', _pawnIdByCoordinate.Where(data => data.Value != "").Select(data => $"{data.Key}:{data.Value}"));
            GUILayout.Label(coordinates);
        }
#endif
    }
}