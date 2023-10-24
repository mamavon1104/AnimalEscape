using System.Collections.Generic;
using UnityEngine;

public class ItemInformationManager : ManagerSingletonBase<ItemInformationManager>
{
    /// <summary>
    /// item , parent
    /// </summary>
    public Dictionary<Transform, Transform> itemParentDic = new Dictionary<Transform, Transform>();
}
