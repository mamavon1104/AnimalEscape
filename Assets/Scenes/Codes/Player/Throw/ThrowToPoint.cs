using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


public class ThrowToPoint : MonoBehaviour
{
    [Header("������|�W�V����"), SerializeField]
    private Transform throwPointNow;
    private Vector3 startPos; // �ړ��̊J�n�ʒu

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

    [Header("������Ƃ��ɕύX������Camera"), SerializeField]
    GameObject thisCamera;
    
    [Header("�X�t�B�A�R���C�_�[�A����"),SerializeField]
    GameObject sphereColliderObj;
   
    [Header("Player�̒l(PlayerValue)"), SerializeField]
    private MyPlayersValue playerValue;

    void Start()
    {
        myT = transform.parent;
        catchPutItemsCS = transform.GetComponent<CatchPut_Items>();
        FinishTriggerParent = GameObject.FindGameObjectWithTag("FinishTriggerParent"); //find�͎g��Ȃ��B
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

        if (!SelectThrow) //�I�΂�ĂȂ��ꍇ�͑I��
        {
            SelectThrow = true;
            thisCamera.gameObject.SetActive(true);
            throwPointNow.gameObject.SetActive(true);
            PlayerInformationManager.Instance.inputScriptDic[myT].ChangeCameraMove(false);
        }
        else//�I�΂ꂽ��͓�����
        {
            startPos = objTrans.position;

            //������ׂ�rigidbody,�����Ă锻����폜�A��
            catchPutItemsCS.ResetOtherStateAndReleaseCatch();

            // �ˏo���x���Z�o
            Vector3 velocity = CalculateVelocity(startPos, throwPointNow.position, playerValue.throwAngle);
            //��΂�
            objTrans.GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
            
            //�����������ǂ������肷�邽�߂�obj�쐬
            GameObject sphereObjs = GetObjectFromPool();
            sphereObjs.transform.position = throwPointNow.position;
            
            FinishResetVariable(this);
        }
    }

    private GameObject GetObjectFromPool()
    {
        // ��A�N�e�B�u�ȃI�u�W�F�N�g���������ĕԂ�
        foreach (GameObject obj in FinishTriggerList)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // �v�[�����̂��ׂẴI�u�W�F�N�g���g�p���̏ꍇ�͐V���ɃI�u�W�F�N�g�𐶐����ĕԂ�
        var newObj = Instantiate(sphereColliderObj);
        newObj.transform.parent = FinishTriggerParent.transform;
        FinishTriggerList.Add(newObj);
        return newObj;
    }

    /// <param name="orderClass">
    /// �Ăяo�����̃N���X���W�F�l���b�N���\�b�h��case�Ŕ���
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
                Debug.LogError("�z��O�̃N���X����Ăяo�����s���Ă��܂�");
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
            // ���B�n�_�܂ł̑��x�x�N�g��
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}