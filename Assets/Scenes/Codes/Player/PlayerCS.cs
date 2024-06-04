using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(PlayerInputScript))]
public class PlayerCS : MonoBehaviour
{
    #region 変数たち
    [Header("カメラ"), SerializeField] private Transform _thisCamera;
    [Header("子オブジェクトのThrowToPoint"), SerializeField] private ThrowToPoint _myThrow;
    [Header("プレイヤーの数値 : PlayersValue"), SerializeField] private MyPlayersValue _playerValue;
    [Header("子オブジェクトの選択されているかどうかのobject"), SerializeField] GameObject m_selectObj;
    [Header("現在のプレイヤーの状態"), SerializeField] private PlayerState _playerState = PlayerState.Grounded;

    /// <summary>
    /// キャッチされている時に相手のCatchPutItemsを代入してそれをこっちから呼び起こしてあげる。
    /// </summary>
    [SerializeField]
    private CatchPut_Items _catchPutItemsCSOfParent;
    public CatchPut_Items CatchPutItemsCSOfParent
    {
        set { _catchPutItemsCSOfParent = value; }
    }

    //以下、private変数
    private Rigidbody _myRig;
    private Vector2 _inputMove;　// 動く数値
    private Transform _myTrans;
    private bool _isSelect = false;
    private Quaternion _targetRotation;
    private AudioManager _audioManager;
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
    private void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>();
    }
    private void FixedUpdate()
    {
        if (!_isSelect)
            return;

        //Vector3 cameraForward = Vector3.Scale(thisCamera.forward, new Vector3(1, 0, 1)).normalized;
        var horizontalRotation = Quaternion.AngleAxis(_thisCamera.eulerAngles.y, Vector3.up);
        var velocity = horizontalRotation * new Vector3(_inputMove.x, 0, _inputMove.y) * _playerValue.speed * Time.deltaTime;

        // 移動入力がある場合は、振り向き動作も行う
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
            //記述;
            _myTrans.rotation = Quaternion.RotateTowards(_myTrans.rotation, _targetRotation, _playerValue.playerRotateSpeed * Time.deltaTime);
        }
        else if (!_myRig.constraints.HasFlag(RigidbodyConstraints.FreezeRotationY))
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
        Debug.Log(_myRig);
        _myRig.velocity = getVec + new Vector3(0, _myRig.velocity.y, 0);
    }

    /// <summary>
    /// jump最中は落下し始めたか判定(Fallingへ)
    /// Falling中は下にrayを飛ばしStateの設定へ。
    /// </summary>
    private void CheckIsPlayerGrouded() //自分の状態を見つける旅へ(Find以外の関数名が思いつかない)
    {
        if (_playerState == PlayerState.Falling || _playerState == PlayerState.BeingThrown) //落下し始めているのならray開始
        {
            if (Physics.Raycast(_myTrans.position, Vector3.down, 0.1f + _myTrans.lossyScale.y / 2, ~0, QueryTriggerInteraction.Ignore))
                ChangeState(PlayerState.Grounded);

            return;
        }
        else if (_playerState == PlayerState.Jumpping && _myRig.velocity.y < 0.1)
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
        if (_playerState == PlayerState.Grounded)
        {
            if (Physics.Raycast(_myTrans.position, Vector3.down, out var other, 1 + _myTrans.lossyScale.y / 2, 1 << 11))
            {
                other.transform.GetComponent<JumpThrowPlayer>().ThrowPlayerinJumpAction(_myTrans, this);
                return;
            }
            _playerState = PlayerState.Jumpping;
            PlayerMove(new Vector3(0, _playerValue.jumpSpeed, 0));
            _audioManager.PlayJump(new InputAction.CallbackContext());
        }
        else if (_playerState == PlayerState.BeingCarried)
        {
            _myTrans.localPosition = Vector3.up;
            _catchPutItemsCSOfParent.ResetOtherStateAndReleaseCatch();
        }
    }

    /// <summary>
    /// プレイヤー変更時に必要な事をやっていく関数。
    /// </summary>
    public void SetPlayerSelectionStatus(bool setBool)
    {
        _isSelect = setBool;
        m_selectObj.SetActive(setBool);
        _thisCamera.gameObject.SetActive(setBool);
    }
    #endregion

    public void ChangeState(PlayerState getstate)
    {
        _playerState = getstate;
    }
}