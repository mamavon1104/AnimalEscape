using UnityEngine;
using UnityEngine.InputSystem;

public class CatchPut_Items : MonoBehaviour
{
    [Header("自分の真上のポジション。"), SerializeField]
    private Transform myUpTrans;

    private Transform myT; //私のTrans、キャッシュ用。
    private Transform triggerObject = null; //トリガーに当たっているなら取得、それでなければNull;
    private Transform catchObject = null; //管するobj、現在持っているもの;]
    private ThrowToPoint throwToPoint;
    public Transform TriggerObject
    {
        private get { return triggerObject; }
        set
        {
            if (value == null)
            {
                triggerObject = value;
                return; 
            }
            if (value.tag != "Untagged")
                triggerObject = value;
        }
    }
    public Transform CatchObject
    {
        get { return catchObject; }
        private set { catchObject = value; }
    }
    
    private void Start()
    {
        myT = transform;
        throwToPoint = myT.GetComponent<ThrowToPoint>();
    }
    /// <param name="context"></param>
    public void CatchAndPut(InputAction.CallbackContext context)
    {
        if (TriggerObject == null && CatchObject == null)
            return;

        if (CatchObject == null)
            SetCatchObject();   
        else 
            ResetOtherObjPos();
    }
    private void OnTriggerEnter(Collider other)
    {
        TriggerObject = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == TriggerObject) //もし抜けたものがotherだった場合
            TriggerObject = null; //追い出す。
    }

    /// <summary>
    /// 物を拾う時の設定、CatchObjectsから呼び起こされる。
    /// </summary>
    public void SetCatchObject()
    {
        CatchObject = TriggerObject;                             //取得したObjに
        
        if (CatchObject.CompareTag("Player"))
        {
            var otherPlayerCS = CatchObject.GetComponent<PlayerCS>();
            otherPlayerCS.ChangeState(PlayerCS.PlayerState.BeingCarried);
            otherPlayerCS.CatchPutItemsCSOfParent = this;
        }

        CatchObject.parent = myUpTrans;                //親をupにします。
        CatchObject.position = myUpTrans.position;    //位置を真上から離さん
        CatchObject.rotation = myUpTrans.rotation;    //回転も同じにしてやるからな。
        CatchObject.GetComponent<Rigidbody>().isKinematic = true;
    }
        
    /// <summary>
    /// これが呼び出されている場合:
    /// オブジェクトの位置を直しつつ状態リセットをする。
    /// </summary>
    public void ResetOtherObjPos()
    {
        CatchObject.position = myT.position;        //位置を直すし
        CatchObject.rotation = myT.rotation;        //回転も戻す
        ResetOtherStateAndReleaseCatch();
    }

    /// <summary>
    /// ResetOtherObjPosじゃなく、これが単体で呼ばれている場合 :
    /// 位置は相手のコードで直し、キャッチしている判定を消す
    /// </summary>
    public void ResetOtherStateAndReleaseCatch()
    {
        CatchObject.GetComponent<Rigidbody>().isKinematic = false;
        if (CatchObject.CompareTag("Player"))
        {
            var otherPlayerCS = CatchObject.GetComponent<PlayerCS>();
            otherPlayerCS.CatchPutItemsCSOfParent = null;
            otherPlayerCS.GetComponent<PlayerCS>().ChangeState(PlayerCS.PlayerState.Falling);
            CatchObject.parent = PlayerInformationManager.Instance.playerParentsDic[otherPlayerCS]; //親を戻します。
        }
        CatchObject = null; 
        TriggerObject = null;
        throwToPoint.FinishResetVariable(this);
    }
}
