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
    private Transform thisCamera;
    private bool isSelect = false;

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
    private Rigidbody myRig;
    private Renderer renderer;
    private Vector2 inputMove;　// 動く数値
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
        
        // 移動入力がある場合は、振り向き動作も行う
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
                //記述;
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
        else if (playerState == PlayerState.Jumpping && myRig.velocity.y < 0)
            ChangeState(PlayerState.Falling);
    }

    #region move,jump,change
    // ムーブ
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        inputMove = context.ReadValue<Vector2>();
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