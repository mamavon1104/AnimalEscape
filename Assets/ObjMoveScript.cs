using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ObjMoveScript : ObjectTransformBase
{
    [SerializeField] Transform trans;
    [SerializeField] GameObject finishAnimationDisableObj;
    public async void StartMove()
    {
        finishAnimationDisableObj.SetActive(true);
        transform.DOLocalMove(trans.position, animationSpeed);
        await UniTask.Delay(ReturnFixSecond());
        finishAnimationDisableObj.SetActive(false);
    }
}
