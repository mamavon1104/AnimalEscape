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
    private Transform _thisCamera;
    private bool _isSelect = false;

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
    private Rigidbody _myRig;
    private Vector2 _inputMove;�@// �������l
    private Transform _myTrans;
    private Quaternion _targetRotation;
    #endregion

    public enum PlayerState
    {
        Grounded,
        Jumpping,
        Falling,
        BeingThrown,
        BeingCarried
    }

    private void Awake()
    {
        _myTrans = transform;
        _targetRotation = _myTrans.rotation;

        _myRig = _myTrans.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!_isSelect)
            return;

        //Vector3 cameraForward = Vector3.Scale(thisCamera.forward, new Vector3(1, 0, 1)).normalized;
        var horizontalRotation = Quaternion.AngleAxis(_thisCamera.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(_inputMove.x, 0, _inputMove.y) * playerValue.speed * Time.deltaTime;
        
        // �ړ����͂�����ꍇ�́A�U�����������s��
        if (velocity.sqrMagnitude > 0f)
        {
            PlayerMove(velocity);
            _targetRotation = Quaternion.LookRotation(velocity);
        }

        if (myThrow.SelectThrow == false)
        {
            if (_myRig.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY))
            {
                _myRig.constraints = _myRig.constraints ^ RigidbodyConstraints.FreezeRotationY;
            }
                //�L�q;
            _myTrans.rotation = Quaternion.RotateTowards(_myTrans.rotation, _targetRotation, playerValue.playerRotateSpeed * Time.deltaTime);           
        }
        else if(!_myRig.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY))
            _myRig.constraints = _myRig.constraints | RigidbodyConstraints.FreezeRotationY;
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
            _myTrans.localPosition = playerValue.leaveCarriedScale * getVec;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
            return;
        }
        _myRig.velocity = _myRig.velocity + getVec;
    }

    /// <summary>
    /// jump�Œ��͗������n�߂�������(Falling��)
    /// Falling���͉���ray���΂�State�̐ݒ�ցB
    /// </summary>
    private void CheckIsPlayerGrouded() //�����̏�Ԃ������闷��(Find�ȊO�̊֐������v�����Ȃ�)
    {
        if (playerState == PlayerState.Falling || playerState == PlayerState.BeingThrown) //�������n�߂Ă���̂Ȃ�ray�J�n
        {
            if (Physics.Raycast(_myTrans.position, Vector3.down, 0.1f + _myTrans.lossyScale.y / 2, ~0, QueryTriggerInteraction.Ignore))
                ChangeState(PlayerState.Grounded);

            return;
        }
        else if (playerState == PlayerState.Jumpping && _myRig.velocity.y < 0.1)
            ChangeState(PlayerState.Falling);
    }

    #region move,jump
    // ���[�u
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();
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
            _myTrans.localPosition = Vector3.up;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
        }
    }

    /// <summary>
    /// �v���C���[�ύX���ɕK�v�Ȏ�������Ă����֐��B
    /// </summary>
    public void SetPlayerSelectionStatus(bool setBool)
    {
        _isSelect = setBool;
        _thisCamera.gameObject.SetActive(setBool);
    }
    #endregion

    public void ChangeState(PlayerState getstate)
    {
        playerState = getstate;
    }
}