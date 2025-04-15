using System;
using App.Services.BoardService;
using App.Services.MatchService;
using DG.Tweening;
using ppl.Services.Core;
using ppl.SimpleEventSystem;
using UnityEngine;

namespace App.Entities
{
    public class Pawn : BoardEntity, IEventBindable
    {
        [SerializeField] private MovementLayout _settings;

        private bool _isWatchingPawn;
        private Sequence _sequence;

        private void EventOnPawnSelected(EventPayload<string> payload)
        {
            if(payload.Args.Equals(Id))
                return;

            _image.raycastTarget = false;
            _button.interactable = false;
        }

        private void EventOnPawnDeselected(EventPayload<string> payload)
        {
            _image.raycastTarget = true;
            _button.interactable = true;
        }

        protected override void OnDestroy()
        {
            this.StopListenForEvent<string>(EventsConstants.EVENT_ON_PAWN_SELECTED);
            this.StopListenForEvent<string>(EventsConstants.EVENT_ON_PAWN_DESELECTED);
        }

        protected override void OnClick()
        {
            bool isBot = false;
            ServiceContainer.UseService<IMatchService>(matchService =>
            {
                isBot = matchService.IsBot(Id);
            });
            
             if(isBot)
                 return;
            
            ServiceContainer.UseService<IBoardService>(boardService =>
            {
                boardService.UnHighlightGrids();

                if (_isWatchingPawn)
                {
                    _isWatchingPawn = false;
                    boardService.UnwatchPawn();
                    return;
                }
                
                _isWatchingPawn = true;
                boardService.WatchPawn(_id);
                foreach (var pattern in _settings.Patterns)
                {
                    Path highlightPaths = boardService.EvaluatePathFromDirections(_id, _coordinate, pattern.NeighborhoodSettings, pattern.Inclusive);
                    boardService.HighlightGrids(highlightPaths);
                }
            });
        }

        public override void Setup()
        {
            this.ListenToEvent<string>(EventsConstants.EVENT_ON_PAWN_SELECTED, EventOnPawnSelected);
            this.ListenToEvent<string>(EventsConstants.EVENT_ON_PAWN_DESELECTED, EventOnPawnDeselected);
        }

        public override void SetCoordinate(Coordinate coordinate, bool autoPosition = false)
        {
            _coordinate = coordinate;
            if (autoPosition)
            {
                Interpolate(new Path { Coordinates = new []{ coordinate }}, 0, ()=>{ });
            }
        }

        public void Interpolate(Path toCoordinates, float duration, Action onFinish)
        {
            if (null != _sequence)
                _sequence.Kill();
            
            _sequence = DOTween.Sequence();
            foreach (Coordinate coordinate in toCoordinates.Coordinates)
            {
                _sequence
                    .Append((transform as RectTransform)
                        .DOAnchorPos(coordinate.Center, duration).SetEase(Ease.Linear));
            }

            _sequence.OnComplete(()=> onFinish?.Invoke());
            _sequence.Play();
        }
    }
}
