using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderScript : MonoBehaviour
{
    [SerializeField] GameValueManager gameManager;
    [SerializeField] TextMeshProUGUI m_TextMeshProUGUI;

    private Slider slider;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameValueManager>();
        //TryGetComponent<GameValueManager>(out var gameManager);

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
