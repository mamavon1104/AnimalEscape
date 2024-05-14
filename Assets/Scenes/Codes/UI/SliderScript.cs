using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Slider))]
public class SliderScript : MonoBehaviour
{
    [SerializeField] GameValueManager gameManager;
    [SerializeField] TextMeshProUGUI m_TextMeshProUGUI;
    
    Slider slider;
    
    void Start()
    {
        slider = GetComponent<Slider>();

        UnityAction<float> action = (float value) =>
        {
            m_TextMeshProUGUI.text = value.ToString("F3");

            if (transform.CompareTag("SESlider"))
                gameManager.SetSoundEffect(value);

            if (transform.CompareTag("MusicSlider"))
                gameManager.SetMusicSound(value);
        };
        slider.onValueChanged.AddListener(action);
    }
}
