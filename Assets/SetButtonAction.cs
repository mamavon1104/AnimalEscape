using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ButtonController))]
public class SetButtonAction : MonoBehaviour
{
    [SerializeField]
    UnityEvent _unityEvent;
    async void Awake()
    {
        await UniTask.Yield();

        ButtonController buttonController = GetComponent<ButtonController>();

        buttonController.AddAction(_unityEvent);
    }
}
