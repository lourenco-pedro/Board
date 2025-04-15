using System;
using System.Collections.Generic;
using App.Entities;
using App.Services.BoardService;
using App.Services.BoardService.Implementations;
using App.Services.MatchService;
using App.Services.MatchService.Implementations;
using UnityEngine;
using ppl.Services.Core;
using ppl.SimpleEventSystem;

namespace App
{
    public class GameStarter : MonoBehaviour, IEventBindable
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
            
            this.ListenToEvent<bool>(EventsConstants.EVENT_UI_NAVIGATE_SPANSHOT, (payload) =>
            {
                int direction = payload.Args ? 1 : -1;
                ServiceContainer.UseService<IMatchService>(matchService => matchService.SetHistoryIndex(matchService.GetHistoryIndex() + direction));
            });
            
        }

        private void OnDestroy()
        {
            this.StopListenForEvent<bool>(EventsConstants.EVENT_UI_NAVIGATE_SPANSHOT);
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

        Dictionary<string, object> FactoryBoardServiceArguments()
        {
            //Configurações que serão passadas para o BoardService.
            return new Dictionary<string, object>()
            {
                {
                    "Settings", new BoardSettings(
                        (int)_boardSize.x,
                        (int)_boardSize.y,
                        _boardTileColorA,
                        _boardTileColorB,
                        _highLightTileColor,
                        _blockerColor)
                },
                { "Board", _board },
                { "EntitiesCatalog", Resources.Load<EntitiesCatalog>("EntitiesCatalog") },
                { "TeamColors", _teamColors }
            };
        }

        async void SetupServices()
        {
            await ServiceContainer.AddService<IBoardService, BoardService>(FactoryBoardServiceArguments());
            IMatchService matchService = await ServiceContainer.AddService<IMatchService, MatchService>(new Dictionary<string, object>());

            //Nesse momento, BoardService ja deve estar escutando por eventos de alteração da match.
            //Ao alterar a match com SetMatch. O evento será disparado, iniciando a cadeia de SetupBoard do BoardService
            matchService.SetMatch(new App.Match
            {
                //Definindo os times e as peças de cada time
                Teams = new Dictionary<int, string[]>()
                {
                    { 0, new[] { "team1_a", "team1_b" } },
                    { 1, new[] { "team2_a", "team2_b" } }
                },
                
                //Identificando cada peça
                Pawns = new Dictionary<string, string>()
                {
                    { "team1_a", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "team1_b", EntityNamesConstants.ENTITY_PAWN_BISHOP },
                    { "team2_a", EntityNamesConstants.ENTITY_PAWN_HORSE },
                    { "team2_b", EntityNamesConstants.ENTITY_PAWN_BISHOP },
                },
                
                //Configurando tiles bloqueados
                BlockedTiles = new Dictionary<string, string>()
                {
                    { "d5", EntityNamesConstants.ENTITY_BLOCK_1 },
                    { "e5", EntityNamesConstants.ENTITY_BLOCK_1 },
                    { "f3", EntityNamesConstants.ENTITY_BLOCK_1 }
                },
                
                //Configurando snapshots.
                Snapshots = new List<MatchSnapshot>()
                {
                    new MatchSnapshot()
                    {
                        Pieces = new Dictionary<string, string>()
                        {
                            { "team1_a", "d4" },
                            { "team1_b", "e4" },
                            { "team2_a", "f4" },
                            { "team2_b", "g4" },
                        }
                    },
                }
            }, overrideSnapshotIndex: 0);
        }
    }
}