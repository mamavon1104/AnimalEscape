using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetOptionData : MonoBehaviour
{
    [SerializeField]
    Slider m_MusicSlider,m_SoundEfectSlider;

    public void ResetValue()
    {
        m_MusicSlider.value = 1;
        m_SoundEfectSlider.value = 1;
    }

    private void OnEnable()
    {
        m_MusicSlider.value = GameValueManager.Instance.musicSoundValue;
        m_SoundEfectSlider.value = GameValueManager.Instance.soundEffectValue;
    }
    public void SaveValues()
    {
        GameValueManager.Instance.musicSoundValue = m_MusicSlider.value;
        GameValueManager.Instance.soundEffectValue = m_SoundEfectSlider.value;
    }
}
