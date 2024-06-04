using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathMove : MonoBehaviour
{
    [SerializeField] float x = 0;
    [SerializeField] float y = 0;
    [SerializeField] float z = 0;
    [SerializeField] float r = 5;
    [SerializeField] float theta = 0;

    [Header("関数の高さ、デカいほど高く"), SerializeField]
    float FuncHighet;
    [Header("関数の幅、小さいほど広く"),SerializeField]
    float Funcwidth;
    [Header("大きければ大きいほど早い"), SerializeField]
    public float speed;
    [Header("trueならSinCos、ちがければtan"), SerializeField] 
    private bool moveCircle;
    private void Update()
    {
        if (moveCircle)
        {
            x = Mathf.Cos(theta) * r;
            z = Mathf.Sin(theta) * r;
            y = Mathf.Sin(theta * Funcwidth) * FuncHighet;
        }
        else
        {
            x = theta - 20;
            x = 0.5f;
            z = Mathf.Tan(theta * Funcwidth);
        }
            transform.position = new Vector3(x,y,z);
        theta += (speed * (Mathf.PI / 360));
    }
}
