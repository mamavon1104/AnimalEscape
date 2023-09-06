using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameValueManager : ManagerSingletonBase<GameValueManager>
{
    public float musicSoundValue = 1;
    public float soundEffectValue = 1;

    [SerializeField]
    private float worldTime = 1;
    public float WorldTime
    {
        private get { return worldTime; }
        set
        {
            ChangeWorldTimeValue(Mathf.Clamp01(value));
        }
    }
    private async void ChangeWorldTimeValue(float clampedTargetTime)
    {
        float nowTime = Time.deltaTime;
        while (Mathf.Abs(clampedTargetTime - worldTime) <= 0.01f)
        {
            worldTime = Mathf.Lerp(worldTime, clampedTargetTime, nowTime);
            nowTime += Time.deltaTime;
            await UniTask.Yield();
        }
        worldTime = clampedTargetTime;
    }

    public void SetMusicSound(float getnum) => musicSoundValue = getnum;
    public void SetSoundEffect(float getnum) => soundEffectValue = getnum;

}
