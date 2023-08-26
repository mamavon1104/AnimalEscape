using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValueManager : MonoBehaviour
{
    public float uiMoveValue;
    public float musicSoundValue; 
    public float soundEffectValue; 
    public static GameValueManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetUIMove(float getnum)=> uiMoveValue= getnum;
    public void SetMusicSound(float getnum)=> musicSoundValue = getnum;
    public void SetSoundEffect(float getnum)=> soundEffectValue= getnum;
}
