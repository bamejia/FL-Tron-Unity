using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using CustomCollection;
using Util;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform movePoint;

        private Rigidbody2D body;
        private ListSet<KeyCode> inputs;
        private Direction direction;
        private IDictionary<KeyCode, Direction> movementKeyBind;
        private KeyCode[] movementKeys;

        // Start is called before the first frame update
        void Awake()
        {

            body = GetComponent<Rigidbody2D>();
            inputs = new();
            movePoint.parent = null;

            direction = Direction.NONE;
            movementKeyBind = Player1Constants.MovementKeyBind;
            movementKeys = movementKeyBind.Keys.ToArray();
        }

        // Update is called once per frame
        void Update()
        {
            AddPressedMovementKeys(this.movementKeys, this.inputs);
            RemoveReleasedMovementKeys(this.movementKeys, this.inputs);

            int lastIndex = this.inputs.Count - 1;
            if (lastIndex >= 0)
            {
                Direction newDir = getDirection(this.inputs[lastIndex], this.movementKeyBind);
                if (newDir.GetOppositeDirection() != this.direction)
                    this.direction = newDir;
            }

            body.velocity = GetVelocity(this.direction, this.speed);
            TestUtil.timedLog(String.Format("Current direction: {0}", this.direction));
        }

        /// <summary>
        /// Retrieves the current direction of the player based on the key input and the player movement mapping
        /// </summary>
        /// <param name="key">The movement key that is currently active</param>
        /// <param name="movementKeyBind">The movement key mapping</param>
        /// <returns>The direction associated with the active key</returns>
        private Direction getDirection(KeyCode key, IDictionary<KeyCode, Direction> movementKeyBind)
        {
            if (movementKeyBind is null)
                throw new ArgumentNullException(nameof(movementKeyBind));

            Direction direction;
            if (!movementKeyBind.TryGetValue(key, out direction))
                throw new ArgumentException("Key is not mapped to any direction");
            return direction;
        }


        /// <summary>
        /// Will store movement keys which have had a key down event, not key that has been continually pressed down
        /// </summary>
        /// <param name="movementKeys">The movement keys which will be read</param>
        /// <param name="inputs">The buffer of inputs made by the player</param>
        private void AddPressedMovementKeys(KeyCode[] movementKeys, ListSet<KeyCode> inputs)
        {
            foreach (KeyCode key in movementKeys)
            {
                if (Input.GetKeyDown(key))
                    inputs.Add(key);
            }
        }

        /// <summary>
        /// Any keys that are released, are immediately removed from inputs
        /// </summary>
        /// <param name="movementKeys">The movement keys which will be read</param>
        /// <param name="inputs">The buffer of inputs made by the player</param>
        private void RemoveReleasedMovementKeys(KeyCode[] movementKeys, ListSet<KeyCode> inputs)
        {
            foreach (KeyCode key in movementKeys)
            {
                if (Input.GetKeyUp(key))
                    inputs.Remove(key);
            }
        }

        /// <summary>
        /// Given the direction and speed, function will return the current velocity of the player
        /// </summary>
        /// <param name="dir">Current travel direction of the player</param>
        /// <param name="speed">Speed which the player will travel at in the specified direction</param>
        /// <returns>A vector that respresents the current velocity of the player</returns>
        private Vector2 GetVelocity(Direction dir, float speed)
        {
            float xVel, yVel;
            switch (dir)
            {
                case Direction.NORTH:
                    xVel = 0;
                    yVel = speed;
                    break;
                case Direction.SOUTH:
                    xVel = 0;
                    yVel = -speed;
                    break;
                case Direction.EAST:
                    xVel = speed;
                    yVel = 0;
                    break;
                case Direction.WEST:
                    xVel = -speed;
                    yVel = 0;
                    break;
                case Direction.NONE:
                    xVel = 0;
                    yVel = 0;
                    break;
                default:
                    throw new ArgumentException(String.Format(
                        "The entered direction \"{0}\" does not have an implemented velocity", dir));
            }

            return new Vector2(xVel, yVel);
        }

    }
}
