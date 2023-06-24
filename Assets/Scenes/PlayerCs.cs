using UnityEngine.InputSystem;
using UnityEngine;
using System;
using UnityEngine.XR;
using Unity.VisualScripting;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerCs : MonoBehaviour
{
    #region 変数たち
    [Header("今選択されているかどうかわかる為のobj"), SerializeField]
    private Transform selector;
    private bool isSelect = false;
    
    [SerializeField] private PlayerState playerState = PlayerState.Grounded;

    [Header("プレイヤーの数値 : PlayersValue")]
    public MyPlayersValue playerValue;

    [Header("プレイヤーのInputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _move, _Jump;

    [Header("子オブジェクトのスクリプト格納"), SerializeField]
    private CatchPut_Items catchPutItemsCS;
    [NonSerialized]
    public CatchPut_Items catchPutItemsCSOfParent; //キャッチされている時にこれに代入

    //以下、private変数
    private Transform _myTransform;               // 自分自身の Transform
    private Renderer _renderer;                   // レンダラー
    private Rigidbody myRig;                      // リジッドボディー

    private float _turnVelocity;                  // 回転の速度
    private Vector2 _inputMove;　                 // 動く数値
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

        // 操作入力と、フレームの速度から、現在速度を計算
        var moveDelta = new Vector3
            (
               _inputMove.x * playerValue._speed,
               0,
               _inputMove.y * playerValue._speed
            )
            * Time.deltaTime;

        // 移動入力がある場合は、振り向き動作も行う
        if (_inputMove != Vector2.zero)
        {
            //まず動かす。
            PlayerMove(moveDelta);

            // 操作入力からy軸周りの目標角度[deg]を計算
            var targetAngleY = -Mathf.Atan2(_inputMove.y, _inputMove.x) * Mathf.Rad2Deg + 90;

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
    /// jump最中は落下し始めたか判定(Fallingへ)
    /// Falling中は下にrayを飛ばしStateの設定へ。
    /// </summary>
    private void CheckIsPlayerGrouded() //自分の状態を見つける旅へ(Find以外の関数名が思いつかない)
    {
        if (playerState == PlayerState.Grounded)
            return;

        if (playerState == PlayerState.Falling) //落下し始めているのならray開始
        {
            if (Physics.Raycast(_myTransform.position, Vector3.down, 0.01f + _myTransform.lossyScale.y / 2, 1 << 0))
                ChangeState(PlayerState.Grounded);
            
            return;
        }
        else if(playerState == PlayerState.Jumpping && myRig.velocity.y < 0)
                ChangeState(PlayerState.Falling);
    }

    #region move,jump,change
    // ムーブ
    private void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>().normalized;
    }

    // ジャンプ
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
    /// プレイヤー変更時に必要な事をやっていく関数。
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
            _inputMove = Vector2.zero; //選択されないので(0,0)
            
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