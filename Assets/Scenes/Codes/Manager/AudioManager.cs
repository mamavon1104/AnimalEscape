using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : ManagerSingletonBase<AudioManager>
{
    [SerializeField] AudioSource soundEffectAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] private AudioClip _selectUI, _pushUI, _cancelUI, _jump;
    

    public void PlayPushUI() => soundEffectAudioSource.PlayOneShot(_pushUI);
    public void PlaySelectUI() => soundEffectAudioSource.PlayOneShot(_selectUI); 
    public void PlayCancelUI() => soundEffectAudioSource.PlayOneShot(_cancelUI);
    public void PlayJump() => soundEffectAudioSource.PlayOneShot(_jump);
}
