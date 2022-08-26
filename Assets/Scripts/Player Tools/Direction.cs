using UnityEngine;

namespace Players
{
    public enum Direction
    {
        NORTH, SOUTH, EAST, WEST, NONE
    }

    public static class DirectionExtension
    {
        public static Direction GetOppositeDirection(this Direction dir)
        {
            Direction reverseDir;
            switch (dir)
            {
                case Direction.NORTH:
                    reverseDir = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    reverseDir = Direction.NORTH;
                    break;
                case Direction.EAST:
                    reverseDir = Direction.WEST;
                    break;
                case Direction.WEST:
                    reverseDir = Direction.EAST;
                    break;
                case Direction.NONE:
                    reverseDir = Direction.NONE;
                    break;
                default:
                    throw new System.ArgumentException("The entered direction does not have a mapped reverse direction");
            }
            return reverseDir;
        }
    }

}
