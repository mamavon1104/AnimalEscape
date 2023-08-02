using System.Collections.Generic;
using UnityEngine;

public class StepOnABox : MonoBehaviour
{
    [Header("playerParent������ϐ�"), SerializeField]
    Transform playerParent;
    private void OnTriggerEnter(Collider other)
    {
        //�����A�v���C���[�̏�ɏ���ĂȂ�������A�v���C���[���e�ł��Ȃ�������
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