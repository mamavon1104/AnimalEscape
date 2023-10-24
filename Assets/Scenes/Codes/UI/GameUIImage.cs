using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIImage : MonoBehaviour
{
    [SerializeField]
    PlayerInput playerInput;
    public void PauseGame(InputAction.CallbackContext _)
    {
        playerInput.SwitchCurrentActionMap("UI");
        gameObject.SetActive(true);
        GameValueManager.Instance.worldTime = 0;
    }
    public void ReturnGame()
    {
        playerInput.SwitchCurrentActionMap("Player");

        gameObject.SetActive(false);
            GameValueManager.Instance.worldTime = 1;
    }
}
