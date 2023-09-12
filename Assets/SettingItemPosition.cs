using Cysharp.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;

public class SettingItemPosition : MonoBehaviour
{
    [SerializeField] Transform m_gamePlayer;
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

        Vector2 playerPosition = new Vector2(m_gamePlayer.position.x, m_gamePlayer.position.z);
        for (int i = 0; i < childrenTrans.Length; i++)
        {
            var child = childrenTrans[i];
            child.position = new Vector3(playerPosition.x, child.position.y, playerPosition.y);
        }
    }
}
