using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour
{
    public float speed = 0.05f; 
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(speed * -1, 0,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0, speed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(0, speed * -1, 0);
        }
    }
}
