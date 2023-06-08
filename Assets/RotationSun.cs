using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSun : MonoBehaviour
{
    float math;
    void Start()
    {
       math = GameObject.Find("Sphere").GetComponent<MathMove>().speed;
    }
    private void Update()
    {
        transform.Rotate(new Vector3(math / 360,0,0));
    }
}
