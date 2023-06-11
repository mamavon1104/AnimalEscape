using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CharacterController))]
public class PlayerCs : MonoBehaviour
{
    #region �ϐ�����
    [Header("���I������Ă��邩�ǂ����킩��ׂ�obj"), SerializeField]
    private Transform selector;
    private bool isPlayerSelected = false;

    [Header("�v���C���[�̐��l : PlayersValue")]
    public MyPlayersValue playerValue;

    [Header("�v���C���[��InputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _move;
    private InputAction _Jump;
    [Header("�q�I�u�W�F�N�g�̃X�N���v�g�i�["), SerializeField]
    private PlayerChild playerChild;

    //�ȉ��Aprivate�ϐ�
    private Transform _myTransform;               // �������g�� Transform
    private Renderer _renderer;                   // �����_���[
    [NonSerialized] 
    public CharacterController _charaCtrl;        // �L�����N�^�[�R���g���[���[

    private bool _isGroundedPrev;                 // ���O�̐ڒn���
    private float _turnVelocity;                  // ��]�̑��x
    private float _verticalVelocity;              // �W�����v�A�����ɕω����鑬�x�B
    private Vector2 _inputMove;�@                 //�������l
    #endregion

    private void Awake()
    {
        _myTransform = transform;
        _charaCtrl = _myTransform.GetComponent<CharacterController>();
        _renderer = _myTransform.GetComponent<Renderer>();

        Debug.Log(action);
        Debug.Log(action.currentActionMap);
        Debug.Log(action.currentActionMap["Move"]);
        _move = action.currentActionMap["Move"];
        _Jump = action.currentActionMap["Jump"];

        SetPlayerSelectionStatus(isPlayerSelected);
    }

    private void Update()
    {
        CheckFalling();

        if ((_verticalVelocity == playerValue._jumpSpeed) && !isPlayerSelected)
            return;

        // ������͂Ɖ����������x����A���ݑ��x���v�Z
        Vector3 moveVelocity = new Vector3
            (
               _inputMove.x * playerValue._speed,
               _verticalVelocity,
               _inputMove.y * playerValue._speed
            );

        // ���݃t���[���̈ړ��ʂ��ړ����x����v�Z
        var moveDelta = moveVelocity * Time.deltaTime;

        // PlayerMove()�Ɉړ��ʂ��w�肵�A�I�u�W�F�N�g�𓮂���
        PlayerMove(moveDelta);
        // �ړ����͂�����ꍇ�́A�U�����������s�� isSelected��false�Ȃ�0,0�������Ă���B
        if (_inputMove != Vector2.zero)
        {
            // ������͂���y������̖ڕW�p�x[deg]���v�Z
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
                * Mathf.Rad2Deg + 90;

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

    /// <summary> 
    /// �������Ă��邩�H(bool)�������Ă����瑬�x�������邽�߂̊֐�;
    /// </summary>
    void CheckFalling()
    {
        var isGrounded = _charaCtrl.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            // ���n����u�Ԃɗ����̏������w�肵�Ă���
            _verticalVelocity = -playerValue._initFallSpeed;
        }
        else if (!isGrounded)
        {
            // �󒆂ɂ���Ƃ��́A�������ɏd�͉����x��^���ė���������
            _verticalVelocity -= playerValue._gravity * Time.deltaTime;

            // �������鑬���ȏ�ɂȂ�Ȃ��悤�ɕ␳
            if (_verticalVelocity < -playerValue._fallSpeed)
                _verticalVelocity = -playerValue._fallSpeed;
        }
        _isGroundedPrev = isGrounded;
    }

    /// <summary> 
    /// get����vec�����������A������̂ɂ��g������
    /// </summary>
    public void PlayerMove(Vector3 getVec)
    {
        _charaCtrl.Move(getVec);
        playerChild.MoveOtherPlayer(getVec);
    }

    #region move,jump�B
    // ���[�u
    private void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��ێ����Ă���
        _inputMove = context.ReadValue<Vector2>();
    }
    // �W�����v
    private void OnJump(InputAction.CallbackContext context)
    {
        // �n�ʂɂ��ĂȂ��A�I�΂�ĂȂ��@�ꍇ�͂��Ⴀ��
        if ((!_charaCtrl.isGrounded) || (!isPlayerSelected))
            return;

        // ����������ɑ��x��^����
        _verticalVelocity = playerValue._jumpSpeed;
    }
    // �L�����N�^�[�̕ύX(PlayerInput������Ă΂��)
    public void SetPlayerSelectionStatus(bool getBool)
    {
        isPlayerSelected = getBool;
        selector.gameObject.SetActive(getBool);
        if (getBool)
        {
            _move.canceled  += OnMove;
            _move.performed += OnMove;
            _Jump.performed += OnJump;
            ChangeColor(Color.blue);
        }
        else
        {
            _move.canceled  -= OnMove;
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
}