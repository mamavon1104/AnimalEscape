using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ManagerSingletonBase<AudioManager>
{
    private AudioManager _audioManager;
    [SerializeField]
    private AudioSource _selectUI,_pushUI,_cancelUI, _separateUI;
    private void Start()
    {
        TryGetComponent<AudioManager>(out _audioManager);
        if(_audioManager == null)
           _audioManager = gameObject.AddComponent<AudioManager>();
    }
}
