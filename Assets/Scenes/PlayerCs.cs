using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Unity.Mathematics;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PlayerInputScript))]
public class PlayerCS : MonoBehaviour
{
    #region 変数たち
    [Header("カメラ"), SerializeField]
    private Transform thisCamera;
    private bool isSelect = false;

    [Header("現在のプレイヤーの状態"), SerializeField]
    private PlayerState playerState = PlayerState.Grounded;

    [Header("プレイヤーの数値 : PlayersValue")]
    public MyPlayersValue playerValue;

    [Header("子オブジェクトのスクリプト格納"), SerializeField]
    private CatchPut_Items catchPutItemsCS;

    /// <summary>
    /// キャッチされている時に相手のCatchPut_Itemsを代入してそれをこっちから呼び起こしてあげる。
    /// </summary>
    [NonSerialized] 
    public CatchPut_Items catchPutItemsCSOfParent; 

    //以下、private変数
    private Rigidbody _myRig;
    private Renderer _renderer;
    private Transform myTrans;
    private PlayerInputScript inputScript;

    private Vector2 _inputMove;　// 動く数値
    #endregion
    private Quaternion targetRotation;

    private void Awake()
    {
        myTrans = transform;
        targetRotation = myTrans.rotation;

        _myRig = myTrans.GetComponent<Rigidbody>();
        _renderer = myTrans.GetComponent<Renderer>();
        inputScript = myTrans.GetComponent<PlayerInputScript>();
    }
    private void Start()
    {
        PlayerInformationMaster.instance.playerParentsDic.Add(this,myTrans.parent);
    }

    private void FixedUpdate()
    {
        if (!isSelect)
            return;

        var horizontalRotation = Quaternion.AngleAxis(thisCamera.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(_inputMove.x, 0, _inputMove.y) * playerValue._speed * Time.deltaTime;
        
        // 移動入力がある場合は、振り向き動作も行う
        if (velocity.sqrMagnitude > 0f)
        {
            PlayerMove(velocity);
            targetRotation = Quaternion.LookRotation(velocity);
        }

        myTrans.rotation = Quaternion.RotateTowards(myTrans.rotation, targetRotation, playerValue.playerRotateSpeed * Time.deltaTime);
        
        #region
        //Vector3 cameraForward = Vector3.Scale(thisCamera.forward, new Vector3(1, 0, 1)).normalized;
        //Vector3 moveForward = (cameraForward * _inputMove.y + thisCamera.right * _inputMove.x).normalized * playerValue._speed * Time.deltaTime;

        //PlayerMove(moveForward);

        //// 移動入力がある場合は、振り向き動作も行う
        //if (moveForward.sqrMagnitude > 0.5f)
        //{
        //    myRotation = Quaternion.LookRotation(moveForward);
        //}
        //_myTransform.rotation = Quaternion.RotateTowards(_myTransform.rotation, myRotation, playerValue.playerRotateSpeed * Time.deltaTime);
        #endregion
    }
    private void LateUpdate()
    {
        if (playerState == PlayerState.Grounded)
            return;

        if (_myRig.velocity.y < -0.01)
        {
            CheckIsPlayerGrouded();
        }
    }
    private void PlayerMove(Vector3 getVec)
    {
        if (playerState == PlayerState.BeingThrown)
            return;

        if (playerState == PlayerState.BeingCarried)
        {
            myTrans.localPosition = Time.deltaTime * getVec;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
            return;
        }

        //_myRig.velocity = _myRig.velocity + getVec;
        _myRig.AddForce(getVec,ForceMode.Impulse);
    }

    /// <summary>
    /// jump最中は落下し始めたか判定(Fallingへ)
    /// Falling中は下にrayを飛ばしStateの設定へ。
    /// </summary>
    private void CheckIsPlayerGrouded() //自分の状態を見つける旅へ(Find以外の関数名が思いつかない)
    {
        if (playerState == PlayerState.Falling || playerState == PlayerState.BeingThrown) //落下し始めているのならray開始
        {
            if (Physics.Raycast(myTrans.position, Vector3.down, 0.1f + myTrans.lossyScale.y / 2, ~0, QueryTriggerInteraction.Ignore))
                ChangeState(PlayerState.Grounded);

            return;
        }
        else if (playerState == PlayerState.Jumpping && _myRig.velocity.y < 0)
            ChangeState(PlayerState.Falling);
    }

    #region move,jump,change
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
            PlayerMove(new Vector3(0, playerValue._jumpSpeed, 0));
        }
        else if (playerState == PlayerState.BeingCarried)
        {
            myTrans.localPosition = Vector3.up;
            catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
        }
    }

    /// <summary>
    /// プレイヤー変更時に必要な事をやっていく関数。
    /// </summary>
    public void SetPlayerSelectionStatus(bool setBool)
    {
        isSelect = setBool;
        thisCamera.gameObject.SetActive(setBool);
        inputScript.Setting(setBool);

        if (setBool)
            ChangeColor(Color.blue);
        else
            ChangeColor(Color.red);
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