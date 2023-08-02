using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
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
    private bool selectThrow;

    [Header("投げるときに変更させるCamera"), SerializeField]
    GameObject thisCamera;
    
    [Header("スフィアコライダー、作る為"),SerializeField]
    GameObject sphereColliderObj;
    private static GameObject FinishTriggerParent;
    private static List<GameObject> FinishTriggerList = new List<GameObject>();
   
    [Header("Playerの値(PlayerValue)"), SerializeField]
    private MyPlayersValue playerValue;
    
    void Start()
    {
        catchPutItemsCS = transform.GetComponent<CatchPut_Items>();
        FinishTriggerParent = GameObject.FindGameObjectWithTag("FinishTriggerParent");
    }
    private void Update()
    {
         if (!selectThrow)
            return;
    }
    public void Select_OR_Throw(InputAction.CallbackContext _)
    {
        objTrans = catchPutItemsCS.CatchObject;
        
        if (objTrans == null)
            return;

        if (!selectThrow) //選ばれてない場合は選択
        {
            selectThrow = true;
            thisCamera.gameObject.SetActive(true);
        }
        else//選ばれた後は投げる
        {
            startPos = objTrans.position;
            
            //投げる為にrigidbody,持ってる判定を削除、等
            catchPutItemsCS.ResetOtherStateAndReleaseCatch();

            // 射出速度を算出
            Vector3 velocity = CalculateVelocity(startPos, throwPointNow.position, playerValue._throwAngle);
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
        selectThrow = false;

        if(sphere != null) 
           sphere.SetActive(false);

        if (objTrans == null)
            return;

        thisCamera.gameObject.SetActive(false);

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
    /*
    private bool CheckWallCollision()
    {
        RaycastHit hit;

        if (objTrans == null)
            return false;

        Debug.DrawRay(objTrans.position, objTrans.forward * 2, Color.red, 5.0f);
        if (Physics.Raycast(objTrans.position, objTrans.forward * 2, out hit, 1f, 1 << 0, QueryTriggerInteraction.Ignore)) //1<<0 == defaultのやつ。
        {
            if (hit.collider.CompareTag("Untagged"))
                return true; // 壁に当たった場合はtrueを返す
        }
        return false; // 壁に当たっていない場合はfalseを返す
    }
    */
}