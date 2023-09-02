using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();

        float v = slider.value;

        //UnityAction<float> action = (float value) =>
        //{
        //    Debug.Log(value);
        //};
        //slider.onValueChanged.AddListener(action);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(slider.value);
    }
}
