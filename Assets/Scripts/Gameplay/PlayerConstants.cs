using System.Collections.Generic;
using UnityEngine;
using Navigation;
using System;

namespace Gameplay
{
    public static class PlayerConstants
    {
        public static readonly float DEFAULT_SPEED = 15;

        private static readonly IDictionary<KeyCode, Direction> MainPlayerMovementKeyBind = 
            new Dictionary<KeyCode, Direction>()
            {
                [KeyCode.UpArrow] = Direction.NORTH,
                [KeyCode.DownArrow] = Direction.SOUTH,
                [KeyCode.LeftArrow] = Direction.WEST,
                [KeyCode.RightArrow] = Direction.EAST
            };

        private static readonly IDictionary<KeyCode, Direction> LocalSecondPlayerMovementKeyBind =
            new Dictionary<KeyCode, Direction>()
            {
                [KeyCode.W] = Direction.NORTH,
                [KeyCode.S] = Direction.SOUTH,
                [KeyCode.A] = Direction.WEST,
                [KeyCode.D] = Direction.EAST
            };

        public static IDictionary<KeyCode, Direction> getPlayerMovementKeyBind(PlayType type, DesignatedPlayer selected)
        {
            if (type == PlayType.LOCAL && selected == DesignatedPlayer.TWO)
            {
                return LocalSecondPlayerMovementKeyBind;
            } else if (type == PlayType.LOCAL && selected != DesignatedPlayer.ONE)
            {
                throw new ArgumentException("No keybinds for players 3 or greater in local multiplayer");
            } else
            {
                return MainPlayerMovementKeyBind;
            }
        }
    }
}
