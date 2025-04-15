using UnityEngine;

namespace App.Services.BoardService
{
    public struct BoardSettings
    {
        public int Width;
        public int Height;
        public Color TileColorA;
        public Color TileColorB;
        public Color HighlightColor;
        public Color BlockerColor;

        public BoardSettings(int width, int height, Color tileColorA, Color tileColorB, Color highlightColor, Color blockerColor)
        {
            Width = width;
            Height = height;
            TileColorA = tileColorA;
            TileColorB = tileColorB;
            HighlightColor = highlightColor;
            BlockerColor = blockerColor;
        }

    }
}
