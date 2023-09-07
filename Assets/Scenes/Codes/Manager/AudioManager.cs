using UnityEngine;

public class AudioManager : ManagerSingletonBase<AudioManager>
{
    [SerializeField]
    private AudioSource _selectUI, _pushUI, _cancelUI, _separateUI;

    public void PlaySelectUI() => _selectUI?.Play();
    public void PlayPushUI() => _pushUI?.Play();
    public void PlayCancelUI() => _cancelUI?.Play();
    public void PlaySeparateUI() => _separateUI?.Play();
}
