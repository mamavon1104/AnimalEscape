using UnityEngine;
using TMPro;

class SignBoard : MonoBehaviour
{
    [Header("書きたいメッセージ。"), SerializeField]
    private string message;

    [Header("キャンバス内にある「イメージ枠」"), SerializeField]
    private TextMeshProUGUI text;
    private Transform textImageParent; //テキストの親、こいつを動かして、textにtextを送る。  

    void Start()
    {
        textImageParent = text.transform.parent;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        //textImage;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;


    }
}