using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class StepOnABox : MonoBehaviour
{
    private Vector3 myPosPreview;
    private List<PlayerCs> playerCs = new List<PlayerCs>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerCs playerScript = other.transform.GetComponent<PlayerCs>();
            
            if (!playerCs.Contains(playerScript))
            {
                playerCs.Add(playerScript);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerCs playerScript = other.transform.GetComponent<PlayerCs>();

            if (playerCs.Contains(playerScript))
            {
                playerCs.Remove(playerScript);
            }
        }
    }
    private void OnTriggerStay()
    {
        if (playerCs.Count == 0) //âΩÇ‡Ç»Ç¢èÛë‘Ç»ÇÁ
            return;

        Vector3 myPos = transform.position;
        PlayerMove(myPos - myPosPreview);
        myPosPreview = myPos;
    }
    private void PlayerMove(Vector3 getVec)
    {
        foreach (PlayerCs playerScript in playerCs)
        {
            playerScript.PlayerMove(getVec);
        }
    }
}
