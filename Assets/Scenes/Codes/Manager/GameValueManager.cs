using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameValueManager : ManagerSingletonBase<GameValueManager>
{
    public bool isPlayer2 = false;
    public float musicSoundValue = 1;
    public float soundEffectValue = 1;

    [SerializeField]
    public float worldTime = 1;

    public void SetMusicSound(float getnum) => musicSoundValue = getnum;
    public void SetSoundEffect(float getnum) => soundEffectValue = getnum;

}
