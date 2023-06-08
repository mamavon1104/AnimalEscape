using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class CircularMotion : MonoBehaviour
{
    [Header("staticのオブジェクト推奨、ゲーム開始時に半径が決まる為動いても問題なし"), SerializeField]
    private Transform getTrans;
    
    [Header("Objectの数値の為、使用する"), SerializeField]
    private ObjectsValue objValue;
    //enumでpublic、選択可能 ⇒ overrideとかで四角、〇運動、斜め運動　、

    [Header("自分の子オブジェクトを入れておく"), SerializeField]
    private GameObject parentObj;

    private float radius;  // 半径
    private bool isStop = false;
    private bool canStop = true;
    private float stopedTime = 0.0f;


    private (Vector3 my, Vector3 centerpos) points; //myposとcenterpos、


    private void Awake()
    {
        points = (transform.position, getTrans.position);
        radius = Vector2.Distance(points.my, points.centerpos);

        if (objValue.canRide)
            parentObj.SetActive(true);
        else
            parentObj.SetActive(false);
    }
    private void Update()
    {
        // 現在の位置を計算
        int angle = (int)((Time.time - stopedTime) * objValue.speed);

        if (!isStop)
            transform.position = (Vector2)GetPointOnCircle(angle);
        else
        {
            stopedTime += Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        if (!objValue.drawOrbit) //boolで描かない場合
            return;

        #region もし描く場合、必要な情報がnullになってしまうので取得し続ける。
        
        if (points.my == null || points.centerpos == null)
            points = (transform.position, getTrans.position);

        if (!(Gizmos.color == Color.green))
            Gizmos.color = Color.green;// ギズモの色を設定
        #endregion

        if (!Application.isPlaying)
        {
            if (points.my != transform.position || points.centerpos != getTrans.position)
            {
                points.centerpos = getTrans.position;
                radius = Vector2.Distance(transform.position, points.centerpos);
                points = (transform.position, points.centerpos);
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

        if ((calculateSin == -1 || calculateSin == 1) && canStop)
        {
            canStop = false;
            StartCoroutine(CaluculateTimes());
        }
        else if ((calculateSin > -1 && calculateSin < 1) && !canStop)
        {
            if (isStop)
                isStop = false;
            canStop = true;
        }

        return new Vector3(x, y, z);
    }

    IEnumerator CaluculateTimes()
    {
        isStop = true;
        yield return new WaitForSeconds(3.0f);
        isStop = false;
    }
}
