using UnityEngine;


namespace Players 
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float speed;
        private Rigidbody2D body;
        private ListSet<KeyCode> inputs;
        private KeyCode[] p1MovementKeys = new KeyCode[] {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };

        // Start is called before the first frame update
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            inputs = new();
        }

        // Update is called once per frame
        void Update()
        {
            addPressedMovementKeys(p1MovementKeys);
            removeReleasedMovementKeys(p1MovementKeys);

            int lastIndex = inputs.Count - 1;
            if (lastIndex >= 0)
                body.velocity = getVelocity(inputs[lastIndex]);
        }

        /// <summary>
        /// Any keys that are released, are immediately removed from inputs
        /// </summary>
        private void removeReleasedMovementKeys(KeyCode[] movementKeys)
        {
            foreach (KeyCode key in movementKeys)
            {
                if (Input.GetKeyUp(key))
                    inputs.Remove(key);
            }
            

        // This might be better for performance? but the above is more flexible
            //if (Input.GetKeyUp(KeyCode.UpArrow))
            //{
            //    inputs.Remove(KeyCode.UpArrow);
            //}
            //if (Input.GetKeyUp(KeyCode.LeftArrow))
            //{
            //    inputs.Remove(KeyCode.LeftArrow);
            //}
            //if (Input.GetKeyUp(KeyCode.RightArrow))
            //{
            //    inputs.Remove(KeyCode.RightArrow);
            //}
            //if (Input.GetKeyUp(KeyCode.DownArrow))
            //{
            //    inputs.Remove(KeyCode.DownArrow);
            //}
        }

        /// <summary>
        /// Will retrieve the last key which had a key down event, not necessarily key that is still pressed down
        /// </summary>
        /// <returns>Last key to have a key down event</returns>
        private void addPressedMovementKeys(KeyCode[] movementKeys)
        {
            foreach (KeyCode key in movementKeys)
            {
                if (Input.GetKeyDown(key))
                    inputs.Add(key);
            }
        }

        /// <summary>
        /// Given the input key, will return the current velocity of the player
        /// </summary>
        /// <param name="keyCode">Input key by the user</param>
        /// <returns>A vector that respresents the velocity being traveled</returns>
        private Vector2 getVelocity(KeyCode keyCode)
        {
            float xVel = keyCode == KeyCode.LeftArrow ? -speed
                : keyCode == KeyCode.RightArrow ? speed
                : 0;
            float yVel = keyCode == KeyCode.UpArrow ? speed
                : keyCode == KeyCode.DownArrow ? -speed
                : 0;

            return new Vector2(xVel, yVel);
        }
    }

}
