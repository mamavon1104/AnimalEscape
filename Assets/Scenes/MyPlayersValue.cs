using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MyPlayersValue : ScriptableObject
{
    [Header("移動の速さ")]
    public float speed = 3;

    [Header("ジャンプする力")]
    public float jumpSpeed = 7;

    [Header("落下時の速さ制限（Infinityで無制限、正の値）")]
    public float fallSpeed = 10;

    [Header("投げる方向 angle"), Range(30f, 65f)]
    public float throwAngle;

    [Header("Playerの回転スピード")]
    public float playerRotateSpeed;

    /// <summary>
    /// 
    /// </summary>
    [Header("おんぶされている後の動く量")]
    public float leaveCarriedScale;
}