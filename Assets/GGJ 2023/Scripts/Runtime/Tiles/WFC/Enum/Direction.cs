namespace GGJRuntime
{
    public enum Direction
    {
        Up      = 0,
        Down    = 1,
        Left    = 2,
        Right   = 3
    }


    public static class DirectionUtility
    {
        public static Direction GetOppositeDirection(this Direction direction)
        {
            switch(direction)
            {
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
            }

            return direction;
        }
    }
}