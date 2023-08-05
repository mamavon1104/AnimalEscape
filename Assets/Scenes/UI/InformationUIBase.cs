using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InformationUIBase : MonoBehaviour
{
    [Header("�����������b�Z�[�W�B"), SerializeField]
    private string message;
    [Header("�L�����o�X���ɂ���u�C���[�W�g�v"), SerializeField]
    protected TextMeshProUGUI informationText;
    protected Transform informationTextParent; //�e�L�X�g�̐e�A�����𓮂����āAtext��text�𑗂�B  

    protected void Start()
    {
        informationTextParent = informationText.transform.parent;
    }
    protected virtual void OnTriggerEnter(Collider other) => DisplayInformationUI();
    protected virtual void OnTriggerExit(Collider other) => HideInformationUI();

    // �v�Z�Ȃǂ����Ă���邱�Ƃ�����
    protected abstract void DisplayInformationUI();
    protected abstract void HideInformationUI();

    protected void ChangeText() => informationText.text = message;
}
