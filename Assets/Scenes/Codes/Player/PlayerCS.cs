using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PlayerInputScript))]
public class PlayerCS : MonoBehaviour
{
    #region �ϐ�����
    [Header("�J����"), SerializeField]
    private Transform thisCamera;
    private bool isSelect = false;

    [Header("���݂̃v���C���[�̏��"), SerializeField]
    private PlayerState playerState = PlayerState.Grounded;

    [Header("�v���C���[�̐��l : PlayersValue"), SerializeField]
    public MyPlayersValue playerValue;

    [Header("�q�I�u�W�F�N�g�̃X�N���v�g�i�["), SerializeField]
    private CatchPut_Items catchPutItemsCS;

    [Header("�q�I�u�W�F�N�g��ThrowToPoint"), SerializeField]
    private ThrowToPoint myThrow;

    /// <summary>
    /// �L���b�`����Ă��鎞�ɑ����CatchPutItems�������Ă��������������ĂыN�����Ă�����B
    /// </summary>
    [NonSerialized] 
    public CatchPut_Items catchPutItemsCSOfParent; 

    //�ȉ��Aprivate�ϐ�
    private Rigidbody myRig;
    private Renderer renderer;
    private Vector2 inputMove;�@// �������l
    private Transform myTrans;
    private Quaternion targetRotation;
    #endregion

    private void Awake()
    {
        myTrans = transform;
        targetRotation = myTrans.rotation;

        myRig = myTrans.GetComponent<Rigidbody>();
        renderer = myTrans.GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (!isSelect)
            return;

        //Vector3 cameraForward = Vector3.Scale(thisCamera.forward, new Vector3(1, 0, 1)).normalized;
        var horizontalRotation = Quaternion.AngleAxis(thisCamera.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(inputMove.x, 0, inputMove.y) * playerValue.speed * Time.deltaTime;
        
        // �ړ����͂�����ꍇ�́A�U�����������s��
        if (velocity.sqrMagnitude > 0f)
        {
            PlayerMove(velocity);
            targetRotation = Quaternion.LookRotation(velocity);
        }

        if (myThrow.SelectThrow == false)
        {
            if (myRig.constraints.HasFlag(RigidbodyConstraints.FreezePositionY))
            {
                myRig.constraints = myRig.constraints ^ RigidbodyConstraints.FreezePositionY;
            }
                //�L�q;
            myTrans.rotation = Quaternion.RotateTowards(myTrans.rotation, targetRotation, playerValue.playerRotateSpeed * Time.deltaTime);           
        }
        else if(!myRig.constraints.HasFlag(RigidbodyConstraints.FreezePositionY))
            myRig.constraints = myRig.constraints | RigidbodyConstraints.FreezePositionY;
    }
    private void LateUpdate()
    {
        if (playerState == PlayerState.Grounded || playerState == PlayerState.BeingCarried)
            return;

        CheckIsPlayerGrouded();
    }
    private void PlayerMove(Vector3 getVec)
    {
        if (playerState == PlayerState.BeingThrown)
            return;

        if (playerState == PlayerState.BeingCarried)
        {
            myTrans.localPosition = playerValue.leaveCarriedScale * getVec;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
            return;
        }

        //myRig.velocity = myRig.velocity + getVec;
        myRig.AddForce(getVec,ForceMode.Impulse);
    }

    /// <summary>
    /// jump�Œ��͗������n�߂�������(Falling��)
    /// Falling���͉���ray���΂�State�̐ݒ�ցB
    /// </summary>
    private void CheckIsPlayerGrouded() //�����̏�Ԃ������闷��(Find�ȊO�̊֐������v�����Ȃ�)
    {
        if (playerState == PlayerState.Falling || playerState == PlayerState.BeingThrown) //�������n�߂Ă���̂Ȃ�ray�J�n
        {
            if (Physics.Raycast(myTrans.position, Vector3.down, 0.1f + myTrans.lossyScale.y / 2, ~0, QueryTriggerInteraction.Ignore))
                ChangeState(PlayerState.Grounded);

            return;
        }
        else if (playerState == PlayerState.Jumpping && myRig.velocity.y < 0)
            ChangeState(PlayerState.Falling);
    }

    #region move,jump,change
    // ���[�u
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        inputMove = context.ReadValue<Vector2>();
    }

    // �W�����v
    public void OnJump(InputAction.CallbackContext context)
    {
        if (playerState == PlayerState.Grounded)
        {
            playerState = PlayerState.Jumpping;
            PlayerMove(new Vector3(0, playerValue.jumpSpeed, 0));
        }
        else if (playerState == PlayerState.BeingCarried)
        {
            myTrans.localPosition = Vector3.up;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
        }
    }

    /// <summary>
    /// �v���C���[�ύX���ɕK�v�Ȏ�������Ă����֐��B
    /// </summary>
    public void SetPlayerSelectionStatus(bool setBool)
    {
        isSelect = setBool;
        thisCamera.gameObject.SetActive(setBool);
        
        if (setBool)
            ChangeColor(Color.blue);
        else
            ChangeColor(Color.red);
    }
    #endregion
    private void ChangeColor(Color color)
    {
        renderer.material.color = color;
    }
    public enum PlayerState
    {
        Grounded,
        Jumpping,
        Falling,
        BeingThrown,
        BeingCarried
    }

    public void ChangeState(PlayerState getstate)
    {
        playerState = getstate;
    }
}