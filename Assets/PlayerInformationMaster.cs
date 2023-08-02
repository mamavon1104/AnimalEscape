using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformationMaster : MonoBehaviour
{
    public static PlayerInformationMaster instance;
    public Dictionary<PlayerCS,Transform> playerParentsDic = new Dictionary<PlayerCS, Transform>();
    private void Awake()
    {
        playerParentsDic.Clear();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
