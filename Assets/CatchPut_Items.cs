using UnityEngine;
using UnityEngine.InputSystem;

public class CatchPut_Items : MonoBehaviour
{
    [Header("PlayerInput"), SerializeField]
    private PlayerInput action;
    private InputAction catchAndPutAction,throwAction;

    [Header("�����̐^��̃|�W�V�����B"), SerializeField]
    private Transform myUpTrans;
    [Header("PlayerParent"), SerializeField]
    private Transform playerParent;

    public  Transform triggerObject = null; //�g���K�[�ɓ������Ă���Ȃ�擾�A����łȂ����Null;
    private Transform catchObject = null; //���{�^�����������Ƃ��ɕۊǂ���obj;
    private bool isCatchingNow = false; //�����Ă��邩�H
    
    private Transform myT; //����Trans�A�L���b�V���p�B

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
        if (otherTrans == triggerObject) //�������������̂�other�������ꍇ
            triggerObject = null; //�ǂ��o���B
    }

    public void SetCatchObject()
    {
        catchObject = triggerObject;                             //�擾����Obj��
        catchObject.transform.parent = myUpTrans;                //�e��up�ɂ��܂��B
        catchObject.transform.position = myUpTrans.position;   //�ʒu��^�ォ�痣����
        catchObject.transform.rotation = myUpTrans.rotation;   //��]�������ɂ��Ă�邩��ȁB
        catchObject.GetComponent<Rigidbody>().isKinematic = true;

        if (catchObject.tag == "Player")
        {
            var otherPlayerCS = catchObject.GetComponent<PlayerCs>();
            otherPlayerCS.ChangeState(PlayerCs.PlayerState.BeingCarried);
            otherPlayerCS.catchPutItemsCSOfParent = this;
        }
        isCatchingNow = true; //�F�Ⴂ�L���b�`�@�ȒP�Ł@���I
    }
        
    /// <summary>
    /// ���ꂪ�Ăяo����Ă��鎞�̓I�u�W�F�N�g�̈ʒu�𒼂���ԃ��Z�b�g������B
    /// </summary>
    public void ResetOtherObjPos()
    {
        catchObject.transform.position = myT.position;        //�ʒu�𒼂���
        catchObject.transform.rotation = myT.rotation;        //��]���߂�
        ResetOtherStateAndReleaseCatch();
    }

    /// <summary>
    /// SetOtherOBjPos����Ȃ��A���ꂪ�P�̂ŌĂ΂�Ă���ꍇ�A�ʒu�͑���̃R�[�h�Œ����A�L���b�`���Ă��锻�������������
    /// </summary>
    public void ResetOtherStateAndReleaseCatch()
    {
        catchObject.parent = null;                    //�e��߂��܂��B
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
