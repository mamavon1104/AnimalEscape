using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CatchObjects : MonoBehaviour
{
    CatchPut_Items parentCS;
    ThrowToPoint throwToPoint;
    bool canCatch = true;
    bool nowCatch = false;
    private void Start()
    {
        var parent =transform.parent;
        parentCS = parent.GetComponent<CatchPut_Items>();
        throwToPoint = parent.GetComponent<ThrowToPoint>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (!canCatch || nowCatch)
            return;

        nowCatch = true;
        var otherTrans = other.transform;

        if (otherTrans.gameObject.layer != 0 && otherTrans.CompareTag("Player"))
        {
            parentCS.TriggerObject = otherTrans;
            parentCS.SetCatchObject();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(WaitSecond());
        throwToPoint.FinishResetVariable(this);
    }
    public IEnumerator WaitSecond()
    {
        nowCatch = false;
        canCatch = false;
        yield return new WaitForSeconds(0.5f);
        canCatch = true;
    }
}
