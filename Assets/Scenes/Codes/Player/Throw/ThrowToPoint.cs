using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


public class ThrowToPoint : MonoBehaviour
{
    [Header("投げるポジション"), SerializeField]
    private Transform throwPointNow;
    private Vector3 startPos; // 移動の開始位置

    private CatchPut_Items catchPutItemsCS;
    /// <summary>
    /// catchPutItemsCS.CatchObject;
    /// </summary>
    private Transform objTrans;
    private Transform myT;
    private bool selectThrow;
    public bool SelectThrow
    {
        get { return selectThrow; }
        private set {selectThrow = value;}
    }
    static GameObject FinishTriggerParent;
    static List<GameObject> FinishTriggerList = new List<GameObject>();

    [Header("投げるときに変更させるCamera"), SerializeField]
    GameObject thisCamera;
    
    [Header("スフィアコライダー、作る為"),SerializeField]
    GameObject sphereColliderObj;
   
    [Header("Playerの値(PlayerValue)"), SerializeField]
    private MyPlayersValue playerValue;

    void Start()
    {
        myT = transform.parent;
        catchPutItemsCS = transform.GetComponent<CatchPut_Items>();
        FinishTriggerParent = GameObject.FindGameObjectWithTag("FinishTriggerParent"); //findは使わない。
    }
    private void Update()
    {
         if (!SelectThrow)
            return;
    }
    public void Select_OR_Throw(InputAction.CallbackContext _)
    {
        objTrans = catchPutItemsCS.CatchObject;
        
        if (objTrans == null)
            return;

        if (!SelectThrow) //選ばれてない場合は選択
        {
            SelectThrow = true;
            thisCamera.gameObject.SetActive(true);
            throwPointNow.gameObject.SetActive(true);
            PlayerInformationManager.Instance.inputScriptDic[myT].ChangeCameraMove(false);
        }
        else//選ばれた後は投げる
        {
            startPos = objTrans.position;

            //投げる為にrigidbody,持ってる判定を削除、等
            catchPutItemsCS.ResetOtherStateAndReleaseCatch();

            // 射出速度を算出
            Vector3 velocity = CalculateVelocity(startPos, throwPointNow.position, playerValue.throwAngle);
            //飛ばす
            objTrans.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            
            //到着したかどうか判定するためのobj作成
            GameObject sphereObjs = GetObjectFromPool();
            sphereObjs.transform.position = throwPointNow.position;
            
            FinishResetVariable(this);
        }
    }

    private GameObject GetObjectFromPool()
    {
        // 非アクティブなオブジェクトを検索して返す
        foreach (GameObject obj in FinishTriggerList)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // プール内のすべてのオブジェクトが使用中の場合は新たにオブジェクトを生成して返す
        var newObj = Instantiate(sphereColliderObj);
        newObj.transform.parent = FinishTriggerParent.transform;
        FinishTriggerList.Add(newObj);
        return newObj;
    }

    /// <param name="orderClass">
    /// 呼び出し元のクラスをジェネリックメソッドとcaseで判別
    /// </param>
    public void FinishResetVariable<T>(T orderClass, GameObject sphere = null) where T : class
    {
        SelectThrow = false;

        if(sphere != null) 
           sphere.SetActive(false);

        if (objTrans == null)
            return;

        thisCamera.gameObject.SetActive(false);
        throwPointNow.gameObject.SetActive(false);
        PlayerInformationManager.Instance.inputScriptDic[myT].ChangeCameraMove(true);

        Debug.Log($"<color=red>{ orderClass }</color>");
        switch (orderClass)
        {
            case CatchPut_Items:
            case CatchObjects:
                if (objTrans.CompareTag("Player"))
                    objTrans.GetComponent<PlayerCS>().ChangeState(PlayerCS.PlayerState.Falling);
                break;
            case ThrowToPoint:
                if (objTrans.CompareTag("Player"))
                    objTrans.GetComponent<PlayerCS>().ChangeState(PlayerCS.PlayerState.BeingThrown);
                objTrans = null;
                break;
            default:
                Debug.LogError("想定外のクラスから呼び出しが行われています");
                break;
        }
    }
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        float rad = angle * Mathf.PI / 180;
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));
        float y = pointA.y - pointB.y;
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            return Vector3.zero;
        }
        else
        {
            // 到達地点までの速度ベクトル
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}