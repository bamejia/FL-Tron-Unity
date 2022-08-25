using System;
using UnityEngine;
using Utils;


namespace Players 
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float speed;
        private Rigidbody2D body;
        private ListSet<KeyCode> inputs;

        // Start is called before the first frame update
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            inputs = new();
        }

        // Update is called once per frame
        void Update()
        {
            //float xVel = Input.GetAxis("Horizontal") * speed;
            //float yVel = Input.GetAxis("Vertical") * speed;

            KeyCode key = getLastPressedDownKey();
            if (key != KeyCode.None)
                inputs.Add(key);
            removeReleasedKeys();

            int lastIndex = inputs.Count - 1;
            if (lastIndex >= 0)
                body.velocity = getVelocity(inputs[lastIndex]);
        }

        /// <summary>
        /// Any keys that are released, are immediately removed from inputs
        /// </summary>
        private void removeReleasedKeys()
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                inputs.Remove(KeyCode.UpArrow);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                inputs.Remove(KeyCode.LeftArrow);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                inputs.Remove(KeyCode.RightArrow);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                inputs.Remove(KeyCode.DownArrow);
            }
        }

        /// <summary>
        /// Will retrieve the last key which had a key down event, not necessarily key that is still pressed down
        /// </summary>
        /// <returns>Last key to have a key down event</returns>
        private KeyCode getLastPressedDownKey()
        {
            return Input.GetKeyDown(KeyCode.UpArrow) ? KeyCode.UpArrow
                : Input.GetKeyDown(KeyCode.DownArrow) ? KeyCode.DownArrow
                : Input.GetKeyDown(KeyCode.LeftArrow) ? KeyCode.LeftArrow
                : Input.GetKeyDown(KeyCode.RightArrow) ? KeyCode.RightArrow
                : KeyCode.None;
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
