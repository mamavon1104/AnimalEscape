using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIImage : MonoBehaviour
{
    public void PauseGame(InputAction.CallbackContext _)
    {
        gameObject.SetActive(true);
        GameValueManager.Instance.WorldTime = 0;
    }
    public void ReturnGame()
    {
        GameValueManager.Instance.WorldTime = 1;
        gameObject.SetActive(false);
    }
}
