using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_TextMeshProUGUI;

    Slider slider;
    
    void Start()
    {
        slider = GetComponent<Slider>();

        UnityAction<float> action = (float value) =>
        {
             m_TextMeshProUGUI.text = value.ToString("F2");
        };
        slider.onValueChanged.AddListener(action);
    }
}
