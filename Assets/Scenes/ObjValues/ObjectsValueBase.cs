using UnityEngine;

public class ObjectsValueBase : ScriptableObject
{
    [Header("オブジェクトのスピード")]
    public float speed;

    [Header("物体に乗れるかどうかbool")]
    public bool canRide;

    [Header("軌道を書く？")]
    public bool drawOrbit;

    [Header("止まる時間")]
    public float StopTime;   
}
