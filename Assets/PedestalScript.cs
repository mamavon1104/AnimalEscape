using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PedestalScript : ObjectTransformBase
{
    [SerializeField] UnityEvent myUniEvent;
    Rigidbody rb;
    private async void OnTriggerStay(Collider other)
    {
        if (didIt == true)
            return;

        if (!other.CompareTag("CatchItems"))
            return;

        if (rb == null)
            rb = other.GetComponent<Rigidbody>();

        if (rb.isKinematic)
            return;

        var otherT = other.transform;
        otherT.parent = transform;
        await SetItemTransforms(otherT);
        myUniEvent.Invoke();
        didIt = true;
    }

    private async UniTask SetItemTransforms(Transform otherT)
    {
        // オブジェクトのサイズを1.418033に設定
        otherT.DOScale(new Vector3(1.418033f, 1.418033f, 1.418033f), animationSpeed);

        // オブジェクトの位置を(0, 0.6090164, 0)に設定
        otherT.DOLocalMove(new Vector3(0f, 0.6090164f, 0f), animationSpeed);

        // オブジェクトの回転を(0, -45, 0)に設定
        otherT.DOLocalRotate(new Vector3(0f, -45f, 0f), animationSpeed);

        await UniTask.Delay(ReturnFixSecond());
    }
}
