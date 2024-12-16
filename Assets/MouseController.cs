using UnityEngine;

public class MouseController : MonoBehaviour
{
    void Start()
    {
        SetEnabledMouse(false);
    }

    public void SetEnabledMouse(bool enableMouse)
    {
        Cursor.visible = enableMouse;
    }
}
