using UnityEngine;

[CreateAssetMenu]
public class ObjectsValue : ScriptableObject
{
    [Header("オブジェクトが回転運動するときの変数")]
    public float speed;

    [Header("オブジェクトが回転運動するときの変数")]
    public int resolution = 12;

    [Header("物体に乗れるかどうかbool")]
    public bool canRide;

    [Header("物体に乗れるかどうかbool")]
    public bool drawOrbit;

    public enum DirectionOfRotation
    {
        [InspectorName("縦で止まる")]
        stopVertical,
        [InspectorName("横で止まる")]
        stopHorizontal,
    }
    
    public DirectionOfRotation directionOfRotation;
}
