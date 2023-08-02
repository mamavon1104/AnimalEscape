using UnityEngine;

public class FixCameraRotation : MonoBehaviour
{
    [SerializeField, Header("�Ǐ]���������^�[�Q�b�g")]
    private Transform target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        // �Q�[���J�n���_�̃J�����ƃ^�[�Q�b�g�̋����i�I�t�Z�b�g�j���擾
        offset = transform.position - target.position;
    }

    /// <summary>
    /// �v���C���[���ړ�������ɃJ�������ړ�����悤�ɂ��邽�߂�LateUpdate�ɂ���B
    /// </summary>
    void LateUpdate()
    {
        // �J�����̈ʒu���^�[�Q�b�g�̈ʒu�ɃI�t�Z�b�g�𑫂����ꏊ�ɂ���B
        Vector3 targetPosition = target.position + offset;
        transform.position = targetPosition;

        // �J�����̉�]���^�[�Q�b�g�Ɠ����ɂ���i��]��ł������j
        transform.rotation = target.rotation;
    }
}