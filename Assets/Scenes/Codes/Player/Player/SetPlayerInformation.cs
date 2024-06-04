using TMPro;
using UnityEngine;

public class SetPlayerInformation : MonoBehaviour
{
    [SerializeField]
    private SignBoardUIAnimatorCS mySBUI;
    [SerializeField]
    private TextMeshProUGUI text;
    private void Start()
    {
        var myTrans = transform;
        var playerCS = myTrans.GetComponent<PlayerCS>();
        var inputCS = myTrans.GetComponent<PlayerInputScript>();

        PlayerInformationManager.Instance.playerUITextDic.Add(myTrans, text);
        PlayerInformationManager.Instance.inputScriptDic.Add(myTrans, inputCS);
        PlayerInformationManager.Instance.isPlayerCatchedDic.Add(myTrans, false);
        PlayerInformationManager.Instance.playerSignBoardUIDic.Add(myTrans, mySBUI);
        PlayerInformationManager.Instance.playerParentsDic.Add(playerCS, myTrans.parent);
    }
}
