using UnityEngine;
using UnityEngine.Events;

public class AreaInvokeEvent : MonoBehaviour
{
    [SerializeField] UnityEvent unityEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (unityEvent != null)
            unityEvent.Invoke();
    }
}
