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
        [SerializeField] private Transform bufferPoint;

        private Rigidbody2D body;
        private Vector3 size;

        private ListSet<KeyCode> movementQueue; // Stored movement key inputs in the order they were pressed
        private Direction bufferDir; // Direction the buffer point is looking
        private Direction direction; // Current direction of the player
        private IDictionary<KeyCode, Direction> movementKeyBind;
        private KeyCode[] movementKeys;

        // Start is called before the first frame update
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            size = GetComponent<BoxCollider2D>().bounds.size;
            movementQueue = new();
            movePoint.parent = null;
            bufferPoint.parent = null;

            bufferDir = Direction.NONE;
            direction = bufferDir;
            movementKeyBind = Player1Constants.MovementKeyBind;
            movementKeys = movementKeyBind.Keys.ToArray();
        }

        // Update is called once per frame
        void Update()
        {
            // Stores pressed down movement keys into a queued set
            AddPressedMovementKeys(this.movementKeys, this.movementQueue);
            // Removes movement keys from queued set after they are released
            RemoveReleasedMovementKeys(this.movementKeys, this.movementQueue);

            // Acquire the last pressed down key within a set of pressed down keys
            int lastIndex = this.movementQueue.Count - 1;
            if (lastIndex >= 0)
            {
                Direction newDir = getDirection(this.movementQueue[lastIndex], this.movementKeyBind);
                if (newDir.GetOppositeDirection() != this.direction)
                    this.bufferDir = newDir;
            } 

            // Moves the player body towards the movePoint
            body.transform.position = Vector2.MoveTowards(body.transform.position, movePoint.position, speed * Time.deltaTime);
            // After the player reaches the movePoint, it gets moved to the location of the bufferPoint
            if (Vector2.Distance(body.transform.position, movePoint.position) <= 0f)
            {
                movePoint.position = bufferPoint.position;
                this.direction = this.bufferDir;
            }
            // The bufferPoint gets moved to a player width distance from the movePoint in the player selected direction
            bufferPoint.position = movePoint.position + GetPosDelta(this.bufferDir, this.size);

            TestUtil.timedLog(String.Format("Current direction: {0}", this.bufferDir));
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
        /// Given the direction and player size, the buffer velocity is returned to move the player in a gridlike manner
        /// </summary>
        /// <param name="dir">Current travel direction the player wishes to travel</param>
        /// <param name="size">The hitbox size of the player which will be used to move the player in a gridlike pattern</param>
        /// <returns>A vector that respresents the current delta position of the buffer point</returns>
        private Vector3 GetPosDelta(Direction dir, Vector2 size)
        {
            float xDelta, yDelta;
            switch (dir)
            {
                case Direction.NORTH:
                    xDelta = 0;
                    yDelta = size.y;
                    break;
                case Direction.SOUTH:
                    xDelta = 0;
                    yDelta = -size.y;
                    break;
                case Direction.EAST:
                    xDelta = size.x;
                    yDelta = 0;
                    break;
                case Direction.WEST:
                    xDelta = -size.x;
                    yDelta = 0;
                    break;
                case Direction.NONE:
                    xDelta = 0;
                    yDelta = 0;
                    break;
                default:
                    throw new ArgumentException(String.Format(
                        "The entered direction \"{0}\" does not have an implemented velocity", dir));
            }

            return new Vector2(xDelta, yDelta);
        }

    }
}
