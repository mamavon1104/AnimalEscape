using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CharacterController))]
public class PlayerCs : MonoBehaviour
{
    #region 変数たち
    [Header("今選択されているかどうかわかる為のobj"), SerializeField]
    private Transform selector;
    private bool isPlayerSelected = false;

    [Header("プレイヤーの数値 : PlayersValue")]
    public MyPlayersValue playerValue;

    [Header("プレイヤーのInputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _move;
    private InputAction _Jump;
    [Header("子オブジェクトのスクリプト格納"), SerializeField]
    private PlayerChild playerChild;

    //以下、private変数
    private Transform _myTransform;               // 自分自身の Transform
    private Renderer _renderer;                   // レンダラー
    [NonSerialized] 
    public CharacterController _charaCtrl;        // キャラクターコントローラー

    private bool _isGroundedPrev;                 // 直前の接地状態
    private float _turnVelocity;                  // 回転の速度
    private float _verticalVelocity;              // ジャンプ、落下に変化する速度。
    private Vector2 _inputMove;　                 //動く数値
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

        // 操作入力と鉛直方向速度から、現在速度を計算
        Vector3 moveVelocity = new Vector3
            (
               _inputMove.x * playerValue._speed,
               _verticalVelocity,
               _inputMove.y * playerValue._speed
            );

        // 現在フレームの移動量を移動速度から計算
        var moveDelta = moveVelocity * Time.deltaTime;

        // PlayerMove()に移動量を指定し、オブジェクトを動かす
        PlayerMove(moveDelta);
        // 移動入力がある場合は、振り向き動作も行う isSelectedがfalseなら0,0が入っている。
        if (_inputMove != Vector2.zero)
        {
            // 操作入力からy軸周りの目標角度[deg]を計算
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x)
                * Mathf.Rad2Deg + 90;

            // イージングしながら次の回転角度[deg]を計算
            var angleY = Mathf.SmoothDampAngle(
                _myTransform.eulerAngles.y,
                targetAngleY,
                ref _turnVelocity,
                0.1f
            );
            // オブジェクトの回転を更新
            _myTransform.rotation = Quaternion.Euler(0, angleY, 0);
        }
    }

    /// <summary> 
    /// 落下しているか？(bool)落下していたら速度を加えるための関数;
    /// </summary>
    void CheckFalling()
    {
        var isGrounded = _charaCtrl.isGrounded;

        if (isGrounded && !_isGroundedPrev)
        {
            // 着地する瞬間に落下の初速を指定しておく
            _verticalVelocity = -playerValue._initFallSpeed;
        }
        else if (!isGrounded)
        {
            // 空中にいるときは、下向きに重力加速度を与えて落下させる
            _verticalVelocity -= playerValue._gravity * Time.deltaTime;

            // 落下する速さ以上にならないように補正
            if (_verticalVelocity < -playerValue._fallSpeed)
                _verticalVelocity = -playerValue._fallSpeed;
        }
        _isGroundedPrev = isGrounded;
    }

    /// <summary> 
    /// getしたvecだけ動かす、投げるのにも使いたい
    /// </summary>
    public void PlayerMove(Vector3 getVec)
    {
        _charaCtrl.Move(getVec);
        playerChild.MoveOtherPlayer(getVec);
    }

    #region move,jump。
    // ムーブ
    private void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
    }
    // ジャンプ
    private void OnJump(InputAction.CallbackContext context)
    {
        // 地面についてない、選ばれてない　場合はじゃあね
        if ((!_charaCtrl.isGrounded) || (!isPlayerSelected))
            return;

        // 鉛直上向きに速度を与える
        _verticalVelocity = playerValue._jumpSpeed;
    }
    // キャラクターの変更(PlayerInput側から呼ばれる)
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
            _inputMove = Vector2.zero; //選択されないので(0,0)
            ChangeColor(Color.red);
        }
    }
    #endregion
    private void ChangeColor(Color color)
    {
        _renderer.material.color = color;
    }
}