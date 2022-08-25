using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Players 
{
    public class Player : MonoBehaviour
    {

        [SerializeField] private float speed;
        private Rigidbody2D body;
        private KeyCode _lastKey;
        private KeyCode LastKey
        {
            get
            {
                return _lastKey;
            }
            set
            {
                if (value != _lastKey)
                {
                    prevKey = _lastKey;
                }
                _lastKey = value;
            }
        }

        private KeyCode prevKey;

        // Start is called before the first frame update
        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            //float xVel = Input.GetAxis("Horizontal") * speed;
            //float yVel = Input.GetAxis("Vertical") * speed;

            LastKey = Input.GetKeyDown(KeyCode.UpArrow) ? KeyCode.UpArrow
                : Input.GetKeyDown(KeyCode.DownArrow) ? KeyCode.DownArrow
                : Input.GetKeyDown(KeyCode.LeftArrow) ? KeyCode.LeftArrow
                : Input.GetKeyDown(KeyCode.RightArrow) ? KeyCode.RightArrow
                : LastKey;

            if (Input.GetKeyUp(prevKey))
            {
                prevKey = LastKey;
            }

            Vector2 newVelocity;
            if (Input.GetKey(LastKey))
            {
                newVelocity = getVelocity(LastKey);
            }
            else if (!Input.anyKey)
            {
                newVelocity = body.velocity;
                // Uncomment to stop when no keys are pressed
                //curKey = KeyCode.None;
            }
            else if (Input.GetKey(prevKey))
            {
                newVelocity = getVelocity(prevKey);
            }
            else
            {
                newVelocity = getVelocity(LastKey);
            }

            body.velocity = newVelocity;
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
