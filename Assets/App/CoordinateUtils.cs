using Unity.Mathematics;

namespace App
{
    public static class CoordinateUtils
    {
        public static Path ToPath(this Coordinate[] coordinates, bool inclusive = false)
        {
            return new Path
            {
                Coordinates = coordinates,
                Inclusive = inclusive
            };
        }

        public static (int x, int y) AddCoordinatesInt(int horizontal, int vertical, Direction direction)
        {
            int x = horizontal;
            int y = vertical;

            switch (direction)
            {
                case Direction.UP:
                    y += 1;
                    break;
                case Direction.DOWN:
                    y -= 1;
                    break;
                case Direction.LEFT:
                    x -= 1;
                    break;
                case Direction.RIGHT:
                    x += 1;
                    break;
                case Direction.UP_RIGHT:
                    x += 1;
                    y += 1;
                    break;
                case Direction.UP_LEFT:
                    x -= 1;
                    y += 1;
                    break;
                case Direction.DOWN_RIGHT:
                    x += 1;
                    y -= 1;
                    break;
                case Direction.DOWN_LEFT:
                    x -= 1;
                    y -= 1;
                    break;
            }
            
            return (x, y);
        }
    }
}