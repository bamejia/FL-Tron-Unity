using UnityEngine;


namespace Players 
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float speed;
        private Rigidbody2D body;
        private ListSet<KeyCode> inputs;
        private KeyCode lastActiveMovementKey = KeyCode.None;
        private KeyCode[] p1MovementKeys = new KeyCode[] {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow
        };

        private Logger logger = new Logger(Debug.unityLogger.logHandler);

        // Start is called before the first frame update
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            inputs = new();
        }

        // Update is called once per frame
        void Update()
        {
            AddPressedMovementKeys(p1MovementKeys);
            RemoveReleasedMovementKeys(p1MovementKeys);
            //logger.Log(string.Format("Last active pressed key: {0}", lastActiveMovementKey));

            int lastIndex = inputs.Count - 1;
            if (lastIndex >= 0)
            {
                KeyCode activeKey = inputs[lastIndex];
                if (GetReverseInput(activeKey) is KeyCode reverseKey
                    && !inputs.Contains(reverseKey)
                    && (inputs.Count == 1 ? reverseKey != lastActiveMovementKey : true))
                    body.velocity = GetVelocity(activeKey);
            }
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
                {
                    if (inputs.Remove(key) && inputs.Count == 0 && lastActiveMovementKey != GetReverseInput(key))
                        lastActiveMovementKey = key;
                }
                
            }
            

        // TODO This might be better for performance? but the above is more flexible. Delete if unnecessary
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
        /// Given the input key, will return the current velocity of the player
        /// </summary>
        /// <param name="keyCode">Input key by the user</param>
        /// <returns>A vector that respresents the velocity being traveled</returns>
        private Vector2 GetVelocity(KeyCode keyCode)
        {
            float xVel = keyCode == KeyCode.LeftArrow ? -speed
                : keyCode == KeyCode.RightArrow ? speed
                : 0;
            float yVel = keyCode == KeyCode.UpArrow ? speed
                : keyCode == KeyCode.DownArrow ? -speed
                : 0;

            return new Vector2(xVel, yVel);
        }

        /// <summary>
        /// Get the movement key input that is the reverse direction of what is given
        /// </summary>
        /// <param name="key">Key to find the reverse input of</param>
        /// <returns>Reverse key</returns>
        private KeyCode GetReverseInput(KeyCode key)
        {
            KeyCode reverseKey;
            if (key == KeyCode.UpArrow)
                reverseKey = KeyCode.DownArrow;
            else if (key == KeyCode.DownArrow)
                reverseKey = KeyCode.UpArrow;
            else if (key == KeyCode.LeftArrow)
                reverseKey = KeyCode.RightArrow;
            else if (key == KeyCode.RightArrow)
                reverseKey = KeyCode.LeftArrow;
            else
                throw new System.ArgumentException("The entered movement input key does not have a mapped reverse key");

            return reverseKey;
        }
    }

}
