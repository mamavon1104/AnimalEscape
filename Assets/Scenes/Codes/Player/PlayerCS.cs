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
    private PlayerState _playerState = PlayerState.Grounded;

    [Header("�v���C���[�̐��l : PlayersValue"), SerializeField]
    private MyPlayersValue _playerValue;

    [Header("�q�I�u�W�F�N�g�̃X�N���v�g�i�["), SerializeField]
    private CatchPut_Items _catchPutItemsCS;

    [Header("�q�I�u�W�F�N�g��ThrowToPoint"), SerializeField]
    private ThrowToPoint _myThrow;

    /// <summary>
    /// �L���b�`����Ă��鎞�ɑ����CatchPutItems�������Ă��������������ĂыN�����Ă�����B
    /// </summary>
    [NonSerialized] 
    private CatchPut_Items _catchPutItemsCSOfParent; 
    public CatchPut_Items CatchPutItemsCSOfParent
    {
        set { _catchPutItemsCS = value; }
    }

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
        var velocity = horizontalRotation * new Vector3(_inputMove.x, 0, _inputMove.y) * _playerValue.speed * Time.deltaTime;
        
        // �ړ����͂�����ꍇ�́A�U�����������s��
        if (velocity.sqrMagnitude > 0f)
        {
            PlayerMove(velocity);
            _targetRotation = Quaternion.LookRotation(velocity);
        }

        if (_myThrow.SelectThrow == false)
        {
            if (_myRig.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY))
            {
                _myRig.constraints = _myRig.constraints ^ RigidbodyConstraints.FreezeRotationY;
            }
                //�L�q;
            _myTrans.rotation = Quaternion.RotateTowards(_myTrans.rotation, _targetRotation, _playerValue.playerRotateSpeed * Time.deltaTime);           
        }
        else if(!_myRig.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY))
            _myRig.constraints = _myRig.constraints | RigidbodyConstraints.FreezeRotationY;
    }
    private void LateUpdate()
    {
        if (_playerState == PlayerState.Grounded || _playerState == PlayerState.BeingCarried)
            return;

        CheckIsPlayerGrouded();
    }
    private void PlayerMove(Vector3 getVec)
    {
        if (_playerState == PlayerState.BeingThrown)
            return;

        if (_playerState == PlayerState.BeingCarried)
        {
            _myTrans.localPosition = _playerValue.leaveCarriedScale * getVec;
            _catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
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
        if (_playerState == PlayerState.Falling || _playerState == PlayerState.BeingThrown) //�������n�߂Ă���̂Ȃ�ray�J�n
        {
            if (Physics.Raycast(_myTrans.position, Vector3.down, 0.1f + _myTrans.lossyScale.y / 2, ~0, QueryTriggerInteraction.Ignore))
                ChangeState(PlayerState.Grounded);

            return;
        }
        else if (_playerState == PlayerState.Jumpping && _myRig.velocity.y < 0.1)
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
        if (_playerState == PlayerState.Grounded)
        {
            _playerState = PlayerState.Jumpping;
            PlayerMove(new Vector3(0, _playerValue.jumpSpeed, 0));
        }
        else if (_playerState == PlayerState.BeingCarried)
        {
            _myTrans.localPosition = Vector3.up;
            _catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
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
        _playerState = getstate;
    }
}