using UnityEngine;

public class ObjectsValueBase : ScriptableObject
{
    [Header("�I�u�W�F�N�g�̃X�s�[�h")]
    public float speed;

    [Header("���̂ɏ��邩�ǂ���bool")]
    public bool canRide;

    [Header("�~�܂鎞��"), SerializeField]
    public float stopTime;
}
