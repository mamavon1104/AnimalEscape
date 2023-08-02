using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerInputScript : MonoBehaviour
{
    [Header("PlayerCS"), SerializeField]
    PlayerCS m_playerCs;
    [Header("CatchObjectCS"), SerializeField]
    CatchPut_Items m_CatchObjectCS;
    [Header("CatchObjectCS"), SerializeField]
    ThrowToPoint m_ThrowToPointCS;

    [Header("ÉvÉåÉCÉÑÅ[ÇÃInputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _move, _jump, _throw, _catchPut;
    private void Awake()
    {
        _move = action.currentActionMap["Move"];    
        _jump = action.currentActionMap["Jump"];
        _throw = action.currentActionMap["ThrowObject"];
        _catchPut = action.currentActionMap["CatchAndPut"];
    }

    public void Setting(bool setBool)
    {
        if (setBool)
        {
            _move.canceled += m_playerCs.OnMove;
            _move.performed += m_playerCs.OnMove;
            _jump.performed += m_playerCs.OnJump;
            _throw.performed += m_ThrowToPointCS.Select_OR_Throw;
            _catchPut.performed += m_CatchObjectCS.CatchAndPut;
        }
        else
        {
            _move.canceled -= m_playerCs.OnMove;
            _move.performed -= m_playerCs.OnMove;
            _jump.performed -= m_playerCs.OnJump;
            _throw.performed -= m_ThrowToPointCS.Select_OR_Throw;
            _catchPut.performed -= m_CatchObjectCS.CatchAndPut;
        }

        Debug.Log(setBool);
    }
}