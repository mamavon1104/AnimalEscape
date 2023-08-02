using System.Collections.Generic;
using UnityEngine;

public class PlayerChild : MonoBehaviour
{
    private MyPlayersValue playerValue = null;
    private PlayerCS parentPlayer = null;
    private PlayerCS otherPlayerScript;
    private void Start()
    {
        parentPlayer = transform.parent.GetComponent<PlayerCS>();
        playerValue = parentPlayer.playerValue;
    }

    private void Update()
    {
        if (playerValue == null)
            return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
            otherPlayerScript = other.transform.GetComponent<PlayerCS>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
            otherPlayerScript = null;
    }
}