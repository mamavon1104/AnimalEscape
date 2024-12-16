using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInformationManager : ManagerSingletonBase<PlayerInformationManager>
{
    /// <summary>
    /// PlayerTransform,IsCatchBool
    /// </summary>
    public Dictionary<Transform,bool> isPlayerCatchedDic = new Dictionary<Transform,bool>();
    /// <summary>
    /// PlayerCS,PlayerParentのdic
    /// </summary>
    public Dictionary<PlayerCS, Transform> playerParentsDic = new Dictionary<PlayerCS, Transform>();

    /// <summary>
    /// PlayerTransform,SignBoardUIのdic
    /// </summary>
    public Dictionary<Transform, SignBoardUIAnimatorCS> playerSignBoardUIDic = new Dictionary<Transform, SignBoardUIAnimatorCS>();

    /// <summary>
    /// PlayerTransform,TextMeshUIのdic
    /// </summary>
    public Dictionary<Transform, TextMeshProUGUI> playerUITextDic = new Dictionary<Transform, TextMeshProUGUI>();

    /// <summary>
    /// PlayerTransform,playerInputScript
    /// </summary>
    public Dictionary<Transform, PlayerInputScript> inputScriptDic = new Dictionary<Transform, PlayerInputScript>();
}
