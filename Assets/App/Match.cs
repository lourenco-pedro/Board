using System.Collections.Generic;

namespace App
{ 
// Estrutura de dados que representa uma partida.
// Contém todas as informações necessárias para a execução da partida em si.
//
// Tentei focar em uma arquitetura orientada a dados.
// Features que dariam para ser implementadas com o que tem no projeto:
// - Sistema de salvamento de partidas: possibilita compartilhar sua partida com amigos. Podendo importar um arquivo
// de save, pro exemplo
// - Death match de N jogadores.
// - History Navigation
//
// Aproveitei, e resolvi puxar um pouco do que o Chess.com tem. Um sistema de "Snapshots",
// Sistema que permite navegar pelo histórico completo da partida.
//
// Isso abre espaço para recursos como:
// - Histórico de partidas em jogos multiplayer: permite verificar se todas as jogadas foram válidas,
//   ajudando a identificar possíveis trapaças.
// - Replay da partida: assista às melhores jogadas, navegando entre os snapshots para rever os melhores momentos.
//
// O sistema foi projetado para suportar mais de dois jogadores,
// permitindo a criação de modos de jogo como Deathmatch com N jogadores.
//
//Pls. refer to MatchStarterTypeFactory.cs para observar as diferentes formas de configurar a Match

    public class Match
    {
        public Dictionary<string, string> Pawns;
        public Dictionary<int, string[]> Teams;
        public int[] BotTeams;
        public Dictionary<string, string> BlockedTiles;
        public List<MatchSnapshot> Snapshots;
    }
}