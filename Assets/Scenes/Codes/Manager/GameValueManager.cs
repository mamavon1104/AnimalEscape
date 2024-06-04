using UnityEngine;

public class GameValueManager : ManagerSingletonBase<GameValueManager>
{
    public bool isPlayer2 = false;
    public float musicSoundValue = 1;
    public float soundEffectValue = 1;
    public float worldTime = 1;
    [SerializeField] AudioSource soundEffectAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    protected override void Awake()
    {
        base.Awake(); // 親クラスの実装を呼び出す
        // ここに追加の初期化処理を記述
    }

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
