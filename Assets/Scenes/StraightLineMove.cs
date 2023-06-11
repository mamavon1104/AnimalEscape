using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StraightLineMove : MonoBehaviour
{
    [Header("static�̃I�u�W�F�N�g�����A�Q�[���J�n���ɔ��a�����܂�ד����Ă����Ȃ�"), SerializeField]
    private Transform getTrans;

    [Header("Object�̐��l�ׁ̈A�g�p����"), SerializeField]
    private StraightObjValue objValue;

    [Header("�����̎q�I�u�W�F�N�g�����Ă���"), SerializeField]
    private GameObject parentObj;

    private (Vector3 my, Vector3 centerpos) points; //mypos��centerpos�A

    private float length;  //�����B
    private bool isStop = false;
    private bool canStop = true;
    private float stopedTime = 0.0f;

    private void Awake()
    {
        points = (transform.position, getTrans.position);
        SwitchLength();

        if(objValue.stopPos == StraightObjValue.StopPos.stop)
            canStop = true;

        if (objValue.canRide)
            parentObj.SetActive(true);
        else
            parentObj.SetActive(false);
    }
    private void Update()
    {
        // ���݂̈ʒu���v�Z
        int angle = (int)((Time.time - stopedTime) * objValue.speed);

        if (!isStop)
            transform.position = GetPos(angle);
        else
            stopedTime += Time.deltaTime;
        

        switch (objValue.stopPos)
        {
            case StraightObjValue.StopPos.dontStop:
                break;

            case StraightObjValue.StopPos.stop:
                isThisObjStop(Mathf.Sin(angle * Mathf.Deg2Rad));
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (!objValue.drawOrbit) //bool�ŕ`���Ȃ��ꍇ
            return;

        #region �����`���ꍇ�A�K�v�ȏ��null�ɂȂ��Ă��܂��̂Ŏ擾��������B

        if (points.my == null || points.centerpos == null)
            points = (transform.position, getTrans.position);

        if (!(Gizmos.color == Color.green))
            Gizmos.color = Color.green;// �M�Y���̐F��ݒ�
        #endregion

        if (!Application.isPlaying)
        {
            if (points.my != transform.position || points.centerpos != getTrans.position)
            {
                points.centerpos = getTrans.position;
                points = (transform.position, points.centerpos);
                SwitchLength();
            }
        }


        Vector3 DrawCenterPos = Vector3.zero,
                DrawmyPos     = Vector3.zero;

        //x,y,z�̕�����2�{���Ă�����
        switch (objValue.directions)
        {
            case StraightObjValue.GoToDirections.x:
                DrawCenterPos = new Vector3(points.centerpos.x * 2,points.centerpos.y, points.centerpos.z);
                DrawmyPos = new Vector3(points.my.x, points.centerpos.y, points.centerpos.z);
                break;

            case StraightObjValue.GoToDirections.y:
                DrawCenterPos = new Vector3(points.centerpos.x, points.centerpos.y * 2, points.centerpos.z);
                DrawmyPos = new Vector3(points.centerpos.x,points.my.y, points.centerpos.z);
                break;

            case StraightObjValue.GoToDirections.z:
                DrawCenterPos = new Vector3(points.centerpos.x, points.centerpos.y, points.centerpos.z * 2);
                DrawmyPos = new Vector3(points.centerpos.x,points.centerpos.y,points.my.z);
                break;
            
            default:
                break;
        }
        
        Gizmos.DrawLine(DrawmyPos,DrawCenterPos);
    }

    /// <summary>
    /// x,y,z�̗񋓌^enum�����Ĉ�����������Ԃ�
    /// </summary>
    private void SwitchLength()
    {
        switch (objValue.directions)
        {
            case StraightObjValue.GoToDirections.x:
                length = points.centerpos.x - transform.position.x;
                break;

            case StraightObjValue.GoToDirections.y:
                length = points.centerpos.y - transform.position.y;
                break;

            case StraightObjValue.GoToDirections.z:
                length = points.centerpos.z - transform.position.z;
                break;
        }
    }
    
    private Vector3 GetPos(float angle)
    {
        float sinPos = Mathf.Sin(angle * Mathf.Deg2Rad);
        float position;
        switch (objValue.directions)
        {
            case StraightObjValue.GoToDirections.x:
                position = points.centerpos.x + length * sinPos;
                return new Vector3(position, points.centerpos.y, points.centerpos.z);

            case StraightObjValue.GoToDirections.y:
                position = points.centerpos.y + length * sinPos;
                return new Vector3(points.centerpos.x, position, points.centerpos.z);

            case StraightObjValue.GoToDirections.z:
                position = points.centerpos.z + length * sinPos;
                return new Vector3(points.centerpos.x, points.centerpos.y, position);

            default:
                return Vector3.zero;
        }
    }
    private void isThisObjStop(float getPos)
    {
        if ((getPos == -1 || getPos == 1) && canStop)
        {
            canStop = false;
            StartCoroutine(WaitSeconds());
        }
        else if ((getPos != -1 && getPos != 1) && !canStop)
        {
            canStop = true;
        }
    }
    private IEnumerator WaitSeconds()
    {
        isStop = true;
        yield return new WaitForSeconds(objValue.StopTime);
        isStop = false;
    }
}
