using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorTriggerSetting : MonoBehaviour
{
    [SerializeField] string myTriggerName;
    public void SetAnimatorTrigger(bool isTrue)
    {
        var anim = GetComponent<Animator>();
        if (isTrue)
            anim.SetTrigger(myTriggerName);
        else
            anim.ResetTrigger(myTriggerName);
    }
}
