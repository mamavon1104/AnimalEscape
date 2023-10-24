using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderUnityEvent : MonoBehaviour
{
    [SerializeField] UnityEvent unityEvent;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            unityEvent.Invoke();
    }
}
