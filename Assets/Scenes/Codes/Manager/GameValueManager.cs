using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameValueManager : ManagerSingletonBase<GameValueManager>
{
    public bool isPlayer2 = false;
    public float musicSoundValue = 1;
    public float soundEffectValue = 1;
    public float worldTime = 1;

    [SerializeField] AudioSource soundEffectAudioSource;
    [SerializeField] AudioSource musicAudioSource;


    public void SetMusicSound(float getnum) 
    {
        musicSoundValue = getnum;
        musicAudioSource.volume = getnum;
    }
    public void SetSoundEffect(float getnum)
    {
        soundEffectValue = getnum;
        soundEffectAudioSource.volume = getnum;
    }
}
