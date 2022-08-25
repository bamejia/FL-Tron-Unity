using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // private RigidBody2D body;
    // Start is called before the first frame update
    private Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float xVel = Input.GetAxis("horizontal");
        float yVel = Input.GetAxis("vertical");

        body.velocity = new Vector3(xVel, yVel);
    }
}
