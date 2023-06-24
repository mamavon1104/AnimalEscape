using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MyPlayersValue : ScriptableObject
{
    [Header("�ړ��̑���"), SerializeField]
    public float _speed = 3;

    [Header("�W�����v�����"), SerializeField]
    public float _jumpSpeed = 7;

    [Header("�������̑��������iInfinity�Ŗ������A���̒l�j"), SerializeField]
    public float _fallSpeed = 10;
}