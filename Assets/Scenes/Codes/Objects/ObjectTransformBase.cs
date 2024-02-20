using UnityEngine;

public class ObjectTransformBase : MonoBehaviour
{
    [SerializeField] protected float animationSpeed = 3;
    protected bool didIt = false;
    protected int ReturnFixSecond()
    {
        return (int)(animationSpeed * 1000);
    }
}
