using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIPointEnter : MonoBehaviour
{
    [SerializeField] GameObject[] _trueObj;
    [SerializeField] GameObject _selectObj;
    [SerializeField] EventSystem m_eventSystem; // EventSystemに対しての参照
    private bool selected = false;
    AudioManager _audioManager;

    private void Start()
    {
        var director = GameObject.FindGameObjectWithTag("GameManager");
        _audioManager = director.GetComponent<AudioManager>();
    }
    private void Update()
    {
        // このゲームオブジェクトがカーソルによって選択されているか？
        bool hoverOver = m_eventSystem.currentSelectedGameObject == this.gameObject;
        if (selected == hoverOver)  // 前の状態との比較
            return;

        selected = hoverOver;
        if (selected)
            ResetOBJActive(true);
        else
            ResetOBJActive(false);
    }
    private void ResetOBJActive(bool getBool)
    {
        foreach (var obj in _trueObj)
            obj.SetActive(getBool);
    }
}