using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class ItemActions : MonoBehaviour
{
    [SerializeField] bool canDeleteOrAddObj; 
    [SerializeField] GameObject _caughtTrueObj;
    [SerializeField] GameObject _caughtFalseObj;
    private bool _isPrevFrameCaught = false;
    private bool _isCaught = false;
    public bool IsCatched
    {
        private get { return _isCaught; }
        set { _isCaught = value; }
    }
    private void Start()
    {
        if (!canDeleteOrAddObj)
            return;
        Action act = !IsCatched ? NotCaughtSetting : CaughtSetting;
        act();
    }
    private void Update()
    {
        if (IsCatched != _isPrevFrameCaught)
        {
            Action act = IsCatched ? CaughtSetting : NotCaughtSetting;
            act();
            _isPrevFrameCaught = IsCatched;
        }
    }
    private void CaughtSetting()
    {
        _caughtTrueObj?.SetActive(true);
        _caughtFalseObj?.SetActive(false);
    }
    private void NotCaughtSetting()
    {
        _caughtTrueObj?.SetActive(false);
        _caughtFalseObj?.SetActive(true);
    }
}
