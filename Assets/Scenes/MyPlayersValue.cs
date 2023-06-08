using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MyPlayersValue : ScriptableObject
{
    [Header("移動の速さ"), SerializeField]
    public float _speed = 3;

    [Header("ジャンプする力"), SerializeField]
    public float _jumpSpeed = 7;

    [Header("重力加速度"), SerializeField]
    public float _gravity = 15;

    [Header("落下時の速さ制限（Infinityで無制限、正の値）"), SerializeField]
    public float _fallSpeed = 10;

    [Header("落下の初速"), SerializeField]
    public float _initFallSpeed = 2;

    [Header("player判定を取るため、上へのray距離(0.08015でキリが良く判定出来る)"), SerializeField]
    public float upCheckDistance;

    [Header("自分のレイヤーの名前(Player)"), SerializeField]
    public LayerMask playerLayers;

    [HideInInspector] ///<remarks> Vector2(0,0)が入っています。 </remarks>
    public Vector2 zeroVec = Vector2.zero;
}