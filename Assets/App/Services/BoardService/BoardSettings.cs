using UnityEngine;

namespace App.Services.BoardService
{
    
    //Aqui seria um ótimo local para passar todos os prefabs que serão utilizados durante essa partida.
    //Junto com addressable, o GameStarter poderia carregar apenas os prefabs que serão utilizados
    //durante a partida. Preenchendo um campo que existiria aqui de nome "Prefabs". Algo assim.
    //Isso evitaria o uso desnecessário de Resources.Load
    
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
