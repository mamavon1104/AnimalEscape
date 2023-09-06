using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIImage : MonoBehaviour
{
    public void PauseGame(InputAction.CallbackContext _)
    {
        try
        {
            gameObject.SetActive(true);
            GameValueManager.Instance.WorldTime = 0;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void ReturnGame()
    {
        try
        {
            gameObject.SetActive(false);
            GameValueManager.Instance.WorldTime = 1;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
