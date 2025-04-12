using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public struct Path : IEquatable<Path>
    {
        public Coordinate[] Coordinates;
        public bool Inclusive;

        public bool Equals(Path other)
        {
            string[] myBoardCoordinates = Coordinates.Select(c => c.BoardCoordinate).ToArray();
            string[] otherBoardCoordinates = other.Coordinates.Select(c => c.BoardCoordinate).ToArray();
            
            return !myBoardCoordinates.Except(otherBoardCoordinates).Any()
                   && !otherBoardCoordinates.Except(myBoardCoordinates).Any();
        }

        public Path Split(Coordinate coordinate)
        {
            return Split(coordinate.BoardCoordinate);
        }

        public Path Split(string boardCoordinates)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in Coordinates)
            {
                coordinates.Add(coordinate);
                if(coordinate.BoardCoordinate == boardCoordinates)
                    break;
            }

            return coordinates.ToArray().ToPath();
        }

        public override bool Equals(object obj)
        {
            return obj is Path other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Coordinates != null ? Coordinates.GetHashCode() : 0);
        }
    }
}
