using UnityEngine;

public class SetItemInformation : MonoBehaviour
{
    private void Start()
    {
        ItemInformationManager.Instance.itemParentDic.Add(transform,transform.parent);
    }
}
