using System.Collections.Generic;
using System.Linq;
using App.Entities;

namespace App
{
    public static class MatchStarterTypeFactory
    {
        public static Match GenerateMatch()
        {
            List<string> chessCoordinates = new List<string>
            {
                "a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7",
                "b0", "b1", "b2", "b3", "b4", "b5", "b6", "b7",
                "c0", "c1", "c2", "c3", "c4", "c5", "c6", "c7",
                "d0", "d1", "d2", "d3", "d4", "d5", "d6", "d7",
                "e0", "e1", "e2", "e3", "e4", "e5", "e6", "e7",
                "f0", "f1", "f2", "f3", "f4", "f5", "f6", "f7",
                "g0", "g1", "g2", "g3", "g4", "g5", "g6", "g7",
                "h0", "h1", "h2", "h3", "h4", "h5", "h6", "h7"
            };

            string[] blockedCoordinates = new string[8];
            string[] enemiesCoordinates = new string[10];
            string[] playerCoordinates = new string[4];
            
            Dictionary<string, string> pieces = new Dictionary<string, string>();
            
            //Randomizando bloqueadores de caminho
            for (int i = 0; i < 8; i++)
            {
                int randomizedIndex = UnityEngine.Random.Range(0, chessCoordinates.Count);
                blockedCoordinates[i] = chessCoordinates[randomizedIndex];
                chessCoordinates.RemoveAt(randomizedIndex);
            }
            
            //Randomizando inimigos
            for (int i = 0; i < 10; i++)
            {
                int randomizedIndex = UnityEngine.Random.Range(0, chessCoordinates.Count);
                enemiesCoordinates[i] = chessCoordinates[randomizedIndex];
                chessCoordinates.RemoveAt(randomizedIndex);
                pieces["enemy_"+i] = enemiesCoordinates[i];
            }

            //Randomizando players
            for (int i = 0; i < 4; i++)
            {
                int randomizedIndex = UnityEngine.Random.Range(0, chessCoordinates.Count);
                playerCoordinates[i] = chessCoordinates[randomizedIndex];
                chessCoordinates.RemoveAt(randomizedIndex);
                pieces["player_"+i] = playerCoordinates[i];
            }

            //Montando a partida
            return new Match()
            {
                //Definindo os times e as peças de cada time
                Teams = new Dictionary<int, string[]>()
                {
                    { 0, new[] { "player_0", "player_1", "player_2", "player_3" } },
                    { 1, new[] { "enemy_0", "enemy_1", "enemy_2", "enemy_3", "enemy_4", "enemy_5", "enemy_6", "enemy_7", "enemy_8", "enemy_9" } }
                },
                
                //Informando ao sistema que o time 1 é um time de bots. Jogador não vai conseguir realizar inputs
                //nas pessas deste time
                BotTeams = new []{ 1 },

                //Identificando cada peça
                Pawns = new Dictionary<string, string>()
                {
                    //Player
                    { "player_0", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "player_1", EntityNamesConstants.ENTITY_PAWN_BISHOP },
                    { "player_2", EntityNamesConstants.ENTITY_PAWN_HORSE },
                    { "player_3", EntityNamesConstants.ENTITY_PAWN_TOWER },
                    
                    //Enemy
                    { "enemy_0", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_1", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_2", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_3", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_4", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_5", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_6", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_7", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_8", EntityNamesConstants.ENTITY_PAWN_SIMPLE },
                    { "enemy_9", EntityNamesConstants.ENTITY_PAWN_SIMPLE }
                },

                //Configurando tiles bloqueados
                BlockedTiles = blockedCoordinates.ToDictionary(k => k, v => EntityNamesConstants.ENTITY_BLOCK_1),
                
                //Configurando snapshots.
                Snapshots = new List<MatchSnapshot>()
                {
                    new MatchSnapshot()
                    {
                        Pieces = pieces
                    },
                }
            };
        }
    }
}
