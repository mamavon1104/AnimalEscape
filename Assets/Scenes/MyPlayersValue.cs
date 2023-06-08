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

    [Header("�d�͉����x"), SerializeField]
    public float _gravity = 15;

    [Header("�������̑��������iInfinity�Ŗ������A���̒l�j"), SerializeField]
    public float _fallSpeed = 10;

    [Header("�����̏���"), SerializeField]
    public float _initFallSpeed = 2;

    [Header("player�������邽�߁A��ւ�ray����(0.08015�ŃL�����ǂ�����o����)"), SerializeField]
    public float upCheckDistance;

    [Header("�����̃��C���[�̖��O(Player)"), SerializeField]
    public LayerMask playerLayers;

    [HideInInspector] ///<remarks> Vector2(0,0)�������Ă��܂��B </remarks>
    public Vector2 zeroVec = Vector2.zero;
}