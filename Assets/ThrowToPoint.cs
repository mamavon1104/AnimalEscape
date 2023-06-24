using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowToPoint : MonoBehaviour
{
    private CatchPut_Items catchPutItemsCS;
    [SerializeField]
    private Transform PlayerPos;

    [Header("投げるポジション"), SerializeField]
    private Transform throwPointNow;
    /// <summary>
    /// throwPointNowのposition
    /// </summary>
    private Vector3 throwPoint;

    private float moveDuration = 2f; // 移動にかかる時間

    private float moveTimer = 0f; // 移動の経過時間
    private Vector3 startPos; // 移動の開始位置

    private bool isMoving = false; // 移動中かどうか
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            StartMovement();
        }
        if (isMoving)
        {
            MovePlayer();
        }
    }

    private void StartMovement()
    {
        startPos = PlayerPos.position;
        moveTimer = 0f;
        isMoving = true;
    }

    private void MovePlayer()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer < moveDuration)
        {
            // 2秒間で移動する
            float t = moveTimer / moveDuration;
            PlayerPos.position = Vector3.Lerp(startPos, throwPoint, t);
        }
        else
        {
            // 移動が完了したら停止
            PlayerPos.position = throwPoint;
            isMoving = false;
        }
    }
    private void Start()
    {
        throwPoint = throwPointNow.position;
        catchPutItemsCS = transform.GetComponent<CatchPut_Items>();
    }
}
