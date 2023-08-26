using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIImage : MonoBehaviour
{
    public void PauseGame(InputAction.CallbackContext _)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void ReturnGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
