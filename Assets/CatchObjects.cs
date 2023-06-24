using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObjects : MonoBehaviour
{
    CatchPut_Items parentCS;
    [SerializeField] bool canCatch = true;
    private void Start()
    {
        parentCS = transform.parent.GetComponent<CatchPut_Items>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var otherTrans = other.transform;
        if(otherTrans.gameObject.layer != 0 && otherTrans.tag == "Player" && canCatch)
        {
            parentCS.triggerObject = otherTrans;
            parentCS.SetCatchObject();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(WaitSecond());
    }
    public IEnumerator WaitSecond()
    {
        canCatch = false;
        yield return new WaitForSeconds(0.5f);
        canCatch = true;
    }
}
