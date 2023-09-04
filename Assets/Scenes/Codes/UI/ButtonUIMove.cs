using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    enum WhatButton
    {
        returnGame,
        resetScene,
        loadScene,
        enableObj,
        disableObj,
    }
    
    [SerializeField]
    private WhatButton m_whatButton;
    [SerializeField]
    private UnityEvent doClick = null;
    private Transform myT;
    [Header("enumÇ™EnableDisableÇÃèÍçáÇÃÇ›égóp"),SerializeField]
    private GameObject m_SetActiveObject;
    private void Start()
    {
        myT = transform;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);

        TryGetComponent<ChangeSceneMaster>(out var buttonLoadGame);

        switch (m_whatButton)
        {
           case WhatButton.returnGame: 
                Assert.IsNull(m_SetActiveObject);
                doClick.AddListener(myT.parent.GetComponent<GameUIImage>().ReturnGame);
                break;
           
           case WhatButton.resetScene:
                Assert.IsNull(m_SetActiveObject);
                buttonLoadGame.SceneObject = SceneManager.GetActiveScene().name;
                doClick.AddListener(() => buttonLoadGame.RoadScene());
                break;

           case WhatButton.loadScene:
                Assert.IsNull(m_SetActiveObject);
                doClick.AddListener(() => buttonLoadGame.RoadScene());
                break;

           case WhatButton.enableObj:
           case WhatButton.disableObj:
                Assert.IsNotNull(m_SetActiveObject);
                var objActive = m_whatButton == WhatButton.enableObj ? true : false;
                doClick.AddListener(() => m_SetActiveObject.SetActive(objActive));
                m_SetActiveObject.SetActive(false);
                break;
        }
    }
    async void OnClick()
    {
        Vector3 nowScale = myT.localScale;
        await Animation();
        myT.localScale = nowScale;
    }
    async UniTask Animation() => await myT.DOScale(1.2f, 0.1f).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
}
    