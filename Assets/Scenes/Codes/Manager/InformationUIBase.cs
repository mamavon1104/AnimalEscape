using TMPro;
using UnityEngine;

public abstract class InformationUIBase : MonoBehaviour
{
    [Header("�����������b�Z�[�W�B"), SerializeField]
    private float messageSize = 20;
    [Header("�����������b�Z�[�W�B"), SerializeField]
    private string[] message;
    
    protected virtual void OnTriggerEnter(Collider other) => DisplayInformationUI(other.transform);
    protected virtual void OnTriggerExit(Collider other) => HideInformationUI(other.transform);

    //animation�𓮂������Ƃ�����
    protected abstract void DisplayInformationUI(Transform otherT);
    protected abstract void HideInformationUI(Transform otherT);

    //protected void ChangeText() => PlayerInformationMaster.instance.playerSignBoardUIDic[0] = message;
    protected void ChangeText(Transform otherT)
    {
        TextMeshProUGUI playerText = PlayerInformationManager.Instance.playerUITextDic[otherT];
        playerText.text = "";
        playerText.fontSize = messageSize;
        for (int i = 0;i < message.Length; i++)
        {
            playerText.text += message[i];
            if (i + 1 != message.Length) playerText.text += "\n";
        }
    }
}
