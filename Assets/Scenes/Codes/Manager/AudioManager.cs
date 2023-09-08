using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : ManagerSingletonBase<AudioManager>
{
    private AudioSource _AudioSource;
    [SerializeField] private AudioClip _selectUI, _pushUI, _cancelUI, _separateUI;
    
    private void Start() =>  _AudioSource = GetComponent<AudioSource>();

    public void PlayPushUI() => _AudioSource.PlayOneShot(_pushUI);
    public void PlaySelectUI() => _AudioSource.PlayOneShot(_selectUI); 
    public void PlayCancelUI() => _AudioSource.PlayOneShot(_cancelUI);
    public void PlaySeparateUI() => _AudioSource.PlayOneShot(_separateUI);
}
