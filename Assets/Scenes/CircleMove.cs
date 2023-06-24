using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class CircleMove : MonoBehaviour
{
    [Header("staticのオブジェクト推奨、ゲーム開始時に半径が決まる為動いても問題なし"), SerializeField]
    private Transform getTrans;
    private Transform myTrans;

    [Header("Objectの数値の為、使用する"), SerializeField]
    private CircleObjValue objValue;

    [Header("自分の子オブジェクトを入れておく"), SerializeField]
    private GameObject rideCollider;
    private StepOnABox stepOnAbox;

    private (Vector3 my, Vector3 centerpos) points; //myposとcenterpos、

    private float radius;  // 半径
    private bool isStop = false;
    private bool canStop = true;
    private float stopedTime = 0.0f;

    private void Start()
    {
        myTrans = transform;
        points = (myTrans.position, getTrans.position);
        radius = Vector2.Distance(points.my, points.centerpos);

        
        if (objValue.canRide)
        {
            rideCollider.SetActive(true);
            stepOnAbox = rideCollider.GetComponent<StepOnABox>();
        }
        else
            rideCollider.SetActive(false);
    }
    private void Update()
    {
        // 現在の位置を計算
        int angle = (int)((Time.time - stopedTime) * objValue.speed);

        var circlePos = GetPointOnCircle(angle);

        if (!isStop)
            myTrans.position = circlePos;
        else
            stopedTime += Time.deltaTime;

        switch (objValue.stopPos)
        {
            case CircleObjValue.StopPos.dontStop: 
                break;
                    
            case CircleObjValue.StopPos.stopVertical:
                isThisObjStop(Mathf.Sin(angle * Mathf.Deg2Rad));
                break;

            case CircleObjValue.StopPos.stopHorizontal:
                isThisObjStop(Mathf.Cos(angle * Mathf.Deg2Rad));
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (!objValue.drawOrbit) //boolで描かない場合
            return;

        #region もし描く場合、必要な情報がnullになってしまうので取得し続ける。
        if(myTrans == null)
            myTrans = transform;

        if (points.my == null || points.centerpos == null)
            points = (myTrans.position, getTrans.position);

        if (!(Gizmos.color == Color.green))
            Gizmos.color = Color.green;// ギズモの色を設定
        #endregion

        if (!Application.isPlaying)
        {
            if (points.my != myTrans.position || points.centerpos != getTrans.position)
            {
                points.centerpos = getTrans.position;
                radius = Vector2.Distance(myTrans.position, points.centerpos);
                points = (myTrans.position, points.centerpos);
            }
        }

        // 円の描画
        float angleIncrement = 360f / objValue.resolution;
        float currentAngle = 0f;
        Vector3 anglePrev = GetPointOnCircle(currentAngle);

        for (int i = 0; i < objValue.resolution; i++)
        {
            currentAngle += angleIncrement;
            Vector3 nextPoint = GetPointOnCircle(currentAngle);
            Gizmos.DrawLine(anglePrev, nextPoint);

            anglePrev = nextPoint;
        }
    }

    private Vector3 GetPointOnCircle(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;
        float calculateSin = Mathf.Sin(radians);
        float calculateCos = Mathf.Cos(radians);

        float x = points.centerpos.x + radius * calculateCos;
        float y = points.centerpos.y + radius * calculateSin;
        float z = points.centerpos.z;

        return new Vector3(x, y, z);
    }

    private void isThisObjStop(float getPos)
    {
        if ((getPos == -1 || getPos == 1) && canStop)
        {
            canStop = false;
            StartCoroutine(WaitSeconds());
        }
        else if((getPos != -1 && getPos != 1) && !canStop)
        {
            canStop = true;
        }
    }
    private IEnumerator WaitSeconds()
    {
        isStop = true;
        yield return new WaitForSeconds(objValue.stopTime);
        isStop = false;
    }
}
