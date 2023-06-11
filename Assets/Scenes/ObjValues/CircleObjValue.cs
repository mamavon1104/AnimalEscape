using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjValue/CircleObjValue")]
public class CircleObjValue : ObjectsValueBase
{
    [Header("objÇÃãOìπÇÃïœêî")]
    public int resolution = 12;

    public enum StopPos
    {
        [InspectorName("è„â∫Ç≈é~Ç‹ÇÈ")]
        stopVertical,
        [InspectorName("ç∂âEÇ≈é~Ç‹ÇÈ")]
        stopHorizontal,
        [InspectorName("é~Ç‹ÇÁÇ»Ç¢")]
        dontStop
    }

    [Header("âΩèàÇ≈é~Ç‹ÇËÇ‹Ç∑ÅH")]
    public StopPos stopPos;
}
