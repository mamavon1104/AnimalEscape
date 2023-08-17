using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class SetPlayerInformation : MonoBehaviour
{
    [SerializeField]
    private SignBoardUIAnimatorCS mySBUI;
    [SerializeField]
    private TextMeshProUGUI text; 
    void Start()
    {
        var myTrans = transform;
        Assert.IsTrue(myTrans.TryGetComponent<PlayerCS>(out var playerCS), "PlayerCS‚ªnull");

        PlayerInformationMaster.instance.playerParentsDic.Add(playerCS, myTrans.parent);
        PlayerInformationMaster.instance.playerSignBoardUIDic.Add(myTrans, mySBUI);
        PlayerInformationMaster.instance.playerUITextDic.Add(myTrans, text);
    }
}
