using UnityEngine;

[RequireComponent(typeof(AnimatorTriggerSetting))]
public class AnimationStartSet : MonoBehaviour
{
    private void Start()
    {
        GetComponent<AnimatorTriggerSetting>().SetAnimatorTrigger(true);
    }
}
