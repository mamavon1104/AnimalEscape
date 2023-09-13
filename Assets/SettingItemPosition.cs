using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

public class SettingItemPosition : MonoBehaviour
{
    [SerializeField] Transform itemPosition;
    private Transform[] childrenTrans = null;
    private void Start()
    {
        var myT = transform;
        childrenTrans = new Transform[myT.childCount];
        
        for (int i = 0; i < childrenTrans.Length; i++)
            childrenTrans[i] = myT.GetChild(i);
    }
    private async void Update()
    {
        for (int i = 0; i < 8; i++)
            await UniTask.Yield();

        Vector3 playerPosition = new Vector3(itemPosition.position.x, itemPosition.position.y, itemPosition.position.z);
        for (int i = 0; i < childrenTrans.Length; i++)
        {
            var child = childrenTrans[i];
            child.position = new Vector3(playerPosition.x, child.position.y, playerPosition.z);
            if(child.CompareTag("Effect"))
                child.position = new Vector3(playerPosition.x, playerPosition.y-0.8f, playerPosition.z);
        }
    }
}
