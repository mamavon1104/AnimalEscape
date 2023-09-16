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
        // �I�u�W�F�N�g�̃T�C�Y��1.418033�ɐݒ�
        otherT.DOScale(new Vector3(1.418033f, 1.418033f, 1.418033f), animationSpeed);

        // �I�u�W�F�N�g�̈ʒu��(0, 0.6090164, 0)�ɐݒ�
        otherT.DOLocalMove(new Vector3(0f, 0.6090164f, 0f), animationSpeed);

        // �I�u�W�F�N�g�̉�]��(0, -45, 0)�ɐݒ�
        otherT.DOLocalRotate(new Vector3(0f, -45f, 0f), animationSpeed);

        await UniTask.Delay(ReturnFixSecond());
    }
}
