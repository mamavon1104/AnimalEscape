using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler
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

    [SerializeField]
    private WhatButton m_whatButton;
    private Action doClick = null;

    private Transform myT;
    [Header("enumÇ™EnableDisableÇÃèÍçáÇÃÇ›égóp"), SerializeField]
    private GameObject m_SetActiveObject;
    private void Awake()
    {
        myT = transform;
        GetComponent<Button>().onClick.AddListener(() => OnClick());
        TryGetComponent<ChangeSceneMaster>(out var buttonLoadGame);

        switch (m_whatButton)
        {
            case WhatButton.returnGame:
                Assert.IsNull(m_SetActiveObject);
                doClick += myT.parent.GetComponent<GameUIImage>().ReturnGame;
                break;

            case WhatButton.resetScene:
                Assert.IsNull(m_SetActiveObject);
                buttonLoadGame.SceneObject = SceneManager.GetActiveScene().name;
                doClick += buttonLoadGame.RoadScene;
                break;

            case WhatButton.loadScene:
                Assert.IsNull(m_SetActiveObject);
                doClick += buttonLoadGame.RoadScene;
                break;

            case WhatButton.enableObj:
            case WhatButton.disableObj:
                Assert.IsNotNull(m_SetActiveObject);
                (bool objActive,Action PlayAudio) = m_whatButton == WhatButton.enableObj ?
                    (true , (Action) AudioManager.Instance.PlayPushUI):
                    (false, (Action) AudioManager.Instance.PlayCancelUI);
                doClick = () => m_SetActiveObject.SetActive(objActive);
                break;
        }
    }
    async void OnClick()
    {
        Debug.Log("nnnn");
        Vector3 nowScale = myT.localScale;
        await Animation();
        doClick();
        myT.localScale = nowScale;
    }
    async UniTask Animation() => await myT.DOScale(1.2f, 0.1f).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
    public void AddAction(UnityEvent setAction) => doClick += setAction.Invoke;
    public void OnPointerEnter(PointerEventData eventData) => AudioManager.Instance.PlaySelectUI();
}
