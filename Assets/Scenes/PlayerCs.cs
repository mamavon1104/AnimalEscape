using UnityEngine.InputSystem;
using UnityEngine;
using System;
using UnityEngine.XR;
using Unity.VisualScripting;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCs : MonoBehaviour
{
    #region �ϐ�����
    [Header("���I������Ă��邩�ǂ����킩��ׂ�obj"), SerializeField]
    private Transform selector;
    private bool isSelect = false;
    
    [SerializeField] private PlayerState playerState = PlayerState.Grounded;

    [Header("�v���C���[�̐��l : PlayersValue")]
    public MyPlayersValue playerValue;

    [Header("�v���C���[��InputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _move, _Jump;

    [Header("�q�I�u�W�F�N�g�̃X�N���v�g�i�["), SerializeField]
    private CatchPut_Items catchPutItemsCS;
    [NonSerialized]
    public CatchPut_Items catchPutItemsCSOfParent; //�L���b�`����Ă��鎞�ɂ���ɑ��

    //�ȉ��Aprivate�ϐ�
    private Transform _myTransform;               // �������g�� Transform
    private Renderer _renderer;                   // �����_���[
    private Rigidbody myRig;                      // ���W�b�h�{�f�B�[

    private float _turnVelocity;                  // ��]�̑��x
    private Vector2 _inputMove;�@                 // �������l
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        myRig = _myTransform.GetComponent<Rigidbody>();
        _renderer = _myTransform.GetComponent<Renderer>();

        _move = action.currentActionMap["Move"];
        _Jump = action.currentActionMap["Jump"];
    }

    private void Update()
    {
        if (!isSelect)
            return;

        // ������͂ƁA�t���[���̑��x����A���ݑ��x���v�Z
        var moveDelta = new Vector3
            (
               _inputMove.x * playerValue._speed,
               0,
               _inputMove.y * playerValue._speed
            )
            * Time.deltaTime;

        // �ړ����͂�����ꍇ�́A�U�����������s��
        if (_inputMove != Vector2.zero)
        {
            //�܂��������B
            PlayerMove(moveDelta);

            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x) * Mathf.Rad2Deg + 90;

            // �C�[�W���O���Ȃ��玟�̉�]�p�x[deg]���v�Z
            var angleY = Mathf.SmoothDampAngle(
                _myTransform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );
            // �I�u�W�F�N�g�̉�]���X�V
            _myTransform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }
    private void LateUpdate()
    {
        if (playerState == PlayerState.Grounded)
            return;

        if(playerState == PlayerState.Jumpping || playerState == PlayerState.Falling || myRig.velocity.y < -0.01)
        {
            CheckIsPlayerGrouded();
        }
    }
    private void PlayerMove(Vector3 getVec)
    {
        if (playerState == PlayerState.BeingCarried)
        {
            _myTransform.localPosition = new Vector3(_inputMove.x,0, _inputMove.y);
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
            return;
        }
        myRig.AddForce(getVec, ForceMode.Impulse);
    }

    /// <summary>
    /// jump�Œ��͗������n�߂�������(Falling��)
    /// Falling���͉���ray���΂�State�̐ݒ�ցB
    /// </summary>
    private void CheckIsPlayerGrouded() //�����̏�Ԃ������闷��(Find�ȊO�̊֐������v�����Ȃ�)
    {
        if (playerState == PlayerState.Grounded)
            return;

        if (playerState == PlayerState.Falling) //�������n�߂Ă���̂Ȃ�ray�J�n
        {
            if (Physics.Raycast(_myTransform.position, Vector3.down, 0.01f + _myTransform.lossyScale.y / 2, 1 << 0))
                ChangeState(PlayerState.Grounded);
            
            return;
        }
        else if(playerState == PlayerState.Jumpping && myRig.velocity.y < 0)
                ChangeState(PlayerState.Falling);
    }

    #region move,jump,change
    // ���[�u
    private void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>().normalized;
    }

    // �W�����v
    private void OnJump(InputAction.CallbackContext context)
    {
        if (playerState == PlayerState.Grounded)
        {
            playerState = PlayerState.Jumpping;
            PlayerMove(new Vector3(0, playerValue._jumpSpeed, 0));
        }
        else if (playerState == PlayerState.BeingCarried)
        {
            _myTransform.localPosition = Vector3.up;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
        }
    }

    /// <summary>
    /// �v���C���[�ύX���ɕK�v�Ȏ�������Ă����֐��B
    /// </summary>
    public void SetPlayerSelectionStatus(bool getBool)
    {
        selector.gameObject.SetActive(getBool);
        catchPutItemsCS.SelectionStatus(getBool);
        isSelect = getBool;
        if (getBool)
        {
            _move.canceled += OnMove;
            _move.performed += OnMove;
            _Jump.performed += OnJump;
            ChangeColor(Color.blue);
        }
        else
        {
            _move.canceled -= OnMove;
            _move.performed -= OnMove;
            _Jump.performed -= OnJump;
            _inputMove = Vector2.zero; //�I������Ȃ��̂�(0,0)
            
            ChangeColor(Color.red);
        }
    }
    #endregion
    private void ChangeColor(Color color)
    {
        _renderer.material.color = color;
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