using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MyPlayersValue : ScriptableObject
{
    [Header("�ړ��̑���")]
    public float speed = 3;

    [Header("�W�����v�����")]
    public float jumpSpeed = 7;

    [Header("�������̑��������iInfinity�Ŗ������A���̒l�j")]
    public float fallSpeed = 10;

    [Header("��������� angle"), Range(30f, 65f)]
    public float throwAngle;

    [Header("Player�̉�]�X�s�[�h")]
    public float playerRotateSpeed;

    /// <summary>
    /// 
    /// </summary>
    [Header("����Ԃ���Ă����̓�����")]
    public float leaveCarriedScale;
}