using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    enum WhatButton
    {
        [Header("�Q�[���ɖ߂�")] returnGame,
        [Header("�V�[�������[�h�A�܂�߂�c�Ƃ��Q�[���J�n�Ƃ�...")] loadScene,
        [Header("�I�u�W�F�N�g�̃Z�b�g�A�N�e�B�u�ɂ��āB")] setActive
    }
    
    [SerializeField]
    private WhatButton m_whatButton;
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

        Debug.Log("a");

        doClick();
    }
    async UniTask Animation() => await myT.DOScale(1.2f, 0.2f).SetEase(Ease.OutElastic).AsyncWaitForCompletion();
}
    