using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using NUnit.Framework;
using System;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    enum WhatButton
    {
        returnGame,
        loadScene,
        enableObj,
        disableObj,
    }
    
    [SerializeField]
    private WhatButton m_whatButton;
    private Action doClick;
    private Transform myT;
    [Header("enumÇ™EnableDisableÇÃèÍçáÇÃÇ›égóp"),SerializeField]
    private GameObject m_SetActiveObject;
    private void Awake()
    {
        myT = transform;
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        switch (m_whatButton)
        {
           case WhatButton.returnGame: 
                doClick = myT.parent.GetComponent<GameUIImage>().ReturnGame;
                break;
           case WhatButton.loadScene:
                
                break;
           case WhatButton.enableObj:
           case WhatButton.disableObj:
                var objActive = m_whatButton == WhatButton.enableObj ? true : false;
                doClick = () => m_SetActiveObject.SetActive(objActive);
                Assert.IsNull(m_SetActiveObject);
                break;
        }
    }
    async void OnClick()
    {
        Vector3 nowScale = myT.localScale;
        await Animation();
        doClick();
    }
    async UniTask Animation() => await myT.DOScale(1.2f, 0.1f).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
}
    