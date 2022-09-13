using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using Navigation;
using CustomCollection;
using Util;

namespace Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform movePoint;
        [SerializeField] private Transform bufferPoint;
        [SerializeField] private PlayType playType;
        [SerializeField] private DesignatedPlayer designation;
        [SerializeField] private GameObject baseTrail;

        private float speed;
        internal Rigidbody2D body;
        private new BoxCollider2D collider;
        private new SpriteRenderer renderer;
  
        private readonly ListSet<KeyCode> movementQueue = new(); // Stored movement key inputs in the order they were pressed
        internal Direction bufferDir; // Direction the buffer point is looking
        internal Direction direction; // Current direction of the player
        private IDictionary<KeyCode, Direction> movementKeyBind;
        private KeyCode[] movementKeys;
        private PlayerTrailGenerator trailGenerator;

        internal readonly List<GameObject> trail = new(); // Holds all the create trail objects created when a user passes a tile
        internal bool updateTrail = false;
        internal bool directionsMatch = false;
        internal Vector3 newTrailPos = new();

        private bool isAlive = true;
        private float _onDeathColorReduction = 0; // On death, the ratio to multiple that the color will be dimmed
        private byte OnDeathColorDimPrecentage
        {
            get => Convert.ToByte(_onDeathColorReduction * 100f);
            set
            {
                if (value > 100) value = 100;
                _onDeathColorReduction = (float)((100f - Convert.ToSingle(value)) / 100f);
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            OnDeathColorDimPrecentage = 40;
            trailGenerator = new(this, baseTrail);

            body = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
            renderer = GetComponent<SpriteRenderer>();

            movePoint.parent = null;
            bufferPoint.parent = null;

            bufferDir = Direction.NONE;
            direction = bufferDir;
            movementKeyBind = PlayerConstants.getPlayerMovementKeyBind(playType, designation);
            movementKeys = movementKeyBind.Keys.ToArray();
            speed = PlayerConstants.DEFAULT_SPEED;
        }

        // Update is called once per frame
        void Update()
        {
            // Stores pressed down movement keys into a queued set
            this.AddPressedMovementKeys(this.movementKeys, this.movementQueue);
            // Removes movement keys from queued set after they are released
            this.RemoveReleasedMovementKeys(this.movementKeys, this.movementQueue);

            // Acquire the last pressed down key within a set of pressed down keys
            int lastIndex = this.movementQueue.Count - 1;
            if (lastIndex >= 0)
            {
                Direction newDir = getDirection(this.movementQueue[lastIndex], this.movementKeyBind);
                if (newDir.GetOppositeDirection() != this.direction)
                    this.bufferDir = newDir;
            } 

            // Moves the player body towards the movePoint
            if (this.isAlive)
                body.transform.position = Vector2.MoveTowards(
                    body.transform.position, movePoint.position, speed * Time.deltaTime);
            // After the player reaches the movePoint, it gets moved to the location of the bufferPoint
            if (Vector2.Distance(body.transform.position, movePoint.position) <= 0f)
            {
                // Flag to let the player trail generator know to create or update the trail
                this.updateTrail = true;
                this.directionsMatch = this.direction == this.bufferDir;
                this.newTrailPos = this.movePoint.transform.position;

                // MovePoint is moved to the bufferPoint, and now has the same direction
                movePoint.position = bufferPoint.position;
                this.direction = this.bufferDir;
                this.trailGenerator.Generate();
            }
            // The bufferPoint gets moved to a player width distance from the movePoint in the player selected direction
            bufferPoint.position = movePoint.position + GetPosDelta(this.bufferDir, this.renderer.bounds.size);

            //TestUtil.TimedLog(String.Format("Current direction: {0}", this.bufferDir));
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

        public void OnCollisionEnter2D(Collision2D collision)
        {
            Color deathColor = new Color(
            renderer.color.r * _onDeathColorReduction, 
                renderer.color.g * _onDeathColorReduction, 
                renderer.color.b * _onDeathColorReduction, 
                renderer.color.a
                );
            this.renderer.color = deathColor;
            this.trail.ForEach(t => t.GetComponent<SpriteRenderer>().color = deathColor);

            this.isAlive = false;
            //TestUtil.Log("deathColor: {0}", deathColor);
        }
    }
}
