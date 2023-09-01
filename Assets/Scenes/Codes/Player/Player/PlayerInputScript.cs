using Cinemachine;
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
    [Header("ThrowToPointObj"), SerializeField]
    ThrowToPoint m_ThrowToPointCS;
    [Header("ThrowToPointObj"), SerializeField]
    GameUIImage m_GameUIImage;
    [Header("CinemachineVirtualCamera"), SerializeField]
    CinemachineVirtualCamera myCamera;
    CinemachinePOV cameraPOV;

    [Header("プレイヤーの値"), SerializeField]
    private MyPlayersValue playerValue;

    [Header("プレイヤーのInputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _move, _jump, _throw, _submit, _catchPut, _pauseGame;
    private void Awake()
    {
        _move = action.currentActionMap["Move"];    
        _jump = action.currentActionMap["Jump"];
        _submit = action.currentActionMap["Submit"]; 
        _throw = action.currentActionMap["ThrowObject"];
        _catchPut = action.currentActionMap["CatchAndPut"];
        _pauseGame = action.currentActionMap["PauseGame"];
        cameraPOV = myCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void Setting(bool setBool)
    {
        if (setBool)
        {
            _move.canceled += m_playerCs.OnMove;
            _move.performed += m_playerCs.OnMove;
            _jump.performed += m_playerCs.OnJump;
            _pauseGame.performed += m_GameUIImage.PauseGame;
            _catchPut.performed += m_CatchObjectCS.CatchAndPut;
            _throw.performed += m_ThrowToPointCS.Select_OR_Throw;
        }
        else
        {
            _move.canceled -= m_playerCs.OnMove;
            _move.performed -= m_playerCs.OnMove;
            _jump.performed -= m_playerCs.OnJump;
            _pauseGame.performed -= m_GameUIImage.PauseGame;
            _catchPut.performed -= m_CatchObjectCS.CatchAndPut;
            _throw.performed -= m_ThrowToPointCS.Select_OR_Throw;
        }
        Debug.Log(setBool);
    }

    /// <summary>
    /// もしtrueなら動かし、falseなら動かさん。
    /// </summary>
    public void ChangeCameraMove(bool setBool)
    {
        if (setBool)
        {
            cameraPOV.m_VerticalAxis.m_MaxSpeed = playerValue.verticalCameraSpeed;
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = playerValue.horizontalCameraSpeed;
        }
        else
        {
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
        }
    }
}