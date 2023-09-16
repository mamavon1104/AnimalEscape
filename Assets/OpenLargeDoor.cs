using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class OpenLargeDoor : ObjectTransformBase
{
    [SerializeField] float rotateY;
    [SerializeField] GameObject finishAnimationDisableObj;
    public async void OpenDoor()
    {
        if (finishAnimationDisableObj != null)
Å@       Å@ finishAnimationDisableObj.SetActive(true);
        
        transform.DOLocalRotate(new Vector3(0, rotateY, 0), animationSpeed);
        await UniTask.Delay(ReturnFixSecond());
        
        if (finishAnimationDisableObj != null)
            finishAnimationDisableObj?.SetActive(false);
    }
}
