using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
class AreaUI : InformationUIBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        var otherTrans = other.transform;
        var playerUI = PlayerInformationManager.Instance.playerSignBoardUIDic[otherTrans];        
        playerUI.ResetMyParameters(SignBoardUIAnimatorCS.Parameters.Down);

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        var otherTrans = other.transform;
        var playerUI = PlayerInformationManager.Instance.playerSignBoardUIDic[otherTrans];
        playerUI.ResetMyParameters(SignBoardUIAnimatorCS.Parameters.Up);

        base.OnTriggerEnter(other);
    }
    protected override void DisplayInformationUI(Transform otherT) 
    {
        ChangeText(otherT);
    }
    protected override void HideInformationUI(Transform otherT)
    {
        
    }
}