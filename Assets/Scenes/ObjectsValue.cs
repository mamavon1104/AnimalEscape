using UnityEngine;

[CreateAssetMenu]
public class ObjectsValue : ScriptableObject
{
    [Header("�I�u�W�F�N�g����]�^������Ƃ��̕ϐ�")]
    public float speed;

    [Header("�I�u�W�F�N�g����]�^������Ƃ��̕ϐ�")]
    public int resolution = 12;

    [Header("���̂ɏ��邩�ǂ���bool")]
    public bool canRide;

    [Header("���̂ɏ��邩�ǂ���bool")]
    public bool drawOrbit;

    public enum DirectionOfRotation
    {
        [InspectorName("�c�Ŏ~�܂�")]
        stopVertical,
        [InspectorName("���Ŏ~�܂�")]
        stopHorizontal,
    }
    
    public DirectionOfRotation directionOfRotation;
}
