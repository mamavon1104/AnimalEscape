using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    /// PlayerCS,PlayerParent‚Ìdic
    /// </summary>
    public Dictionary<PlayerCS,Transform> playerParentsDic = new Dictionary<PlayerCS, Transform>(); 
    
    /// <summary>
    /// PlayerTransform,SignBoardUI‚Ìdic
    /// </summary>
    public Dictionary<Transform,SignBoardUIAnimatorCS> playerSignBoardUIDic = new Dictionary<Transform, SignBoardUIAnimatorCS>();

    /// <summary>
    /// PlayerTransform,TextMeshUI‚Ìdic
    /// </summary>
    public Dictionary<Transform,TextMeshProUGUI> playerUITextDic = new Dictionary<Transform, TextMeshProUGUI>();
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
