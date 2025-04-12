using System.Linq;
using App.Services.BoardService;
using ppl.Services.Core;
using UnityEngine;

namespace App.Entities
{
    public class Grid : BoardEntity
    {
        private bool _isHighlighted; 
        private Color _normalColor;
        private Color _highlightColor;
        
        public override void Setup()
        {
        }

        public override void SetColor(Color color)
        {
            _normalColor = color;
            base.SetColor(color);
        }
        
        public void SetHighlightColor(Color color)
        {
            _highlightColor = color;
        }

        public void Highlight()
        {
            if(_isHighlighted)
                return;
            
            _isHighlighted = true;
            _image.color = _highlightColor;
        }
        
        public void Unhighlight()
        {
            _isHighlighted = false;
            _image.color = _normalColor;
        }

        protected override void OnClick()
        {
            if (!_isHighlighted)
                return;
            
            ServiceContainer.UseService<IBoardService>(boardService =>
            {
                Path[] paths = boardService.HighlightedPaths();
                Path foundPath = paths.First(p => p.Coordinates.Contains(Coordinate));
                if (foundPath.Coordinates.Length > 0)
                {
                    foundPath = foundPath.Split(Coordinate);
                    boardService.InterpolateWatchedPawnTo(foundPath);
                    boardService.UnHighlightGrids();
                }
            });
        }
    }
}
