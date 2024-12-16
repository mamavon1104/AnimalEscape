using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : ManagerSingletonBase<AudioManager>
{
    [SerializeField] AudioSource soundEffectAudioSource;
    [SerializeField] AudioClip _pushUI, _cancelUI, _jump, _catch, _put;
    [SerializeField] AudioClip[] _selectUI;
    private void Awake()
    {
        base.Awake();
    }

    public void PlayPushUI() => soundEffectAudioSource.PlayOneShot(_pushUI);
    public void PlayCancelUI() => soundEffectAudioSource.PlayOneShot(_cancelUI);
    public void PlayJump(InputAction.CallbackContext _) => soundEffectAudioSource.PlayOneShot(_jump);
    public void PlayPutAudio(InputAction.CallbackContext _) => soundEffectAudioSource.PlayOneShot(_put);
    public void PlayCatchAudio(InputAction.CallbackContext _) => soundEffectAudioSource.PlayOneShot(_catch);
    public void PlaySelectUI()
    {
        int i = Random.Range(0, _selectUI.Length);
        soundEffectAudioSource.PlayOneShot(_selectUI[i]);
    }
}
