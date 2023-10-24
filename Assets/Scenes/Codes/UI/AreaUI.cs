using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
class AreaUI : InformationUIBase
{
    [SerializeField] UnityEvent unityEvent;
    private bool alreadyExecute = false;
    protected override void OnTriggerEnter(Collider other)
    {
        if (alreadyExecute)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (unityEvent != null)
            unityEvent.Invoke();

        var otherTrans = other.transform;
        var playerUI = PlayerInformationManager.Instance.playerSignBoardUIDic[otherTrans];        
        playerUI.ResetMyParameters(SignBoardUIAnimatorCS.Parameters.Down);

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;

        var otherTrans = other.transform;
        var playerUI = PlayerInformationManager.Instance.playerSignBoardUIDic[otherTrans];
        playerUI.ResetMyParameters(SignBoardUIAnimatorCS.Parameters.Up);
        alreadyExecute = true;

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