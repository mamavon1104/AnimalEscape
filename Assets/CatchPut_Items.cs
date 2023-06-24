using UnityEngine;
using UnityEngine.InputSystem;

public class CatchPut_Items : MonoBehaviour
{
    [Header("PlayerInput"), SerializeField]
    private PlayerInput action;
    private InputAction catchAndPutAction,throwAction;

    [Header("自分の真上のポジション。"), SerializeField]
    private Transform myUpTrans;
    [Header("PlayerParent"), SerializeField]
    private Transform playerParent;

    public  Transform triggerObject = null; //トリガーに当たっているなら取得、それでなければNull;
    private Transform catchObject = null; //持つボタンを押したときに保管するobj;
    private bool isCatchingNow = false; //持っているか？
    
    private Transform myT; //私のTrans、キャッシュ用。

    private void Awake()
    {
        myT = transform;
        throwAction = action.currentActionMap["ThrowObject"];
        catchAndPutAction = action.currentActionMap["CatchAndPut"];
    }
    private void CatchAndPut(InputAction.CallbackContext context)
    {
        if (triggerObject == null && catchObject == null)
            return;

        if (!isCatchingNow)
            SetCatchObject();   
        else 
            ResetOtherObjPos();
    }
    private void OnTriggerEnter(Collider other)
    {
        var otherTrans = other.transform;
        if (otherTrans.tag != "Untagged")
            triggerObject = otherTrans;
    }
    private void OnTriggerExit(Collider other)
    {
        var otherTrans = other.transform;
        if (otherTrans == triggerObject) //もし抜けたものがotherだった場合
            triggerObject = null; //追い出す。
    }

    public void SetCatchObject()
    {
        catchObject = triggerObject;                             //取得したObjに
        catchObject.transform.parent = myUpTrans;                //親をupにします。
        catchObject.transform.position = myUpTrans.position;   //位置を真上から離さん
        catchObject.transform.rotation = myUpTrans.rotation;   //回転も同じにしてやるからな。
        catchObject.GetComponent<Rigidbody>().isKinematic = true;

        if (catchObject.tag == "Player")
        {
            var otherPlayerCS = catchObject.GetComponent<PlayerCs>();
            otherPlayerCS.ChangeState(PlayerCs.PlayerState.BeingCarried);
            otherPlayerCS.catchPutItemsCSOfParent = this;
        }
        isCatchingNow = true; //色違いキャッチ　簡単で　す！
    }
        
    /// <summary>
    /// これが呼び出されている時はオブジェクトの位置を直しつつ状態リセットをする。
    /// </summary>
    public void ResetOtherObjPos()
    {
        catchObject.transform.position = myT.position;        //位置を直すし
        catchObject.transform.rotation = myT.rotation;        //回転も戻す
        ResetOtherStateAndReleaseCatch();
    }

    /// <summary>
    /// SetOtherOBjPosじゃなく、これが単体で呼ばれている場合、位置は相手のコードで直し、キャッチしている判定もろもろを消す
    /// </summary>
    public void ResetOtherStateAndReleaseCatch()
    {
        catchObject.parent = null;                    //親を戻します。
        catchObject.GetComponent<Rigidbody>().isKinematic = false;
        if (catchObject.tag == "Player")
        {
            catchObject.parent = playerParent;
            var otherPlayerCS = catchObject.GetComponent<PlayerCs>();
            otherPlayerCS.GetComponent<PlayerCs>().ChangeState(PlayerCs.PlayerState.Falling);
            otherPlayerCS.catchPutItemsCSOfParent = null;
        }
        catchObject = null; 
        isCatchingNow = false;
    }
    public void SelectionStatus(bool getBool)
    {
        if (getBool)
        {
            catchAndPutAction.performed += CatchAndPut;
        }
        else
        {
            catchAndPutAction.performed -= CatchAndPut;
        }
    }
}
