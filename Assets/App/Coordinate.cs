using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App.Services.BoardService;
using ppl.Services.Core;
using UnityEngine;

namespace App
{
    public enum Direction
    {
        UP,
        LEFT,
        RIGHT,
        DOWN,
        UP_RIGHT,
        UP_LEFT,
        DOWN_RIGHT,
        DOWN_LEFT
    }
    
    [System.Serializable]
    public struct Coordinate : IEquatable<Coordinate>
    {
        public int Horizontal;
        public int Vertical;
        public string BoardCoordinate;
        public Vector2 Center;

        static Dictionary<int, char> _alphabetDict = new Dictionary<int, char>
        {
            { 0, 'a' },
            { 1, 'b' },
            { 2, 'c' },
            { 3, 'd' },
            { 4, 'e' },
            { 5, 'f' },
            { 6, 'g' },
            { 7, 'h' },
            { 8, 'i' },
            { 9, 'j' },
            { 10, 'k' },
            { 11, 'l' },
            { 12, 'm' },
            { 13, 'n' },
            { 14, 'o' },
            { 15, 'p' },
            { 16, 'q' },
            { 17, 'r' },
            { 18, 's' },
            { 19, 't' },
            { 20, 'u' },
            { 21, 'v' },
            { 22, 'w' },
            { 23, 'x' },
            { 24, 'y' }
        };

        public static string ToCoordinateString(int horizontal, int vertical)
        {
            return $"{_alphabetDict[horizontal]}{vertical}";
        }
        
        public static Coordinate ToCoordinate(string coordinate)
        {
            var match = Regex.Match(coordinate, @"^([a-zA-Z])(\d+)$");

            if (!match.Success)
            {
                return new Coordinate
                {
                    Horizontal = 0,
                    Vertical = 0,
                };
            }

            char letter = char.ToLower(match.Groups[1].Value[0]);
            int number = int.Parse(match.Groups[2].Value);

            if (!_alphabetDict.Values.Contains(letter) || number < 0 || number >= _alphabetDict.Count)
            {
                return new Coordinate
                {
                    Horizontal = 0,
                    Vertical = 0,
                };
            }
            
            int horizontal = letter - 'a';
            int vertical = number;

            Vector2 center = Vector2.zero;
            ServiceContainer.UseService<IBoardService>(boardService =>
            {
                center = boardService.GetCenter(coordinate);
            });
            
            return ToCoordinate(horizontal, vertical, center);
        }
        public static Coordinate ToCoordinate(int horizontal, int vertical, Vector2 center)
        {
            if (horizontal < 0)
                horizontal = 0;
            else if(horizontal >= _alphabetDict.Count)
                horizontal = _alphabetDict.Count - 1;
            
            if (vertical < 0)
                vertical = 0;
            else if(vertical >= _alphabetDict.Count)
                vertical = _alphabetDict.Count - 1;
            
            return new Coordinate
            {
                Horizontal = horizontal,
                Vertical = vertical,
                Center = center,
                BoardCoordinate = ToCoordinateString(horizontal, vertical)
            };
        }

        public bool Equals(Coordinate other)
        {
            return Horizontal == other.Horizontal && Vertical == other.Vertical && BoardCoordinate == other.BoardCoordinate && Center.Equals(other.Center);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(Horizontal, Vertical, BoardCoordinate, Center);
        }
    }
}
