using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    enum WhatButton
    {
        returnGame,
        loadScene,
        setActive
    }
    
    [SerializeField]
    WhatButton m_whatButton;
    private Action doClick;
    private Transform myT;
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
           case WhatButton.setActive:
                
                break;
        }
    }
    async void OnClick()
    {
        Vector3 nowScale = myT.localScale;
        await Animation();
        myT.localScale = nowScale;
        doClick();
    }
    async UniTask Animation () => await myT.DOScale(1.1f, 0.5f).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
}
