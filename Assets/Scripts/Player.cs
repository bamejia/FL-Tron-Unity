using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;



namespace Players
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float speed;
        private Rigidbody2D body;
        private ListSet<KeyCode> inputs;
        private Direction direction;
        private IDictionary<KeyCode, Direction> movementKeyBind;
        private KeyCode[] movementKeys;

        private Logger logger = new Logger(Debug.unityLogger.logHandler);

        // Start is called before the first frame update
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            inputs = new();

            direction = Direction.NONE;
            movementKeyBind = Player1Constants.MovementKeyBind;
            movementKeys = movementKeyBind.Keys.ToArray();
        }

        // Update is called once per frame
        void Update()
        {
            AddPressedMovementKeys(this.movementKeys);
            RemoveReleasedMovementKeys(this.movementKeys);
            //logger.Log(string.Format("Last active pressed key: {0}", lastActiveMovementKey));

            int lastIndex = inputs.Count - 1;
            if (lastIndex >= 0)
            {
                Direction newDir = getDirection(inputs[lastIndex], this.movementKeyBind);
                if (newDir.GetOppositeDirection() != this.direction)
                {
                    body.velocity = GetVelocity(newDir);
                    this.direction = newDir;
                }

            }
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
        private void AddPressedMovementKeys(KeyCode[] movementKeys)
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
        private void RemoveReleasedMovementKeys(KeyCode[] movementKeys)
        {
            foreach (KeyCode key in movementKeys)
            {
                if (Input.GetKeyUp(key))
                    inputs.Remove(key);
            }
        }

        /// <summary>
        /// Given the direction, function will return the current velocity of the player
        /// </summary>
        /// <param name="dir">Current travel direction of the player</param>
        /// <returns>A vector that respresents the current velocity of the player</returns>
        private Vector2 GetVelocity(Direction dir)
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
                default:
                    throw new ArgumentException(String.Format(
                        "The entered direction \"{0}\" does not have an implemented velocity", dir));
            }

            return new Vector2(xVel, yVel);
        }
    }

}
