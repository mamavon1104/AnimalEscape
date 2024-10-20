using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
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
        Assert.IsNotNull(_unityEvent, "Nullです、取り外すか追加とかどうにかして。");

        ButtonController buttonController = GetComponent<ButtonController>();

        buttonController.AddAction(_unityEvent);
    }
}
