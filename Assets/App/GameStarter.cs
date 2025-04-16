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
        public enum MatchType
        {
            HyperDiveChallenge,
            Chess,
            Damas
        };
    
        [Header("What do you want to play?"), SerializeField] MatchType _matchType = MatchType.HyperDiveChallenge; 
        
        [Space, Header("Board settings")]
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

            Match saveFile = MatchGenerator.GenerateMatch();
            string saveFileString = Newtonsoft.Json.JsonConvert.SerializeObject(saveFile);
           
            await System.IO.File.WriteAllTextAsync(Application.dataPath + "/save.json", saveFileString);
            
            //Nesse momento, BoardService ja deve estar escutando por eventos de alteração da match.
            //Ao alterar a match com SetMatch. O evento será disparado, iniciando a cadeia de SetupBoard do BoardService
            matchService.SetMatch(MatchGenerator.GenerateMatch(), overrideSnapshotIndex: 0);
        }
    }
}