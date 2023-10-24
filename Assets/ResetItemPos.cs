using Cysharp.Threading.Tasks;
using UnityEngine;

public class ResetItemPos : MonoBehaviour
{
    Vector3 pos;
    Transform myT;
    Transform childT;
    private void Start()
    {
        myT = transform;
        childT = myT.GetChild(0);
        pos = childT.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != childT)
            return;

        if (childT.parent != null && childT.parent.parent != null)
            childT.parent.parent.GetComponent<CatchPut_Items>().ResetOtherStateAndReleaseCatch();
        childT.parent = myT;
        childT.position = pos;
    }
}
