using UnityEngine;
using UnityEngine.InputSystem;

public class CatchPut_Items : MonoBehaviour
{
    [Header("�����̐^��̃|�W�V�����B"), SerializeField]
    private Transform myUpTrans;
    [Header("PlayerTransform�B"), SerializeField]
    private Transform m_player;

    private Transform myT; //����Trans�A�L���b�V���p�B
    private Transform triggerObject = null; //�g���K�[�ɓ������Ă���Ȃ�擾�A����łȂ����Null;
    private Transform catchObject = null; //�ǂ���obj�A���ݎ����Ă������;]
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
            if (value.CompareTag("Untagged") || value.CompareTag("ItemPedestal"))
                return;

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
        //TriggerObject��null�ŁACatch��null�Ȃ牽�����Ȃ�����Catch�������痎�Ƃ��Ƃ��̔�����s�������B
        if (TriggerObject == null && CatchObject == null)
            return;

        if (TriggerObject != null &&
            TriggerObject.CompareTag("Player") &&
            PlayerInformationManager.Instance.isPlayerCatchedDic[m_player])
        {
            return;
        }

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
        if (other.transform == TriggerObject) //�������������̂�other�������ꍇ
            TriggerObject = null; //�ǂ��o���B
    }

    /// <summary>
    /// �����E�����̐ݒ�ACatchObjects����ĂыN�������B
    /// </summary>
    public void SetCatchObject()
    {
        CatchObject = TriggerObject;                             //�擾����Obj��

        if (CatchObject.CompareTag("Player"))
        {
            PlayerInformationManager.Instance.isPlayerCatchedDic[CatchObject] = true;
            var otherPlayerCS = CatchObject.GetComponent<PlayerCS>();
            otherPlayerCS.ChangeState(PlayerCS.PlayerState.BeingCarried);
            otherPlayerCS.CatchPutItemsCSOfParent = this;
        }
        else if (CatchObject.CompareTag("CatchItems"))
        {
            if (CatchObject.parent != null && CatchObject.parent.CompareTag("ItemPedestal"))
            {
                CatchObject = null;
                return;
            }
            if (TryGetComponent<ItemActions>(out var otherItemActions))
                otherItemActions.IsCatched = false;
        }

        CatchObject.parent = myUpTrans;                //�e��up�ɂ��܂��B
        CatchObject.position = myUpTrans.position;    //�ʒu��^�ォ�痣����
        CatchObject.rotation = myUpTrans.rotation;    //��]�������ɂ��Ă�邩��ȁB
        CatchObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// ���ꂪ�Ăяo����Ă���ꍇ:
    /// �I�u�W�F�N�g�̈ʒu�𒼂���ԃ��Z�b�g������B
    /// </summary>
    public void ResetOtherObjPos()
    {
        CatchObject.position = myT.position;        //�ʒu�𒼂���
        CatchObject.rotation = myT.rotation;        //��]���߂�
        ResetOtherStateAndReleaseCatch();
    }

    /// <summary>
    /// ResetOtherObjPos����Ȃ��A���ꂪ�P�̂ŌĂ΂�Ă���ꍇ :
    /// �ʒu�͑���̃R�[�h�Œ����A�L���b�`���Ă��锻�������
    /// </summary>
    public void ResetOtherStateAndReleaseCatch()
    {
        CatchObject.GetComponent<Rigidbody>().isKinematic = false;
        if (CatchObject.CompareTag("Player"))
        {
            PlayerInformationManager.Instance.isPlayerCatchedDic[CatchObject] = false;
            var otherPlayerCS = CatchObject.GetComponent<PlayerCS>();
            otherPlayerCS.CatchPutItemsCSOfParent = null;
            otherPlayerCS.GetComponent<PlayerCS>().ChangeState(PlayerCS.PlayerState.Falling);
            CatchObject.parent = PlayerInformationManager.Instance.playerParentsDic[otherPlayerCS]; //�e��߂��܂��B
        }
        else if (CatchObject.CompareTag("CatchItems"))
        {
            if (TryGetComponent<ItemActions>(out var otherItemActions))
                otherItemActions.IsCatched = false;

            CatchObject.parent = null;
        }
        CatchObject = null;
        TriggerObject = null;
        throwToPoint.FinishResetVariable(this);
    }
}
