using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputScript : MonoBehaviour
{
    [Header("PlayerCS"), SerializeField] PlayerCS m_playerCS;
    [Header("InputActions"), SerializeField] PlayerInput m_action;
    [Header("GameUIImage"), SerializeField] GameUIImage m_GameUIImage;
    [Header("プレイヤーの値"), SerializeField] MyPlayersValue playerValue;
    [Header("CatchObjectCS"), SerializeField] CatchPut_Items m_CatchObjectCS;
    [Header("ThrowToPointObj"), SerializeField] ThrowToPoint m_ThrowToPointCS;
    [Header("ThrwoPointMove"), SerializeField] ThrowPointMove m_ThrowPointMove;
    [Header("CinemachineVirtualCamera"), SerializeField] CinemachineVirtualCamera m_myCamera;

    private CinemachinePOV cameraPOV;
    private InputAction _move, _jump, _change, _throw, _catchPut, _pauseGame, _throwMove;

    private void Awake()
    {
        _move = m_action.currentActionMap["Move"];
        _jump = m_action.currentActionMap["Jump"];
        _throw = m_action.currentActionMap["ThrowObject"];
        _change = m_action.currentActionMap["ChangePlayer"];
        _pauseGame = m_action.currentActionMap["PauseGame"];
        _catchPut = m_action.currentActionMap["CatchAndPut"];
        _throwMove = m_action.currentActionMap["ThrowPositionMove"];
        cameraPOV = m_myCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void Setting(bool setBool)
    {
        if (setBool)
        {
            _move.canceled += m_playerCS.OnMove;
            _move.performed += m_playerCS.OnMove;
            _jump.performed += m_playerCS.OnJump;
            _catchPut.performed += m_CatchObjectCS.CatchAndPut;
            _pauseGame.performed += m_GameUIImage.PauseGame;
            _throw.performed += m_ThrowToPointCS.Select_OR_Throw;
            _change.performed += PlayerManager.Instance.ChangePlayerNum;
        }
        else
        {
            _move.canceled -= m_playerCS.OnMove;
            _move.performed -= m_playerCS.OnMove;
            _jump.performed -= m_playerCS.OnJump;
            _catchPut.performed -= m_CatchObjectCS.CatchAndPut;
            _pauseGame.performed -= m_GameUIImage.PauseGame;
            _throw.performed -= m_ThrowToPointCS.Select_OR_Throw;
            _change.performed -= PlayerManager.Instance.ChangePlayerNum;
        }
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
            _throwMove.performed += m_ThrowPointMove.OnMove;
            _throwMove.canceled += m_ThrowPointMove.OnMove;
        }
        else
        {
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
            _throwMove.performed -= m_ThrowPointMove.OnMove;
            _throwMove.canceled -= m_ThrowPointMove.OnMove;
        }
    }
}