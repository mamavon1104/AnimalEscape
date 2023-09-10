using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SetPlayerInformation : MonoBehaviour
{
    [SerializeField]
    private SignBoardUIAnimatorCS mySBUI;
    [SerializeField]
    private TextMeshProUGUI text; 
    private void Start()
    {
        var myTrans = transform;
        Assert.IsTrue(myTrans.TryGetComponent<PlayerCS>(out var playerCS), "PlayerCS‚ªnull");
        Assert.IsTrue(myTrans.TryGetComponent<PlayerInputScript>(out var inputCS), "InputCS‚ªnull");

        PlayerInformationManager.Instance.playerUITextDic.Add(myTrans, text);
        PlayerInformationManager.Instance.inputScriptDic.Add(myTrans, inputCS);
        PlayerInformationManager.Instance.isPlayerCatchedDic.Add(myTrans, false);
        PlayerInformationManager.Instance.playerSignBoardUIDic.Add(myTrans, mySBUI);
        PlayerInformationManager.Instance.playerParentsDic.Add(playerCS, myTrans.parent);
    }
}
