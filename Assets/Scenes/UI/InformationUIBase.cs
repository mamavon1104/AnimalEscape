using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InformationUIBase : MonoBehaviour
{
    [Header("書きたいメッセージ。"), SerializeField]
    private string message;
    
    protected virtual void OnTriggerEnter(Collider other) => DisplayInformationUI(other.transform);
    protected virtual void OnTriggerExit(Collider other) => HideInformationUI(other.transform);

    //animationを動かすことを期待
    protected abstract void DisplayInformationUI(Transform otherT);
    protected abstract void HideInformationUI(Transform otherT);

    //protected void ChangeText() => PlayerInformationMaster.instance.playerSignBoardUIDic[0] = message;
    protected void ChangeText(Transform otherT) => PlayerInformationMaster.instance.playerUITextDic[otherT].text = message;
}
