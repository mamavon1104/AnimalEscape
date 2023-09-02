using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInformationManager : ManagerSingletonBase<PlayerInformationManager>
{
    /// <summary>
    /// PlayerCS,PlayerParent��dic
    /// </summary>
    public Dictionary<PlayerCS, Transform> playerParentsDic = new Dictionary<PlayerCS, Transform>();

    /// <summary>
    /// PlayerTransform,SignBoardUI��dic
    /// </summary>
    public Dictionary<Transform, SignBoardUIAnimatorCS> playerSignBoardUIDic = new Dictionary<Transform, SignBoardUIAnimatorCS>();

    /// <summary>
    /// PlayerTransform,TextMeshUI��dic
    /// </summary>
    public Dictionary<Transform, TextMeshProUGUI> playerUITextDic = new Dictionary<Transform, TextMeshProUGUI>();

    /// <summary>
    /// PlayerTransform,playerInputScript
    /// </summary>
    public Dictionary<Transform, PlayerInputScript> inputScriptDic = new Dictionary<Transform, PlayerInputScript>();
}