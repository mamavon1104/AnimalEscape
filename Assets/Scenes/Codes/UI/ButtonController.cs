using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    enum WhatButton
    {
        returnGame,
        resetScene,
        loadScene,
        enableObj,
        disableObj,
        none,
    }

    [SerializeField] private WhatButton m_whatButton;
    [SerializeField] private UnityEvent m_event;
    [SerializeField] private UnityEvent m_clickEvent;
    [SerializeField] private float m_waitTime = 0f;
    private Action doClick = null;
    private Action<float> doClick2 = null;

    private Transform myT;
    [Header("enumがEnableDisableの場合のみ使用"), SerializeField]
    private GameObject m_SetActiveObject;


    private Vector3 _buttonScale;
    private void Awake()
    {
        myT = transform;
        _buttonScale = myT.localScale;
        GetComponent<Button>().onClick.AddListener(() => OnClick());
        var buttonLoadGame = GetComponent<ChangeSceneMaster>();

        (bool objActive, Action playAudio) = m_whatButton != WhatButton.disableObj ?
            (true, (Action)AudioManager.Instance.PlayPushUI) :
            (false, (Action)AudioManager.Instance.PlayCancelUI);
        doClick += () => playAudio();

        switch (m_whatButton)
        {
            case WhatButton.returnGame:
                doClick += myT.parent.GetComponent<GameUIImage>().ReturnGame;
                break;

            case WhatButton.resetScene:
            case WhatButton.loadScene:
                if (m_whatButton == WhatButton.resetScene)
                    buttonLoadGame.SceneObject = SceneManager.GetActiveScene().name;

                doClick2 += buttonLoadGame.RoadScene;
                break;

            case WhatButton.enableObj:
            case WhatButton.disableObj:
                doClick += () => m_SetActiveObject.SetActive(objActive);
                break;
        }
    }
    async void OnClick()
    {
        Debug.Log("nnnn");
        await Animation();

        if (m_clickEvent != null)
            m_clickEvent.Invoke();

        if (doClick != null)
            doClick();

        if (doClick2 != null)
            doClick2(m_waitTime);

        myT.localScale = _buttonScale;
    }
    async UniTask Animation() => await myT.DOScale(1.2f, 0.1f).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
    public void AddAction(UnityEvent setAction) => doClick += setAction.Invoke;
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySelectUI();
        if (m_event != null)
            m_event.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData) => myT.localScale = _buttonScale;
}
