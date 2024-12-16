using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjValue/CircleObjValue")]
public class CircleObjValue : ObjectsValueBase
{
    [Header("objの軌道の変数")]
    public int resolution = 12;

    public enum StopPos
    {
        [InspectorName("上下で止まる")]
        stopVertical,
        [InspectorName("左右で止まる")]
        stopHorizontal,
        [InspectorName("止まらない")]
        dontStop
    }

    [Header("何処で止まります？")]
    public StopPos stopPos;
}
