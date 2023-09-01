using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIPointEnter : MonoBehaviour
{
    [SerializeField] GameObject[] _trueObj;
    [SerializeField] GameObject _selectObj;
    [SerializeField] EventSystem m_eventSystem; // EventSystem�ɑ΂��Ă̎Q��
    private bool selected = false;
    AudioManager _audioManager;

    private void Start()
    {
        var director = GameObject.FindGameObjectWithTag("GameManager");
        _audioManager = director.GetComponent<AudioManager>();
    }
    private void Update()
    {
        // ���̃Q�[���I�u�W�F�N�g���J�[�\���ɂ���đI������Ă��邩�H
        bool hoverOver = m_eventSystem.currentSelectedGameObject == this.gameObject;
        if (selected == hoverOver)  // �O�̏�ԂƂ̔�r
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