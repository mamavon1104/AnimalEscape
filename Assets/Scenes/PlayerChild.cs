using System.Collections.Generic;
using UnityEngine;

public class PlayerChild : MonoBehaviour
{
    private MyPlayersValue playerValue = null;
    private PlayerCs parentPlayer = null;
    private PlayerCs otherPlayerScript;
    private void Start()
    {
        parentPlayer = transform.parent.GetComponent<PlayerCs>();
        playerValue = parentPlayer.playerValue;
    }

    private void Update()
    {
        if (playerValue == null)
            return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
            otherPlayerScript = other.transform.GetComponent<PlayerCs>();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
            otherPlayerScript = null;
    }
}