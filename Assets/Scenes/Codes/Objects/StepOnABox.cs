using System.Collections.Generic;
using UnityEngine;

public class StepOnABox : MonoBehaviour
{
    [Header("playerParentを入れる変数"), SerializeField]
    Transform playerParent;
    private void OnTriggerEnter(Collider other)
    {
        //もし、プレイヤーの上に乗ってなかったり、プレイヤーが親でもなかったら
        if (other.CompareTag("Player") && other.transform.parent.gameObject.layer != 3) 
        {
            other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.parent.gameObject.layer != 3)
        {
            other.transform.parent = playerParent;  
        }
    }
}