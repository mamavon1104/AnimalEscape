using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowToPoint : MonoBehaviour
{
    private CatchPut_Items catchPutItemsCS;
    [SerializeField]
    private Transform PlayerPos;

    [Header("������|�W�V����"), SerializeField]
    private Transform throwPointNow;
    /// <summary>
    /// throwPointNow��position
    /// </summary>
    private Vector3 throwPoint;

    private float moveDuration = 2f; // �ړ��ɂ����鎞��

    private float moveTimer = 0f; // �ړ��̌o�ߎ���
    private Vector3 startPos; // �ړ��̊J�n�ʒu

    private bool isMoving = false; // �ړ������ǂ���
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
            // 2�b�Ԃňړ�����
            float t = moveTimer / moveDuration;
            PlayerPos.position = Vector3.Lerp(startPos, throwPoint, t);
        }
        else
        {
            // �ړ��������������~
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
