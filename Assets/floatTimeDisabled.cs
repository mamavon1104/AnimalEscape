using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class floatTimeDisabled : MonoBehaviour
{
    [SerializeField] float waitSecond;
    async void OnEnable()
    {
        await UniTask.Delay((int)(waitSecond * 1000));
        gameObject.SetActive(false);
    }
}
