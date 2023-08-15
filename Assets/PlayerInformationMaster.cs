using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InspecterSetDic : MonoBehaviour
{
    [SerializeField]
    int i;
}
public class PlayerInformationMaster : MonoBehaviour
{
    public static PlayerInformationMaster instance;

    /// <summary>
    /// PlayerCS,PlayerParent��dic
    /// </summary>
    public Dictionary<PlayerCS,Transform> playerParentsDic = new Dictionary<PlayerCS, Transform>(); 
    
    /// <summary>
    /// PlayerTransform,SignBoardUI��dic
    /// </summary>
    public Dictionary<Transform,SignBoardUIAnimatorCS> playerSignBoardUIDic = new Dictionary<Transform, SignBoardUIAnimatorCS>();
    
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