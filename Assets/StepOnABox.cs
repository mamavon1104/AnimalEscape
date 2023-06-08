using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StepOnABox : MonoBehaviour
{
    private Vector3 myPosPreview;

    List<PlayerCs> playerCs = new List<PlayerCs>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (playerCs.Count == 0) //�����Ȃ���ԂȂ�
                myPosPreview = transform.position; //�O�t���[��pos�����ݒn

            PlayerCs playerScript = other.transform.GetComponent<PlayerCs>();
            
            if (!playerCs.Contains(playerScript))
                playerCs.Add(playerScript);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerCs playerScript = other.transform.GetComponent<PlayerCs>();

            if (playerCs.Contains(playerScript))
                playerCs.Remove(playerScript);
        }
    }
    private void OnTriggerStay()
    {
        Vector3 myPos = transform.position;
        foreach (PlayerCs playerScript in playerCs)
        {
            playerScript.PlayerMove(myPos - myPosPreview);
        }
        myPosPreview = myPos;
        Debug.Log(myPos - myPosPreview);
    }
}
