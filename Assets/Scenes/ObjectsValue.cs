using UnityEngine;

[CreateAssetMenu]
public class ObjectsValue : ScriptableObject
{
    [Header("�I�u�W�F�N�g�̃X�s�[�h")]
    public float speed;

    [Header("obj�̋O���̕ϐ�")]
    public int resolution = 12;

    [Header("���̂ɏ��邩�ǂ���bool")]
    public bool canRide;

    [Header("�O���������H")]
    public bool drawOrbit;

    [Header("�~�܂鎞��")]
    public float StopTime;

    public enum StopRotation
    {
        [InspectorName("�c�Ŏ~�܂�")]
        stopVertical,
        [InspectorName("���Ŏ~�܂�")]
        stopHorizontal,
        [InspectorName("�~�܂�Ȃ�")]
        dontStop
    }
    
    public StopRotation stopRotation;
}
