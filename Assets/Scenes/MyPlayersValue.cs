using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MyPlayersValue : ScriptableObject
{
    [Header("�ړ��̑���")]
    public float _speed = 3;

    [Header("�W�����v�����")]
    public float _jumpSpeed = 7;

    [Header("�������̑��������iInfinity�Ŗ������A���̒l�j")]
    public float _fallSpeed = 10;

    [Header("��������� angle"), Range(0F, 90F)]
    public float _throwAngle;

    [Header("Player�̉�]�X�s�[�h")]
    public float playerRotateSpeed;
}