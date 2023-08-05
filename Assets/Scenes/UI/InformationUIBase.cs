using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InformationUIBase : MonoBehaviour
{
    [Header("書きたいメッセージ。"), SerializeField]
    private string message;
    [Header("キャンバス内にある「イメージ枠」"), SerializeField]
    protected TextMeshProUGUI informationText;
    protected Transform informationTextParent; //テキストの親、こいつを動かして、textにtextを送る。  

    protected void Start()
    {
        informationTextParent = informationText.transform.parent;
    }
    protected virtual void OnTriggerEnter(Collider other) => DisplayInformationUI();
    protected virtual void OnTriggerExit(Collider other) => HideInformationUI();

    // 計算などをしてくれることを期待
    protected abstract void DisplayInformationUI();
    protected abstract void HideInformationUI();

    protected void ChangeText() => informationText.text = message;
}
