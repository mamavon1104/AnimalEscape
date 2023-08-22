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

        PlayerInformationMaster.instance.playerUITextDic.Add(myTrans, text);
        PlayerInformationMaster.instance.inputScriptDic.Add(myTrans, inputCS);
        PlayerInformationMaster.instance.playerSignBoardUIDic.Add(myTrans, mySBUI);
        PlayerInformationMaster.instance.playerParentsDic.Add(playerCS, myTrans.parent);
    }
}
