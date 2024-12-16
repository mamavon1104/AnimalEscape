using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIImage : MonoBehaviour
{
    [SerializeField] MouseController mouseController;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] GameObject backGroundObj;
    public void PauseGame(InputAction.CallbackContext _)
    {
        playerInput.SwitchCurrentActionMap("UI");
        gameObject.SetActive(true);
        GameValueManager.Instance.worldTime = 0;
        mouseController.SetEnabledMouse(true);
        backGroundObj.SetActive(true);
    }
    public void ReturnGame()
    {
        playerInput.SwitchCurrentActionMap("Player");

        gameObject.SetActive(false);
        GameValueManager.Instance.worldTime = 1;
        mouseController.SetEnabledMouse(false);
        backGroundObj.SetActive(false);
    }
}
