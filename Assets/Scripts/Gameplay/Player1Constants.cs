using System.Collections.Generic;
using UnityEngine;
using Navigation;

namespace Gameplay
{
    public static class Player1Constants
    {
        public static readonly IDictionary<KeyCode, Direction> MovementKeyBind = new Dictionary<KeyCode, Direction>()
        {
            [KeyCode.UpArrow] = Direction.NORTH,
            [KeyCode.DownArrow] = Direction.SOUTH,
            [KeyCode.LeftArrow] = Direction.WEST,
            [KeyCode.RightArrow] = Direction.EAST
        };
    }
}
