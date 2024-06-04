using UnityEngine;

[CreateAssetMenu(menuName = "ObjValue/StraightObjValue")]
public class StraightObjValue : ObjectsValueBase
{
    public enum GoToDirections
    {
        [InspectorName("xの方向へ")]
        x,
        [InspectorName("yの方向へ")]
        y,
        [InspectorName("zの方向へ")]
        z
    }

    public enum StopPos
    {
        [InspectorName("端で止まる")]
        stop,
        [InspectorName("止まらない")]
        dontStop
    }

    [Header("Objが考える、向かうべき方向は？")]
    public GoToDirections directions;
    [Header("何処で止まります？")]
    public StopPos stopPos;
}
