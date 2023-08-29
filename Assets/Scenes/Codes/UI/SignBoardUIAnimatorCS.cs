using UnityEngine;

public class SignBoardUIAnimatorCS : MonoBehaviour
{
    Animator animator;
    private void Start() => animator = GetComponent<Animator>();
    public enum Parameters
    {
        Down,
        Up,
    }
    public void ResetMyParameters(Parameters par)
    {
        if(par == Parameters.Down)
            animator.SetTrigger("DownTrigger");
        else if(par == Parameters.Up)
            animator.SetTrigger("UPTrigger");
    }
}
