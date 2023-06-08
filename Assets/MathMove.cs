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

    [Header("�֐��̍����A�f�J���قǍ���"), SerializeField]
    float FuncHighet;
    [Header("�֐��̕��A�������قǍL��"),SerializeField]
    float Funcwidth;
    [Header("�傫����Α傫���قǑ���"), SerializeField]
    public float speed;
    [Header("true�Ȃ�SinCos�A���������tan"), SerializeField] 
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
