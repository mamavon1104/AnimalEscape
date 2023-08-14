using UnityEngine;
using UnityEngine.Assertions;

public class SetPlayerInformation : MonoBehaviour
{
    [SerializeField]
    private SignBoardUIAnimatorCS mySBUI;
    void Start()
    {
        var myTrans = transform;
        Assert.IsTrue(myTrans.TryGetComponent<PlayerCS>(out var playerCS), "PlayerCS‚ªnull");

        PlayerInformationMaster.instance.playerParentsDic.Add(playerCS, myTrans.parent);
        PlayerInformationMaster.instance.playerSignBoardUIDic.Add(myTrans, mySBUI);
    }
}
