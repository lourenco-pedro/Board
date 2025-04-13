using System.Collections.Generic;
using App.Entities;
using App.Services.BoardService;
using App.Services.BoardService.Implementations;
using App.Services.MatchService;
using App.Services.MatchService.Implementations;
using UnityEngine;
using ppl.Services.Core;

namespace App
{
    public class GameStarter : MonoBehaviour
    {
        
        [Header("Board settings")]
        [SerializeField] private RectTransform _board;
        [SerializeField] private Vector2 _boardSize;
        [SerializeField] private Color _boardTileColorA;
        [SerializeField] private Color _boardTileColorB;
        [SerializeField] private Color _highLightTileColor;
        [SerializeField] private Color _blockerColor;

        [Space] [SerializeField] private Color[] _teamColors;
        
        void Start()
        {
            SetupServices();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ServiceContainer.UseService<IMatchService>(matchService => matchService.SetHistoryIndex(matchService.GetHistoryIndex() - 1));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ServiceContainer.UseService<IMatchService>(matchService => matchService.SetHistoryIndex(matchService.GetHistoryIndex() + 1));
            }
        }

        async void SetupServices()
        {
            //Aqui seria um ótimo local para passar todos os prefabs que serão utilizados durante essa partida.
            //Junto com addressable. Preenchendo um campo que existiria aqui de nome "Prefabs". Algo assim.
            //Isso evitaria o uso desnecessário de Resources.Load
            await ServiceContainer.AddService<IBoardService, BoardService>(new Dictionary<string, object>() {
                { "Settings", new BoardSettings(
                    (int)_boardSize.x, 
                    (int)_boardSize.y, 
                    _boardTileColorA, 
                    _boardTileColorB, 
                    _highLightTileColor,
                    _blockerColor)
                },
                { "Board", _board },
                { "EntitiesCatalog", Resources.Load<EntitiesCatalog>("EntitiesCatalog")},
                { "TeamColors", _teamColors }
            });
            
            IMatchService matchService = await ServiceContainer.AddService<IMatchService, MatchService>(new Dictionary<string, object>());

            
            //Nesse momento, BoardService ja deve estar escutando por eventos de alteração da match.
            //Ao alterar a match com SetMatch. O evento será disparado, iniciando a cadeia de SetupBoard
            matchService.SetMatch(new App.Match
            {
                Teams = new Dictionary<int, string[]>()
                {
                    { 0, new[] { "team1_a", "team1_b" } },
                    { 1, new[] { "team2_a", "team2_b" } }
                },
                Pawns = new Dictionary<string, string>()
                {
                    { "team1_a", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "team1_b", EntityNamesConstants.ENTITY_PAWN_BISHOP },
                    { "team2_a", EntityNamesConstants.ENTITY_PAWN_HORSE },
                    { "team2_b", EntityNamesConstants.ENTITY_PAWN_BISHOP },
                },
                BlockedTiles = new Dictionary<string, string>()
                {
                    { "d5", EntityNamesConstants.ENTITY_BLOCK_1 },
                    { "e5", EntityNamesConstants.ENTITY_BLOCK_1 },
                    { "f3", EntityNamesConstants.ENTITY_BLOCK_1 }
                },
                Snapshots = new List<MatchSnapshot>()
                {
                    new MatchSnapshot()
                    {
                        NextChainnedMovements = 0,
                        Pieces = new Dictionary<string, string>()
                        {
                            { "team1_a", "d4" },
                            { "team1_b", "e4" },
                            { "team2_a", "f4" },
                            { "team2_b", "g4" },
                        }
                    },
                }
            }, overrdeSnapshotIndex: 0);
        }
    }
}