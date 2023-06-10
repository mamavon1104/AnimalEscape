using UnityEngine;

[CreateAssetMenu]
public class ObjectsValue : ScriptableObject
{
    [Header("オブジェクトのスピード")]
    public float speed;

    [Header("objの軌道の変数")]
    public int resolution = 12;

    [Header("物体に乗れるかどうかbool")]
    public bool canRide;

    [Header("軌道を書く？")]
    public bool drawOrbit;

    [Header("止まる時間")]
    public float StopTime;

    public enum StopRotation
    {
        [InspectorName("縦で止まる")]
        stopVertical,
        [InspectorName("横で止まる")]
        stopHorizontal,
        [InspectorName("止まらない")]
        dontStop
    }
    
    public StopRotation stopRotation;
}
