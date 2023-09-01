using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PlayerInputScript))]
public class PlayerCS : MonoBehaviour
{
    #region 変数たち
    [Header("カメラ"), SerializeField]
    private Transform _thisCamera;
    private bool _isSelect = false;

    [Header("現在のプレイヤーの状態"), SerializeField]
    private PlayerState playerState = PlayerState.Grounded;

    [Header("プレイヤーの数値 : PlayersValue"), SerializeField]
    public MyPlayersValue playerValue;

    [Header("子オブジェクトのスクリプト格納"), SerializeField]
    private CatchPut_Items catchPutItemsCS;

    [Header("子オブジェクトのThrowToPoint"), SerializeField]
    private ThrowToPoint myThrow;

    /// <summary>
    /// キャッチされている時に相手のCatchPutItemsを代入してそれをこっちから呼び起こしてあげる。
    /// </summary>
    [NonSerialized] 
    public CatchPut_Items catchPutItemsCSOfParent; 

    //以下、private変数
    private Rigidbody _myRig;
    private Vector2 _inputMove;　// 動く数値
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
        
        // 移動入力がある場合は、振り向き動作も行う
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
                //記述;
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
    /// jump最中は落下し始めたか判定(Fallingへ)
    /// Falling中は下にrayを飛ばしStateの設定へ。
    /// </summary>
    private void CheckIsPlayerGrouded() //自分の状態を見つける旅へ(Find以外の関数名が思いつかない)
    {
        if (playerState == PlayerState.Falling || playerState == PlayerState.BeingThrown) //落下し始めているのならray開始
        {
            if (Physics.Raycast(_myTrans.position, Vector3.down, 0.1f + _myTrans.lossyScale.y / 2, ~0, QueryTriggerInteraction.Ignore))
                ChangeState(PlayerState.Grounded);

            return;
        }
        else if (playerState == PlayerState.Jumpping && _myRig.velocity.y < 0.1)
            ChangeState(PlayerState.Falling);
    }

    #region move,jump
    // ムーブ
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
    }

    // ジャンプ
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
    /// プレイヤー変更時に必要な事をやっていく関数。
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