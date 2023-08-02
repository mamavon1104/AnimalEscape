using System;
using UnityEngine;

public class IsFinishThrow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
            return;

        if (other.CompareTag("Player"))
            other.GetComponent<PlayerCS>().ChangeState(PlayerCS.PlayerState.Falling);
    }
}
